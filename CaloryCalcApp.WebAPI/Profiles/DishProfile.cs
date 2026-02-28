using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.Dishes;
using CaloryCalcLibrary;
using Microsoft.EntityFrameworkCore.Design;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            //CreateMap<Dish, DishDTO>()
            //    .ReverseMap();

            // We are using following code to map data, received by our Function in DTO members
            CreateMap<Dish, DishDTO>()
                .ForMember(dest => dest.DishProducts, opt => opt.MapFrom(src => src.DishProducts))
                .ForMember(dest => dest.TotalProteins, opt => opt.MapFrom(src => src.GetGlobalProteins()))
                .ForMember(dest => dest.TotalCarbohydrates, opt => opt.MapFrom(src => src.GetGlobalCarbohydrates()))
                .ForMember(dest => dest.TotalFats, opt => opt.MapFrom(src => src.GetGlobalFats()))
                .ForMember(dest => dest.TotalCalories, opt => opt.MapFrom(src => src.GetGlobalCalories()));
        }
    }
}
