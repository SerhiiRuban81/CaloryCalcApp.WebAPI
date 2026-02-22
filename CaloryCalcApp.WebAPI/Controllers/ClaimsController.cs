using CaloryCalcApp.WebAPI.Models.ViewModels.Claims;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<HealthyUser> userManager;
        private readonly SignInManager<HealthyUser> signInManager;

        public ClaimsController(UserManager<HealthyUser> userManager,
            SignInManager<HealthyUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claims = User.Claims;
                HealthyUser? healthyUser = await userManager.FindByNameAsync(User.Identity.Name!);

                if (healthyUser == null)
                {
                    return NotFound();
                }
                IndexClaimsVM vM = new IndexClaimsVM
                {
                    Claims = claims,
                    UserName = healthyUser.UserName!,
                    Email = healthyUser.Email!
                };
                return View(vM);
            }
            return RedirectToAction("Login", "Account");
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            if (!ModelState.IsValid)
                return View();
            Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
            HealthyUser? healthyUser = await userManager.GetUserAsync(User); // Passing claims principal to get the current user
            if (healthyUser == null) { return NotFound(); }
            var result = await userManager.AddClaimAsync(healthyUser, claim);
            if (result.Succeeded)
            {
                // Two bottom strings added to see results immediately without logging out and in again by User
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(healthyUser, isPersistent: false);
                return RedirectToAction("Index");
            }
            Errors(ModelState, result);
            return View();
        }

        public void Errors(ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            if (User != null && User.Identity != null)
            {
                string[] claimsData = claimValues.Split(';');
                string claimType = claimsData[0];
                string claimValue = claimsData[1];
                string claimIssuer = claimsData[2];
                Claim? claim = User.Claims.FirstOrDefault(t=>t.Type == claimType && t.Value == claimValue
                && t.Issuer == claimIssuer);
                if (claim != null)
                {
                    HealthyUser? healthyUser = await userManager.GetUserAsync(User);
                    if (healthyUser == null) return NotFound();
                    await userManager.RemoveClaimAsync(healthyUser, claim);
                    // Two bottom strings added to see results immediately without logging out and in again by User
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(healthyUser, isPersistent: false);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "Account");

        }
    }
}
