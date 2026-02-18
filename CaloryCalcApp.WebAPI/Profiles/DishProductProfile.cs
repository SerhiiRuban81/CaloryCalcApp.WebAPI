using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProducts;
using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class DishProductProfile : Profile
    {
        public DishProductProfile()
        {
            CreateMap<DishProduct, DishProductDTO>()
                .ReverseMap();
        }
    }
}
