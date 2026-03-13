using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.HealthyUserDishes;
using CaloryCalcApp.Web.Models.DTOs.HealthyUsers;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Web.Profiles
{
    public class HealthyUserDishProfile : Profile
    {
        public HealthyUserDishProfile()
        {
            CreateMap<HealthyUserDish, HealthyUserDishDto>()
                .ReverseMap();
        }
    }
}

