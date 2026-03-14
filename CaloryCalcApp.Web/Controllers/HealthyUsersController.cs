using AutoMapper;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
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

            var healthyUsers = await _userManager.Users.ToListAsync();

            var userDtos = _mapper.Map<IEnumerable<HealthyUserDto>>(healthyUsers);
            foreach (var userDto in userDtos)
            {
                var user = healthyUsers.FirstOrDefault(u => u.Id.ToString() == userDto.Id);
                if (user != null)
                {
                    userDto.Name = user.UserName ?? "(no username)";
                }
            }

            var pagedUsers = userDtos.ToPagedList(page, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedUsers);
        }

        public async Task<IActionResult> EditAsync(string? id)
        {
            if (id == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userDto = _mapper.Map<HealthyUserDto>(user);
            userDto.Name = user.UserName ?? "(no username)";
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(HealthyUserDto userDto)
        {
            if (!ModelState.IsValid) return View(userDto);
            HealthyUser? user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user != null)
            {
                await _userManager.SetUserNameAsync(user, userDto.Name);
                user.Weight = userDto.Weight;
                user.Height = userDto.Height;
                user.DateOfBirth = userDto.DateOfBirth;
                var result = await _userManager.UpdateAsync(user);
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
                    return View(userDto);
                }
            }
            return View(userDto);

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
            var userDto = _mapper.Map<HealthyUserDto>(user);
            return View(userDto);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmedAsync(HealthyUserDto userDto)
        {
            if (userDto == null) return NotFound();
            HealthyUser? user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null) return NotFound("User not found");
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(userDto);

        }


        public async Task<IActionResult> UserDetailsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not logged in");
            }
            var userDto = _mapper.Map<HealthyUserDto>(user);
            userDto.Name = user.UserName ?? "(no username)";

            double bmr = 0;
            if (user.Sex == Sex.Male)
            {
                bmr = 10 * user.Weight + 6.25 * user.Height - 5 * (DateTime.Today.Year - user.DateOfBirth.Year) + 5;
            }
            else if (user.Sex == Sex.Female)
            {
                bmr = 10 * user.Weight + 6.25 * user.Height - 5 * (DateTime.Today.Year - user.DateOfBirth.Year) - 161;
            }

            var tdeeDictionary = new Dictionary<string, double>
            {
                { "Sedentary (little/no exercise)", bmr * 1.2 },
                { "Lightly Active (light exercise/sports 1-3 days/week)", bmr * 1.375 },
                { "Moderately Active (moderate exercise 3-5 days/week)", bmr * 1.55 },
                { "Very Active (hard exercise 6-7 days/week)", bmr * 1.725 },
                { "Super Active (very hard exercise/physical job)", bmr * 1.9 }
            };

            ViewBag.BMR = bmr;
            ViewBag.TDEE = tdeeDictionary;

            return View(userDto);
        }

		[HttpGet]
		public async Task<IActionResult> WeightUpdateAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				return BadRequest("User Id is required");

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return NotFound();
			var userDto = _mapper.Map<HealthyUserDto>(user);
			return View(userDto);
		}

		[HttpPost]
		public async Task<IActionResult> WeightUpdateAsync(HealthyUserDto model)
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

