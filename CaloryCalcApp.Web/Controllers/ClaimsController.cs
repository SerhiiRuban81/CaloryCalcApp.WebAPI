using CaloryCalcApp.Web.Models.ViewModels.Claims;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace CaloryCalcApp.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<HealthyUser> _userManager;
        private readonly SignInManager<HealthyUser> _signInManager;

        public ClaimsController(UserManager<HealthyUser> userManager,
            SignInManager<HealthyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claims = User.Claims;
                HealthyUser? healthyUser = await _userManager.FindByNameAsync(User.Identity.Name!);

                if (healthyUser == null)
                {
                    return NotFound();
                }
                IndexClaimsVM vM = new IndexClaimsVM
                {
                    Claims = claims,
                    UserName = healthyUser.UserName!,
                    Email = healthyUser.Email!,
                    UserId = healthyUser.Id!,
                };
                return View(vM);
            }
            return RedirectToAction("Login", "Account");
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string claimType, string claimValue)
        {
            if (!ModelState.IsValid)
                return View();
            Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
            HealthyUser? healthyUser = await _userManager.GetUserAsync(User); // Passing claims principal to get the current user
            if (healthyUser == null) { return NotFound(); }
            var result = await _userManager.AddClaimAsync(healthyUser, claim);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(healthyUser, isPersistent: false);
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
        public async Task<IActionResult> DeleteAsync(string claimValues, string returnUrl, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            string[] claimsData = claimValues.Split(';');
            string claimType = claimsData[0];
            string claimValue = claimsData[1];
            string claimIssuer = claimsData[2];

            var claims = await _userManager.GetClaimsAsync(user);

            var claim = claims.FirstOrDefault(c =>
                c.Type == claimType &&
                c.Value == claimValue &&
                c.Issuer == claimIssuer);

            if (claim != null)
            {
                await _userManager.RemoveClaimAsync(user, claim);

                // Refresh cookie only if deleting current user's claim
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null && currentUser.Id == user.Id)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }

                return Redirect(returnUrl);
            }

            return Redirect(returnUrl);
        }


        /// USER's CLAIMS////////////////////////////////
        public async Task<IActionResult> UserClaimsAsync(string id)
        {
            if (!ModelState.IsValid)
                return View();
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var choosenUser = await _userManager.FindByIdAsync(id!);

            if (choosenUser != null)
            {
                var claims = await _userManager.GetClaimsAsync(choosenUser);

                IndexClaimsVM vM = new IndexClaimsVM
                {
                    Claims = claims,
                    UserName = choosenUser.UserName!,
                    Email = choosenUser.Email!,
                    UserId = choosenUser.Id!
                };
                return View(vM);
            }
            return RedirectToAction("Index", "HealthyUsers");
        }
        // Method for Claims of other Users, used in `Users` controller
        [HttpGet]
        public IActionResult CreateUserClaim(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var vm = new IndexClaimsVM
            {
                UserId = id
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserClaimAsync(string id, string claimType, string claimValue)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await _userManager.FindByIdAsync(id); // Getting our user by id
            if (user == null) { return NotFound(); }

            var claim = new Claim(claimType, claimValue);

            var result = await _userManager.AddClaimAsync(user, claim);
            if (result.Succeeded)
            {
                return RedirectToAction("UserClaims", new { id });
            }
            Errors(ModelState, result);
            return View();
        }

    }
}

