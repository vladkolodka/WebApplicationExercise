using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationExercise.Core;
using WebApplicationExercise.Models;
using WebApplicationExercise.Services.Interfaces;

namespace WebApplicationExercise.Services
{
    /// <summary>
    ///     Manages orders their products
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        /// <param name="db">Database context</param>
        public OrderService(MainDataContext db) : base(db)
        {
        }

        /// <summary>
        ///     Get order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Task<Order> Single(Guid orderId)
        {
            // single throws exception when entity doesnt exists.
            return Db.Orders.Include(o => o.Products).AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);
        }

        /// <summary>
        ///     Create new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> Save(Order order)
        {
            var savedOrder = Db.Orders.Add(order);

            await Db.SaveChangesAsync();

            return savedOrder;
        }

        /// <summary>
        ///     Update order details
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Order> Update(Guid orderId, Order order)
        {
            var dbOrder = await Db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);

            if (dbOrder == null) throw new ArgumentException("Order not found");

            dbOrder.IsVisible = order.IsVisible;
            dbOrder.CreatedDate = order.CreatedDate;
            dbOrder.Customer = order.Customer;

            if (dbOrder.Products.Any() && order.Products.Any())
            {
                var products = order.Products.ToDictionary(p => p.Id);

                foreach (var p in dbOrder.Products.Where(p => products.ContainsKey(p.Id)))
                {
                    p.Name = products[p.Id].Name;
                    p.Price = products[p.Id].Price;
                }
            }

            await Db.SaveChangesAsync();

            return dbOrder;
        }

        /// <summary>
        ///     Get all orders with optional filtering
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public Task<List<Order>> All(DateTime? from, DateTime? to, string customerName)
        {
            var orders = Db.Orders.Include(o => o.Products).AsNoTracking();

            if (from != null && to != null)
                orders = FilterByDate(orders, from.Value, to.Value);

            if (customerName != null)
                orders = FilterByCustomer(orders, customerName);

            return orders.Where(o => o.IsVisible).ToListAsync();
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