using AutoMapper;
using CaloryCalcApp.Infrastructure.Data;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserManager<HealthyUser> _userManager;
    private readonly IMapper _mapper;
    private readonly CaloriesContext _context;

    public HomeController(UserManager<HealthyUser> userManager, IMapper mapper, CaloriesContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return View("Index");

        ViewBag.UserId = user.Id;

        ViewBag.DishId = new SelectList(
            await _context.Dishes.ToListAsync(),
            "Id",
            "Name");

        return View("Index");
    }
    
    public IActionResult About()
    {
        return View();
    }
}

