using System.Linq;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.Managers.Interfaces
{
    public interface ICustomerManager
    {
        IQueryable<Order> IsCustomerVisible(IQueryable<Order> query);
    }
}