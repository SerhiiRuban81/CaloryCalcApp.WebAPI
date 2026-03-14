using AutoMapper;
using CaloryCalcApp.Application.DTOs.Products;
using CaloryCalcLibrary;

namespace CaloryCalcApp.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();
        }
    }
}
