using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.Dish;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class DishProfile : Profile
    {
        DishProfile()
        {
            CreateMap<Dish, DishDTO>()
                .ReverseMap();
        }
    }
}
