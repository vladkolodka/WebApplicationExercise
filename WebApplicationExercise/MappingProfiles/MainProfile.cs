namespace WebApplicationExercise.MappingProfiles
{
    using AutoMapper;

    using WebApplicationExercise.Dto;
    using WebApplicationExercise.Models;

    public class MainProfile : Profile
    {
        public MainProfile()
        {
            this.CreateMap<Order, OrderModel>().ForMember(m => m.CustomerName, e => e.MapFrom(o => o.Customer))
                .ReverseMap();

            this.CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}