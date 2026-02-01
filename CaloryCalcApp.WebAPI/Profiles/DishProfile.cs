using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.Dishes;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Dish, DishDTO>()
                .ReverseMap();
            // Can be used to map nested DishProducts
            //CreateMap<Dish, DishDTO>()
            //    .ForMember(dest => dest.DishProducts, opt => opt.MapFrom(src => src.DishProducts));
        }
    }
}
