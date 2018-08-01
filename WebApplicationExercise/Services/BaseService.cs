namespace WebApplicationExercise.Services
{
    using AutoMapper;

    using WebApplicationExercise.Core;

    public class BaseService
    {
        public BaseService(MainDataContext db, IMapper mapper)
        {
            this.Db = db;
            this.Mapper = mapper;
        }

        public MainDataContext Db { get; }

        public IMapper Mapper { get; }
    }
}