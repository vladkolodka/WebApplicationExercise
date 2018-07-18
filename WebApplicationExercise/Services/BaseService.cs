using AutoMapper;
using WebApplicationExercise.Core;

namespace WebApplicationExercise.Services
{
    public class BaseService
    {
        public BaseService(MainDataContext db, IMapper mapper)
        {
            Db = db;
            Mapper = mapper;
        }

        public MainDataContext Db { get; }
        public IMapper Mapper { get; }
    }
}