using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using CaloryCalcApp.WebAPI.Models.ViewModels.Users;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Globalization;
using X.PagedList.Extensions;

namespace CaloryCalcApp.WebAPI.Controllers
{
    public class HealthyUsersController : Controller
    {
        private readonly UserManager<HealthyUser> userManager;
        private readonly IMapper mapper;

        public HealthyUsersController(UserManager<HealthyUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }
            
            IEnumerable<HealthyUser> healthyUsers = await userManager.Users.ToListAsync();

            IEnumerable<HealthyUserDTO> userDTOs = mapper.Map<IEnumerable<HealthyUser>, IEnumerable<HealthyUserDTO>>(healthyUsers);
            foreach (var userDTO in userDTOs)
            {
                HealthyUser? user = healthyUsers.FirstOrDefault(u => u.Id.ToString() == userDTO.Id);
                if (user != null)
                {
                    userDTO.Name = user.UserName ?? "(no username)"; // Setting in our DTO the UserName from IdentityUser
                }
            }

            var pagedUsers = userDTOs.ToPagedList(page, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedUsers);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            HealthyUserDTO userDTO = mapper.Map<HealthyUser, HealthyUserDTO>(user);
            userDTO.Name = user.UserName ?? "(no username)"; // Setting in our DTO the UserName from IdentityUser
            return View(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HealthyUserDTO userDTO)
        {
            if (!ModelState.IsValid) return View(userDTO);
            HealthyUser? user = await userManager.FindByIdAsync(userDTO.Id.ToString());
            if (user != null)
            {
                await userManager.SetUserNameAsync(user, userDTO.Name); // Updating the UserName using UserManager
                user.Weight = userDTO.Weight;
                user.Height = userDTO.Height;
                user.DateOfBirth = userDTO.DateOfBirth;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(userDTO);
                }
            }
            return View(userDTO);

        }

        public async Task<IActionResult> ChangePassword(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");
            Models.ViewModels.Users.ChangePasswordVM vM = new ChangePasswordVM
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(vM);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vM)
        {
            if (!ModelState.IsValid) return View(vM);
            HealthyUser? user = await userManager.FindByIdAsync(vM.Id);
            if (user == null) return NotFound("User not found");
            var result = await userManager.ChangePasswordAsync(user, vM.OldPassword, vM.NewPassword);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(vM);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? email = await userManager.FindByIdAsync(id);
            if (email == null) return NotFound("User not found");
            HealthyUserDTO userDTO = mapper.Map<HealthyUserDTO>(email);
            return View(userDTO);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(HealthyUserDTO userDTO)
        {
            if (userDTO == null) return NotFound();
            HealthyUser? user = await userManager.FindByIdAsync(userDTO.Id.ToString());
            if (user == null) return NotFound("User not found");
            IdentityResult result = await userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(userDTO);

        }


        // For the current logged-in user
        public async Task<IActionResult> UserDetails()
        {            
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not logged in");
            }
            // Map to DTO
            HealthyUserDTO userDTO = mapper.Map<HealthyUser, HealthyUserDTO>(user);
            userDTO.Name = user.UserName ?? "(no username)";

            // Let's calculate Basal Metabolic Rate (BMR) by an activity factor
            // A common formula is Mifflin-St Jeor
            double BMR = 0;
            if(user.Sex == Sex.Male)
            {
                BMR = 10 * user.Weight + 6.25 * user.Height - 5 * (DateTime.Today.Year - user.DateOfBirth.Year) + 5;
            }
            else if(user.Sex == Sex.Female)
            {
                BMR = 10 * user.Weight + 6.25 * user.Height - 5 * (DateTime.Today.Year - user.DateOfBirth.Year) - 161;
            }

            // Let's calucalte our tdee based on Activity Level:
            var tdeeDictionary = new Dictionary<string, double>
            {
                { "Sedentary (little/no exercise)", BMR * 1.2 },
                { "Lightly Active (light exercise/sports 1-3 days/week)", BMR * 1.375 },
                { "Moderately Active (moderate exercise 3-5 days/week)", BMR * 1.55 },
                { "Very Active (hard exercise 6-7 days/week)", BMR * 1.725 },
                { "Super Active (very hard exercise/physical job)", BMR * 1.9 }
            };

            // Let's transfer calculated data to our RazorPage
            ViewBag.BMR = BMR;
            ViewBag.TDEE = tdeeDictionary;


            return View(userDTO);
        }

        
        // Method to update Weight of User
        public async Task<IActionResult> WeightUpdate(string Id, string NewWeight)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("Invalid user Id");
            if (string.IsNullOrWhiteSpace(NewWeight))
                return BadRequest("Weight is required");

            // Parse weight
            if (!double.TryParse(NewWeight.Replace(',', '.'), CultureInfo.InvariantCulture, out double weight))
                return BadRequest("Invalid weight format");

            // Find user
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();

            // Update weight
            user.Weight = weight;
            await userManager.UpdateAsync(user);

            // Reload the user data from database
            var updatedUser = await userManager.FindByIdAsync(Id);
            if (updatedUser == null)
                return NotFound();

            // Map to DTO
            var userDTO = mapper.Map<HealthyUser, HealthyUserDTO>(updatedUser);
            userDTO.Name = updatedUser.UserName ?? "(no username)";
            // Populate other properties if needed

            // Redirect to the UserDetails GET action to reload the page fully
            return RedirectToAction("UserDetails");
        }
    }
}
