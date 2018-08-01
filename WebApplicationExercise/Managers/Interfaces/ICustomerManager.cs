namespace WebApplicationExercise.Managers.Interfaces
{
    using System.Linq;

    using WebApplicationExercise.Models;

    public interface ICustomerManager
    {
        IQueryable<Order> IsCustomerVisible(IQueryable<Order> query);
    }
}