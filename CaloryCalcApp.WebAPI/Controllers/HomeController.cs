using AutoMapper;
using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
		private readonly UserManager<HealthyUser> userManager;
		private readonly IMapper mapper;
		private readonly CaloriesContext _context;

		public HomeController(UserManager<HealthyUser> userManager, IMapper mapper, CaloriesContext context)
		{
			this.userManager = userManager;
			this.mapper = mapper;
			this._context = context;
		}
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);
			if (user == null) return View();

			ViewBag.UserId = user.Id;

			ViewBag.DishId = new SelectList(
				_context.Dishes.ToList(),
				"Id",
				"Name");

			return View();
		}
     
        public IActionResult About()
        {
            return View();
        }
    }

}
