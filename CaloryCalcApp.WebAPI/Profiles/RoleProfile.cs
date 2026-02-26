using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.Roles;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();
        }
    }
}
