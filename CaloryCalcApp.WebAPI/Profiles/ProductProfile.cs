using AutoMapper;
using CaloryCalcLibrary;
using CaloryCalcApp.Web.Models.DTOs.Products;

namespace CaloryCalcApp.Web.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductDto>()
                .ReverseMap();
        }        
    }
}

