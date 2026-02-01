using CaloryCalcApp.WebAPI.Models.DTOs.Admin;
using CaloryCalcApp.WebAPI.Models.DTOs.Admins;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalcApp.WebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<HealthyUser> userManager;
        private readonly SignInManager<HealthyUser> signInManager;

        public AccountController(UserManager<HealthyUser> userManager, 
            SignInManager<HealthyUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO dTO)
        {
            if(!ModelState.IsValid)
                return View(dTO);

            HealthyUser healthyUser = new HealthyUser
            {
                UserName = dTO.Username,
                Email = dTO.Email,
                Sex = dTO.Sex,
                Weight = dTO.Weight,
                Height = dTO.Height,
                DateOfBirth = dTO.DateOfBirth,

            };
            var result = await userManager.CreateAsync(healthyUser, dTO.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(healthyUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(dTO);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO dTO)
        {
            if (!ModelState.IsValid)
                return View(dTO);
            HealthyUser? healthyUser = await userManager.FindByNameAsync(dTO.Username);
            if (healthyUser != null)
            {
                var result = await signInManager.PasswordSignInAsync(healthyUser, dTO.Password,
                    dTO.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError(string.Empty, "User/password is wrong");
            }
            else
                ModelState.AddModelError(string.Empty, "User not found");
            return View(dTO);
        }

        
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
