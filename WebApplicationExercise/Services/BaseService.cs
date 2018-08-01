namespace WebApplicationExercise.Services
{
    using AutoMapper;

    using WebApplicationExercise.Core;

    /// <summary>
    /// Base service
    /// </summary>
    public class BaseService
    {
        public BaseService(MainDataContext db, IMapper mapper)
        {
            this.Db = db;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Database context
        /// </summary>
        public MainDataContext Db { get; }

        /// <summary>
        /// DTO mapper
        /// </summary>
        public IMapper Mapper { get; }
    }
}