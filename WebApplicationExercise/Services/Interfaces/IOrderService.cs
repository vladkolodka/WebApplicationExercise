using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationExercise.Dto;

namespace WebApplicationExercise.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> Single(Guid orderId);
        Task<OrderModel> Save(OrderModel orderModel);
        Task<List<OrderModel>> All(int pageNumber, string sortOrder, DateTime? @from, DateTime? to, string customerName);
        Task Delete(Guid orderId);
        Task<OrderModel> Update(Guid orderId, OrderModel orderModel);
    }
}