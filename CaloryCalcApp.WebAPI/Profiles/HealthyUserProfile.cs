using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
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
