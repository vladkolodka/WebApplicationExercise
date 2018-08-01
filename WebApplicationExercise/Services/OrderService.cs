﻿namespace WebApplicationExercise.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using WebApplicationExercise.Core;
    using WebApplicationExercise.Dto;
    using WebApplicationExercise.Managers.Interfaces;
    using WebApplicationExercise.Models;
    using WebApplicationExercise.Services.Interfaces;

    /// <summary>
    ///     Manages orders their products
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        // TODO to ask if it should be in the config
        private const int OrdersCountOnPage = 2;

        private readonly ICustomerManager manager;

        /// <param name="db">Database context</param>
        /// <param name="mapper"></param>
        /// <param name="manager">Customer manager</param>
        public OrderService(MainDataContext db, IMapper mapper, ICustomerManager manager)
            : base(db, mapper)
        {
            this.manager = manager;
        }

        /// <summary>
        ///     Get all orders with optional filtering
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="sortOrder"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> All(
            int pageNumber,
            string sortOrder,
            DateTime? from,
            DateTime? to,
            string customerName)
        {
            var orders = this.Db.Orders.Include(o => o.Products).AsNoTracking();

            if (from != null && to != null)
            {
                orders = FilterByDate(orders, from.Value, to.Value);
            }

            if (customerName != null)
            {
                orders = FilterByCustomer(orders, customerName);
            }

            // TODO consider convert to: orders = Dictionary<Delegate>[](orders) in case if new fields will be added
            switch (sortOrder?.ToLower())
            {
                case "customer_name":
                    orders = orders.OrderBy(o => o.Customer);
                    break;

                case "created_date":
                    orders = orders.OrderBy(o => o.CreatedDate);
                    break;
                case "customer_name_desc":
                    orders = orders.OrderByDescending(o => o.Customer);
                    break;

                case "created_date_desc":
                    orders = orders.OrderByDescending(o => o.CreatedDate);
                    break;
                default:
                    orders = orders.OrderBy(o => o.Id);
                    break;
            }

            var request = await this.manager.IsCustomerVisible(orders).Skip(OrdersCountOnPage * pageNumber)
                              .Take(OrdersCountOnPage).ToListAsync();

            return this.Mapper.Map<List<OrderModel>>(request);
        }

        /// <summary>
        ///     Delete order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task Delete(Guid orderId)
        {
            if (!await this.Db.Orders.AnyAsync(o => o.Id == orderId))
            {
                return;
            }

            var order = new Order { Id = orderId };

            this.Db.Orders.Attach(order);
            this.Db.Entry(order).State = EntityState.Deleted;

            await this.Db.SaveChangesAsync();
        }

        /// <summary>
        ///     Create new order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public async Task<OrderModel> Save(OrderModel orderModel)
        {
            var order = this.Mapper.Map<Order>(orderModel);

            var savedOrder = this.Db.Orders.Add(order);

            await this.Db.SaveChangesAsync();

            return this.Mapper.Map<OrderModel>(savedOrder);
        }

        /// <summary>
        ///     Get order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderModel> Single(Guid orderId)
        {
            // single throws exception when entity doesnt exists.
            return this.Mapper.Map<OrderModel>(
                await this.Db.Orders.Include(o => o.Products).AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId));
        }

        /// <summary>
        ///     Update order details v2
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OrderModel> Update(Guid orderId, OrderModel orderModel)
        {
            var databaseOrder = await this.Db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);

            if (databaseOrder == null)
            {
                throw new ArgumentException("Order not found");
            }

            this.Db.Orders.Remove(databaseOrder);

            var newDbOrder = this.Db.Orders.Add(this.Mapper.Map<Order>(orderModel));

            await this.Db.SaveChangesAsync();

            return this.Mapper.Map<OrderModel>(newDbOrder);
        }

        private static IQueryable<Order> FilterByCustomer(IQueryable<Order> orders, string customerName)
        {
            return orders.Where(o => o.Customer == customerName);
        }

        private static IQueryable<Order> FilterByDate(IQueryable<Order> orders, DateTime from, DateTime to)
        {
            return orders.Where(o => o.CreatedDate >= from && o.CreatedDate <= to);
        }
    }
}