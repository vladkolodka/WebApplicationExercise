using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> Single(Guid orderId);
        Task<Order> Save(Order order);
        Task<List<Order>> All(DateTime? from, DateTime? to, string customerName);
        Task Delete(Guid orderId);
        Task<Order> Update(Guid orderId, Order order);
    }
}