using AutoMapper;
using CaloryCalcApp.Application.DTOs.DishProducts;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Application.Profiles
{
    public class DishProductProfile : Profile
    {
        public DishProductProfile()
        {
            CreateMap<DishProduct, DishProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));

            CreateMap<DishProductDto, DishProduct>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Dish, opt => opt.Ignore());
        }
    }
}
