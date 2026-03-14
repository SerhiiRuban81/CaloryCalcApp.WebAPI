
using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcApp.Application.DTOs.Products;

namespace CaloryCalcApp.Web.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<ProductDto> ProductsFound { get; set; } = new List<ProductDto>();
        public List<DishDto> DishesFound { get; set; } = new List<DishDto>();
        public List<HealthyUserDto> UsersFound { get; set; } = new List<HealthyUserDto>();
    }
}

