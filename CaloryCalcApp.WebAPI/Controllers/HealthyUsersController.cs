using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.HealthyUsers;
using CaloryCalcApp.Web.Models.ViewModels.Users;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace CaloryCalcApp.Web.Controllers
{
    public class HealthyUsersController : Controller
    {
        private readonly UserManager<HealthyUser> _userManager;
        private readonly IMapper _mapper;

        public HealthyUsersController(UserManager<HealthyUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            IEnumerable<HealthyUser> healthyUsers = await _userManager.Users.ToListAsync();

            IEnumerable<HealthyUserDTO> userDTOs = _mapper.Map<IEnumerable<HealthyUser>, IEnumerable<HealthyUserDTO>>(healthyUsers);
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

        public async Task<IActionResult> EditAsync(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            HealthyUserDTO userDTO = _mapper.Map<HealthyUser, HealthyUserDTO>(user);
            userDTO.Name = user.UserName ?? "(no username)"; // Setting in our DTO the UserName from IdentityUser
            return View(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(HealthyUserDTO userDTO)
        {
            if (!ModelState.IsValid) return View(userDTO);
            HealthyUser? user = await _userManager.FindByIdAsync(userDTO.Id.ToString());
            if (user != null)
            {
                await _userManager.SetUserNameAsync(user, userDTO.Name); // Updating the UserName using UserManager
                user.Weight = userDTO.Weight;
                user.Height = userDTO.Height;
                user.DateOfBirth = userDTO.DateOfBirth;
                IdentityResult result = await _userManager.UpdateAsync(user);
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

        public async Task<IActionResult> ChangePasswordAsync(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");
            ChangePasswordVM vM = new ChangePasswordVM
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(vM);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVM vM)
        {
            if (!ModelState.IsValid) return View(vM);
            HealthyUser? user = await _userManager.FindByIdAsync(vM.Id);
            if (user == null) return NotFound("User not found");
            var result = await _userManager.ChangePasswordAsync(user, vM.OldPassword, vM.NewPassword);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(vM);
        }
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");
            HealthyUserDTO userDTO = _mapper.Map<HealthyUserDTO>(user);
            return View(userDTO);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmedAsync(HealthyUserDTO userDTO)
        {
            if (userDTO == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(userDTO.Id.ToString());
            if (user == null) return NotFound("User not found");
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(userDTO);

        }


        // For the current logged-in user
        public async Task<IActionResult> UserDetailsAsync()
        {            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not logged in");
            }
            // Map to DTO
            HealthyUserDTO userDTO = _mapper.Map<HealthyUser, HealthyUserDTO>(user);
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

		[HttpGet]
		public async Task<IActionResult> WeightUpdateAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				return BadRequest("User Id is required");

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return NotFound();
			var userDTO = _mapper.Map<HealthyUser, HealthyUserDTO>(user);
			return View(userDTO); 
		}

		[HttpPost]
		public async Task<IActionResult> WeightUpdateAsync(HealthyUserDTO model)
		{
			if (model == null || string.IsNullOrWhiteSpace(model.Id))
				return BadRequest("Invalid user Id");

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
				return NotFound();

			user.Weight = model.Weight;
			await _userManager.UpdateAsync(user);

			return RedirectToAction("Index", "Home");
		}
	}
}

