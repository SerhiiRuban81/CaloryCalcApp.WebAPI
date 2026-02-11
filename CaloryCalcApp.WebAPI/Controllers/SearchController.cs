using AutoMapper;
using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.ViewModels.Search;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- for SelectListItem
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CaloryCalcApp.WebAPI.Controllers
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

        // POST: Search
        [HttpPost]
        public async Task<ActionResult> Search(SearchViewModel model)
        {
            // Let's check if our `SearchText` is not empty
            if (model != null)
            {
                if (model.SearchText != null && model.SearchType != null)
                {
                    if(model.SearchType == "Product" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse products where `Name` contains symbols from our search
                        model.ProductsFound = await _context.Products.Where(p => p.Name.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                    }
                    if (model.SearchType == "Dish" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse dishes where `Name` contains symbols from our search
                        model.DishesFound = await _context.Dishes.Where(p => p.Name.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                    }
                    if (model.SearchType == "User" || model.SearchType == "FullSearch")
                    {
                        // Let's choouse products where `Name` contains symbols from our search
                        model.UsersFound = await _userManager.Users.Where(p => p.UserName.Contains(model.SearchText)).OrderBy(p => p.Id).ToListAsync();
                    }
                }
            }

            // Example: redirect to a results page
            return View("_Search", model);
        }
    }
}
