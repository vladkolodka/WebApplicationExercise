namespace WebApplicationExercise.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WebApplicationExercise.Dto;

    public interface IOrderService
    {
        Task<List<OrderModel>> All(
            int pageNumber,
            string sortOrder,
            DateTime? @from,
            DateTime? to,
            string customerName);

        Task Delete(Guid orderId);

        Task<OrderModel> Save(OrderModel orderModel);

        Task<OrderModel> Single(Guid orderId);

        Task<OrderModel> Update(Guid orderId, OrderModel orderModel);
    }
}