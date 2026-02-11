using CaloryCalcLibrary;
using Microsoft.AspNetCore.Mvc.Rendering; // for SelectListItem
using System.Collections.Generic;

namespace CaloryCalcApp.WebAPI.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<Product> ProductsFound { get; set; } = new List<Product>();
        public List<Dish> DishesFound { get; set; } = new List<Dish>();
        public List<HealthyUser> UsersFound { get; set; } = new List<HealthyUser>();
    }
}
