using WebApplicationExercise.Core;

namespace WebApplicationExercise.Services
{
    public class BaseService
    {
        public BaseService(MainDataContext db)
        {
            Db = db;
        }

        public MainDataContext Db { get; }
    }
}