
using CaloryCalcApp.Web.Models.DTOs.Dishes;
using CaloryCalcApp.Web.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Mvc.Rendering; // for SelectListItem
using System.Collections.Generic;

namespace CaloryCalcApp.Web.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<ProductDto> ProductsFound { get; set; } = new List<ProductDto>();
        public List<DishDto> DishesFound { get; set; } = new List<DishDto>();
        public List<HealthyUser> UsersFound { get; set; } = new List<HealthyUser>();
    }
}

