using System.Linq;
using WebApplicationExercise.Managers.Interfaces;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private const string HiddenCustomerName = "Hidden Joe";

        public IQueryable<Order> IsCustomerVisible(IQueryable<Order> query)
        {
            return query.Where(o => o.Customer != HiddenCustomerName);
        }
    }
}