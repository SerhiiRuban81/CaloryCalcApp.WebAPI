using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProduct;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class DishProductProfile : Profile
    {
        DishProductProfile()
        {
            CreateMap<DishProduct, DishProductDTO>()
                .ReverseMap();
        }
    }
}
