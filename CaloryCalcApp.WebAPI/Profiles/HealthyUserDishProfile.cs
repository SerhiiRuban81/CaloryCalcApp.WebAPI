using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUserDishes;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class HealthyUserDishProfile : Profile
    {
        public HealthyUserDishProfile()
        {
            CreateMap<HealthyUserDish, HealthyUserDishDTO>()
                .ReverseMap();
        }
    }
}
