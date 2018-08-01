using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplicationExercise.Dto;
using WebApplicationExercise.Services.Interfaces;

namespace WebApplicationExercise.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Manage orders
    /// </summary>
    [RoutePrefix("api/v1/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;

        /// <inheritdoc />
        /// <summary>
        ///     Inject required service
        /// </summary>
        /// <param name="orderService"></param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        ///     Get single order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<OrderModel> GetOrder(Guid orderId)
        {
            return await _orderService.Single(orderId);
        }

        /// <summary>
        ///     Get all orders with optional filtering by date and customer name
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="sortOrder">customer_name[_desc] ; created_date[_desc]</param>
        /// <param name="from">Date from</param>
        /// <param name="to">Date to</param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route]
        public async Task<List<OrderModel>> GetOrders(int pageNumber, string sortOrder = null, DateTime? from = null,
            DateTime? to = null, string customerName = null)
        {
            return await _orderService.All(pageNumber, sortOrder, from, to, customerName);
        }

        /// <summary>
        ///     Create new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route]
        public async Task<OrderModel> SaveOrder([FromBody] OrderModel order)
        {
            return await _orderService.Save(order);
        }

        /// <summary>
        ///     Delete order
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{orderId}")]
        public async Task DeleteOrder(Guid orderId)
        {
            await _orderService.Delete(orderId);
        }

        /// <summary>
        ///     Update order and product details
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="order">Data</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{orderId}")]
        public async Task<OrderModel> UpdateOrder(Guid orderId, [FromBody] OrderModel order)
        {
            return await _orderService.Update(orderId, order);
        }
    }
}