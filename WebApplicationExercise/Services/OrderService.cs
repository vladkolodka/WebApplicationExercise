﻿namespace WebApplicationExercise.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using WebApplicationExercise.Core;
    using WebApplicationExercise.Dto;
    using WebApplicationExercise.Managers.Interfaces;
    using WebApplicationExercise.Models;
    using WebApplicationExercise.Services.Interfaces;
    using WebApplicationExercise.Utils;

    /// <summary>
    ///     Manages orders their products
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        private readonly ICurrencyConverterService currencyConverterService;

        private readonly ICustomerManager manager;

        /// <param name="db">Database context</param>
        /// <param name="mapper"></param>
        /// <param name="manager">Customer manager</param>
        /// <param name="currencyConverterService">Currency converter</param>
        public OrderService(
            MainDataContext db,
            IMapper mapper,
            ICustomerManager manager,
            ICurrencyConverterService currencyConverterService)
            : base(db, mapper)
        {
            this.manager = manager;
            this.currencyConverterService = currencyConverterService;
        }

        /// <summary>
        ///     Get all orders with optional filtering
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="currency"></param>
        /// <param name="sortOrder"></param>
        /// <param name="direction"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> All(
            int offset,
            int count,
            string currency,
            string sortOrder,
            SortOrder direction,
            DateTime? @from,
            DateTime? to,
            string customerName)
        {
            if (count > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var orders = this.Db.Orders.Include(o => o.Products).AsNoTracking();

            if (from != null && to != null)
            {
                orders = FilterByDate(orders, from.Value, to.Value);
            }

            if (customerName != null)
            {
                orders = FilterByCustomer(orders, customerName);
            }

            if (sortOrder != null)
            {
                orders = direction == SortOrder.Descending
                             ? orders.OrderByDescending(sortOrder)
                             : orders.OrderBy(sortOrder);
            }
            else
            {
                orders = orders.OrderBy(o => o.Id);
            }

            var result = this.Mapper.Map<List<OrderModel>>(
                await this.manager.IsCustomerVisible(orders).Skip(offset).Take(count).ToListAsync());

            if (currency != null)
            {
                await this.ConvertPrice(currency, result);
            }

            return result;
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
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<OrderModel> Single(Guid orderId, string currency)
        {
            // single throws exception when entity doesnt exists.
            var result = this.Mapper.Map<OrderModel>(
                await this.Db.Orders.Include(o => o.Products).AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId));

            if (currency != null)
            {
                await this.ConvertPrice(currency, new[] { result });
            }

            return result;
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

        /// <summary>
        /// Convert (update in source) order's products price from USD to target currency
        /// </summary>
        /// <param name="targetCurrency"></param>
        /// <param name="orders"></param>
        private async Task ConvertPrice(string targetCurrency, IEnumerable<OrderModel> orders)
        {
            var targetPrice = await this.currencyConverterService.GetUsdExchangeRate(targetCurrency);

            foreach (var order in orders)
            {
                if (order.Products == null)
                {
                    continue;
                }

                foreach (var product in order.Products)
                {
                    product.Price = product.Price * targetPrice;
                }
            }
        }
    }
}