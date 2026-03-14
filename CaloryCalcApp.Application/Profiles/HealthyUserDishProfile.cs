using AutoMapper;
using CaloryCalcApp.Application.DTOs.HealthyUserDishes;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Application.Profiles
{
    public class HealthyUserDishProfile : Profile
    {
        public HealthyUserDishProfile()
        {
            CreateMap<HealthyUserDish, HealthyUserDishDto>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish != null ? src.Dish.Name : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.HealthyUser != null ? src.HealthyUser.UserName : string.Empty));

            CreateMap<HealthyUserDishDto, HealthyUserDish>()
                .ForMember(dest => dest.Dish, opt => opt.Ignore())
                .ForMember(dest => dest.HealthyUser, opt => opt.Ignore());
        }
    }
}
