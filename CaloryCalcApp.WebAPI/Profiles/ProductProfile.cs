using AutoMapper;
using CaloryCalcLibrary;
using CaloryCalcApp.WebAPI.Models.DTOs.Product;

namespace CaloryCalcApp.WebAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
        }        
    }
}
