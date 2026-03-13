using AutoMapper;
using CaloryCalcApp.Web.Data;
using CaloryCalcApp.Web.Models.DTOs.Dishes;
using CaloryCalcApp.Web.Models.DTOs.HealthyUsers;
using CaloryCalcApp.Web.Models.DTOs.Products;
using CaloryCalcApp.Web.Models.ViewModels.Search;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly CaloriesContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<HealthyUser> _userManager;

        public SearchController(CaloriesContext context, IMapper mapper, UserManager<HealthyUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Search
        [HttpGet]
        public IActionResult Index()
        {            
            return View("_Search");
        }


        [HttpGet]
        public async Task<ActionResult> SearchAsync(string searchText, string searchType, int productPage = 1, int dishPage = 1, int userPage = 1)
        {
            var model = new SearchViewModel
            {
                SearchText = searchText,
                SearchType = searchType
            };

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(searchType))
            {
                if (searchType == "Product" || searchType == "FullSearch")
                {
                    var pf = await _context.Products
                        .Where(p => p.Name.Contains(searchText))
                        .OrderBy(p => p.Id)
                        .ToListAsync();

                    model.ProductsFound = _mapper.Map<List<ProductDto>>(pf);
                }

                if (searchType == "Dish" || searchType == "FullSearch")
                {
                    var df = await _context.Dishes
                        .Where(p => p.Name.Contains(searchText))
                        .OrderBy(p => p.Id)
                        .ToListAsync();

                    model.DishesFound = _mapper.Map<List<DishDto>>(df);
                }

                if (searchType == "User" || searchType == "FullSearch")
                {
                    model.UsersFound = await _userManager.Users
                        .Where(p => p.UserName!.Contains(searchText) || p.Email!.Contains(searchText))
                        .OrderBy(p => p.Id)
                        .ToListAsync();
                }
            }

            ViewBag.ProductPage = productPage;
            ViewBag.DishPage = dishPage;
            ViewBag.UserPage = userPage;

            return View("_Search", model);
        }



        // POST: Search
        [HttpPost]
        public async Task<ActionResult> SearchAsync(SearchViewModel model)
        {
            // Let's check if our `SearchText` is not empty
            if (model != null)
            {
                if (model.SearchText != null && model.SearchType != null)
                {
                    if (model.SearchType == "Product" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse products where `Name` contains symbols from our search
                        var pf = await _context.Products.Where(p => p.Name.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                        model.ProductsFound = _mapper.Map<List<ProductDto>>(pf);
                    }
                    if (model.SearchType == "Dish" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse dishes where `Name` contains symbols from our search
                        var df = await _context.Dishes.Where(p => p.Name.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                        model.DishesFound = _mapper.Map<List<DishDto>>(df);
                    }
                    if (model.SearchType == "User" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse products where `Name` contains symbols from our search
                        model.UsersFound = await _userManager.Users.Where(p => p.UserName!.Contains(model.SearchText) || p.Email!.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                    }
                }
            }
            // Example: redirect to a results page
            return View("_Search", model);
        }
    }
}

