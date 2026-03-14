using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcApp.Web.Models.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("_Search");
        }

        [HttpGet]
        public async Task<ActionResult> SearchAsync(string searchText, string searchType, int productPage = 1, int dishPage = 1, int userPage = 1)
        {
            var result = await _searchService.SearchAsync(searchText, searchType);

            var model = new SearchViewModel
            {
                SearchText = result.SearchText,
                SearchType = result.SearchType,
                ProductsFound = result.ProductsFound,
                DishesFound = result.DishesFound,
                UsersFound = result.UsersFound
            };

            ViewBag.ProductPage = productPage;
            ViewBag.DishPage = dishPage;
            ViewBag.UserPage = userPage;

            return View("_Search", model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchAsync(SearchViewModel model)
        {
            var result = await _searchService.SearchAsync(model.SearchText, model.SearchType);

            model.ProductsFound = result.ProductsFound;
            model.DishesFound = result.DishesFound;
            model.UsersFound = result.UsersFound;

            return View("_Search", model);
        }
    }
}

