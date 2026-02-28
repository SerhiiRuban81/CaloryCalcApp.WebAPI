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
                    Email = healthyUser.Email!,
                    UserId = healthyUser.Id!,
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
        public async Task<IActionResult> Delete(string claimValues, string returnUrl, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            string[] claimsData = claimValues.Split(';');
            string claimType = claimsData[0];
            string claimValue = claimsData[1];
            string claimIssuer = claimsData[2];

            var claims = await userManager.GetClaimsAsync(user);

            var claim = claims.FirstOrDefault(c =>
                c.Type == claimType &&
                c.Value == claimValue &&
                c.Issuer == claimIssuer);

            if (claim != null)
            {
                await userManager.RemoveClaimAsync(user, claim);

                // Refresh cookie only if deleting current user's claim
                var currentUser = await userManager.GetUserAsync(User);
                if (currentUser != null && currentUser.Id == user.Id)
                {
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, isPersistent: false);
                }

                return Redirect(returnUrl);
            }

            return Redirect(returnUrl);
            //if (User != null && User.Identity != null)
            //{
            //    if (string.IsNullOrWhiteSpace(userId)) 
            //        return RedirectToAction("Login", "Account");

            //    var user = await userManager.FindByIdAsync(userId); 
            //    if (user == null) return NotFound();

            //    string[] claimsData = claimValues.Split(';');
            //    string claimType = claimsData[0];
            //    string claimValue = claimsData[1];
            //    string claimIssuer = claimsData[2];

            //    Claim? claim = User.Claims.FirstOrDefault(
            //        t=>t.Type == claimType 
            //        && t.Value == claimValue
            //        && t.Issuer == claimIssuer);

            //    if (claim != null)
            //    {
            //        HealthyUser? healthyUser = await userManager.GetUserAsync(User);
            //        if (healthyUser == null) return NotFound();
            //        await userManager.RemoveClaimAsync(healthyUser, claim);

            //        // Two bottom strings added to see results immediately without logging out and in again by User
            //        await signInManager.SignOutAsync();
            //        await signInManager.SignInAsync(healthyUser, isPersistent: false);
            //        return RedirectToAction(returnUrl);
            //    }
            //}
            //return RedirectToAction("Login", "Account");

        }


        /// USER's CLAIMS////////////////////////////////
        public async Task<IActionResult> UserClaims(string id)
        {
            if (!ModelState.IsValid)
                return View();
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var choosenUser = await userManager.FindByIdAsync(id!);            

            if (choosenUser != null)
            {
                var claims = await userManager.GetClaimsAsync(choosenUser);

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
        public async Task<IActionResult> CreateUserClaim(string id, string claimType, string claimValue)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await userManager.FindByIdAsync(id); // Getting our user by id
            if (user == null) { return NotFound(); }
            
            var claim = new Claim(claimType, claimValue);

            var result = await userManager.AddClaimAsync(user, claim);
            if (result.Succeeded)
            {
                // Two bottom strings added to see results immediately without logging out and in again by User
                //await signInManager.SignOutAsync();
                //await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("UserClaims", new { id }); 
            }
            Errors(ModelState, result);
            return View();
        }

    }
}
