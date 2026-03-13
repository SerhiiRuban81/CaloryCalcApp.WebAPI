using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.Admins;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalcApp.Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<HealthyUser> _userManager;
    private readonly SignInManager<HealthyUser> _signInManager;
    private readonly IMapper _mapper;

    public AccountController(UserManager<HealthyUser> userManager, 
        SignInManager<HealthyUser> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterUserDTO dto)
    {
        if(!ModelState.IsValid)
            return View("Register", dto);

        HealthyUser healthyUser = _mapper.Map<HealthyUser>(dto);
        var result = await _userManager.CreateAsync(healthyUser, dto.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(healthyUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Register", dto);
        }
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginUserDTO dto)
    {
        if (!ModelState.IsValid)
            return View("Login", dto);
        HealthyUser? healthyUser = await _userManager.FindByNameAsync(dto.Username);
        if (healthyUser != null)
        {
            var result = await _signInManager.PasswordSignInAsync(healthyUser, dto.Password,
                dto.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError(string.Empty, "User/password is wrong");
        }
        else
            ModelState.AddModelError(string.Empty, "User not found");
        return View("Login", dto);
    }
    
    public async Task<IActionResult> LogoutAsync()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

