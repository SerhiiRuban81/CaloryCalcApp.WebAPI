using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.Roles;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalcApp.Web.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();
        }
    }
}

