using AutoMapper;
using CaloryCalcApp.Application.DTOs.Roles;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalcApp.Application.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
        }
    }
}
