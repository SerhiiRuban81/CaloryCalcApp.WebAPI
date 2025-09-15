using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUserDTO;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class HealthyUserProfile : Profile
    {
        public HealthyUserProfile() {
            CreateMap<HealthyUser, HealthyUserDTO>()
                .ReverseMap();
        }
    }
}
