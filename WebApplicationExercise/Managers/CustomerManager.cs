namespace WebApplicationExercise.Managers
{
    using System.Linq;

    using WebApplicationExercise.Managers.Interfaces;
    using WebApplicationExercise.Models;

    // TODO ask if it would be better to convert manager to extensions class to increase linq readability
    public class CustomerManager : ICustomerManager
    {
        private const string HiddenCustomerName = "Hidden Joe";

        public IQueryable<Order> IsCustomerVisible(IQueryable<Order> query)
        {
            return query.Where(o => o.Customer != HiddenCustomerName);
        }
    }
}