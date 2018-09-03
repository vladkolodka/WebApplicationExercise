namespace WebApplicationExercise.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using WebApplicationExercise.Dto;
    using WebApplicationExercise.Services.Interfaces;

    /// <inheritdoc />
    /// <summary>
    ///     Manage orders
    /// </summary>
    [RoutePrefix("api/v1/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService orderService;

        /// <inheritdoc />
        /// <summary>
        ///     Inject required service
        /// </summary>
        /// <param name="orderService"></param>
        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
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
            await this.orderService.Delete(orderId);
        }

        /// <summary>
        ///     Get single order
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="currency">Convert to currency, ex: UAH</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<OrderModel> GetOrder(Guid orderId, string currency = null)
        {
            return await this.orderService.Single(orderId, currency);
        }

        /// <summary>
        ///     Get all orders with optional filtering by date and customer name
        /// </summary>
        /// <param name="direction">Sorting direction</param>
        /// <param name="offset">Retrieved items offset</param>
        /// <param name="count">Retrieved items count</param>
        /// <param name="currency">Convert to currency, ex: UAH</param>
        /// <param name="sortOrder">Customer/CreatedDate/...</param>
        /// <param name="from">Date from</param>
        /// <param name="to">Date to</param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route]
        public async Task<List<OrderModel>> GetOrders(
            SortOrder direction,
            int offset = 0,
            int count = 10,
            string currency = null,
            string sortOrder = null,
            DateTime? from = null,
            DateTime? to = null,
            string customerName = null)
        {
            return await this.orderService.All(offset, count, currency, sortOrder, direction, from, to, customerName);
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
            return await this.orderService.Save(order);
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
            return await this.orderService.Update(orderId, order);
        }
    }
}