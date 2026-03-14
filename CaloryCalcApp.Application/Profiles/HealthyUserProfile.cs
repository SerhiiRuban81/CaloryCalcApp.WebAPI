using AutoMapper;
using CaloryCalcApp.Application.DTOs.Admins;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Application.Profiles
{
    public class HealthyUserProfile : Profile
    {
        public HealthyUserProfile()
        {
            CreateMap<HealthyUser, HealthyUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<HealthyUserDto, HealthyUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<RegisterUserDto, HealthyUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
        }
    }
}
