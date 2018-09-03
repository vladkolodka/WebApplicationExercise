namespace WebApplicationExercise.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using WebApplicationExercise.Dto;

    public interface IOrderService
    {
        Task<List<OrderModel>> All(
            int offset,
            int count,
            string currency,
            string sortOrder,
            SortOrder direction,
            DateTime? @from,
            DateTime? to,
            string customerName);

        Task Delete(Guid orderId);

        Task<OrderModel> Save(OrderModel orderModel);

        Task<OrderModel> Single(Guid orderId, string currency);

        Task<OrderModel> Update(Guid orderId, OrderModel orderModel);
    }
}