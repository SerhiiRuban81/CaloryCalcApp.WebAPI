using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.Admins;
using CaloryCalcApp.Web.Models.DTOs.HealthyUsers;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Web.Profiles;

public class HealthyUserProfile : Profile
{
    public HealthyUserProfile() {
        CreateMap<HealthyUser, HealthyUserDTO>()
            .ReverseMap();
        CreateMap<RegisterUserDTO, HealthyUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
    }
}

