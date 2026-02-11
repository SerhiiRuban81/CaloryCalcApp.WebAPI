
using CaloryCalcApp.WebAPI.Models.DTOs.Dishes;
using CaloryCalcApp.WebAPI.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Mvc.Rendering; // for SelectListItem
using System.Collections.Generic;

namespace CaloryCalcApp.WebAPI.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<ProductDTO> ProductsFound { get; set; } = new List<ProductDTO>();
        public List<DishDTO> DishesFound { get; set; } = new List<DishDTO>();
        public List<HealthyUser> UsersFound { get; set; } = new List<HealthyUser>();
    }
}
