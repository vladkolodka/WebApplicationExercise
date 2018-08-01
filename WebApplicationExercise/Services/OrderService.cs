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
        // TODO to ask if it should be in the config
        private const int OrdersCountOnPage = 2;
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

            var savedOrder = Db.Orders.Add(order);

            await Db.SaveChangesAsync();

            return Mapper.Map<OrderModel>(savedOrder);
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
            var dbOrder = await Db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);

            if (dbOrder == null) throw new ArgumentException("Order not found");

            Db.Orders.Remove(dbOrder);

            var newDbOrder = Db.Orders.Add(Mapper.Map<Order>(orderModel));

            await Db.SaveChangesAsync();

            return Mapper.Map<OrderModel>(newDbOrder);
        }

        /// <summary>
        ///     Get all orders with optional filtering
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> All(int pageNumber, DateTime? @from, DateTime? to, string customerName)
        {
            var orders = Db.Orders.Include(o => o.Products).AsNoTracking();

            if (from != null && to != null)
                orders = FilterByDate(orders, from.Value, to.Value);

            if (customerName != null)
                orders = FilterByCustomer(orders, customerName);

            var request = await _manager.IsCustomerVisible(orders)
                
                // TODO default OrderBy if not specified
                .OrderBy(o => o.CreatedDate)
                
                .Skip(OrdersCountOnPage * pageNumber).Take(OrdersCountOnPage)
                .ToListAsync();

            return Mapper.Map<List<OrderModel>>(request);
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