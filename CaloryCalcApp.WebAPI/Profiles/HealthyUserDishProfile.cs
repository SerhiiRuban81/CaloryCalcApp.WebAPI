using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUserDTO;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class HealthyUserDishProfile : Profile
    {
        public HealthyUserDishProfile()
        {
            CreateMap<HealthyUser, HealthyUserDTO>()
                .ReverseMap();
        }
    }
}
