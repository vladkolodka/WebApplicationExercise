using AutoMapper;
using WebApplicationExercise.Dto;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.MappingProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Order, OrderModel>()
                .ForMember(m => m.CustomerName, e => e.MapFrom(o => o.Customer))
                .ReverseMap();

            CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}