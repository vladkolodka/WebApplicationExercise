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
                .ReverseMap()
                .ForMember(o => o.Id, e => e.Ignore())
                .ForMember(o => o.Products, e => e.Ignore());

            CreateMap<Product, ProductModel>().ReverseMap()
                .ForMember(p => p.Id, e => e.Ignore());
        }
    }
}