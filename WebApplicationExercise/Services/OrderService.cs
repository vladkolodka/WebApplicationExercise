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

namespace WebApplicationExercise.Services
{
    /// <summary>
    ///     Manages orders their products
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        private readonly ICustomerManager _manager;

        /// <param name="db">Database context</param>
        /// <param name="manager">Customer manager</param>
        public OrderService(MainDataContext db, IMapper mapper, ICustomerManager manager) : base(db, mapper)
        {
            _manager = manager;
        }

        /// <summary>
        ///     Get order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderModel> Single(Guid orderId)
        {
            // single throws exception when entity doesnt exists.
            return Mapper.Map<OrderModel>(await Db.Orders.Include(o => o.Products).AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId));
        }

        /// <summary>
        ///     Create new order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public async Task<OrderModel> Save(OrderModel orderModel)
        {
            var order = Mapper.Map<Order>(orderModel);
            order.Products = Mapper.Map<List<Product>>(orderModel.Products);

            var savedOrder = Db.Orders.Add(order);

            await Db.SaveChangesAsync();

            return Mapper.Map<OrderModel>(savedOrder);
        }

        /// <summary>
        ///     Update order details
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OrderModel> Update(Guid orderId, OrderModel order)
        {
            var dbOrder = await Db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);

            if (dbOrder == null) throw new ArgumentException("Order not found");

            Mapper.Map(order, dbOrder);

            if (order.Products != null && dbOrder.Products != null && order.Products.Any() && dbOrder.Products.Any())
            {
                var modelIds = new HashSet<Guid>(order.Products.Select(p => p.Id));

                Db.Products.RemoveRange(dbOrder.Products.Where(p => !modelIds.Contains(p.Id)));

                var dbProducts = dbOrder.Products.ToDictionary(p => p.Id);

                var existingProducts = order.Products.Where(p => dbProducts.ContainsKey(p.Id)).ToDictionary(p => p.Id);

                foreach (var p in existingProducts) Mapper.Map(p.Value, dbProducts[p.Key]);

                dbOrder.Products.AddRange(
                    Mapper.Map<IEnumerable<Product>>(order.Products.Where(pm => !dbProducts.ContainsKey(pm.Id))));
            }

            await Db.SaveChangesAsync();

            return Mapper.Map<OrderModel>(dbOrder);
        }

        /// <summary>
        ///     Get all orders with optional filtering
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> All(DateTime? from, DateTime? to, string customerName)
        {
            var orders = Db.Orders.Include(o => o.Products).AsNoTracking();

            if (from != null && to != null)
                orders = FilterByDate(orders, from.Value, to.Value);

            if (customerName != null)
                orders = FilterByCustomer(orders, customerName);

            return Mapper.Map<List<OrderModel>>(await _manager.IsCustomerVisible(orders).ToListAsync());
        }

        /// <summary>
        ///     Delete order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task Delete(Guid orderId)
        {
            if (!await Db.Orders.AnyAsync(o => o.Id == orderId)) return;

            var order = new Order {Id = orderId};

            Db.Orders.Attach(order);
            Db.Entry(order).State = EntityState.Deleted;

            await Db.SaveChangesAsync();
        }

        private IQueryable<Order> AddClause(IQueryable<Order> query)
        {
            return query.Where(o => o.Customer == "123");
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