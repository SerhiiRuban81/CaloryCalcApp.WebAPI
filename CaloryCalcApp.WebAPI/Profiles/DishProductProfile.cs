using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.DishProducts;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Web.Profiles
{
    public class DishProductProfile : Profile
    {
        public DishProductProfile()
        {
            CreateMap<DishProduct, DishProductDto>()
                .ReverseMap();
        }
    }
}

