using AutoMapper;
using CaloryCalcApp.Application.DTOs.Admins;
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
    public async Task<IActionResult> RegisterAsync(RegisterUserDto Dto)
    {
        if(!ModelState.IsValid)
            return View("Register", Dto);

        HealthyUser healthyUser = _mapper.Map<HealthyUser>(Dto);
        var result = await _userManager.CreateAsync(healthyUser, Dto.Password);
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
            return View("Register", Dto);
        }
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginUserDto Dto)
    {
        if (!ModelState.IsValid)
            return View("Login", Dto);
        HealthyUser? healthyUser = await _userManager.FindByNameAsync(Dto.Username);
        if (healthyUser != null)
        {
            var result = await _signInManager.PasswordSignInAsync(healthyUser, Dto.Password,
                Dto.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError(string.Empty, "User/password is wrong");
        }
        else
            ModelState.AddModelError(string.Empty, "User not found");
        return View("Login", Dto);
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

