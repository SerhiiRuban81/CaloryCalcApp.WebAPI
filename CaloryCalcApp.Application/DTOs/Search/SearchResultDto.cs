using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcApp.Application.DTOs.Products;

namespace CaloryCalcApp.Application.DTOs.Search
{
    public class SearchResultDto
    {
        public string SearchText { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<ProductDto> ProductsFound { get; set; } = new();
        public List<DishDto> DishesFound { get; set; } = new();
        public List<HealthyUserDto> UsersFound { get; set; } = new();
    }
}
