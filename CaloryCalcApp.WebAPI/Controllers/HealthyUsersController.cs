using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using CaloryCalcApp.WebAPI.Models.ViewModels.Users;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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


        public IActionResult Index()
        {
            IEnumerable<HealthyUser> healthyUsers = userManager.Users.ToList();
            IEnumerable<HealthyUserDTO> userDTOs = mapper.Map<IEnumerable<HealthyUser>, IEnumerable<HealthyUserDTO>>(healthyUsers);
            foreach(var userDTO in userDTOs)
            {
                HealthyUser? user = healthyUsers.FirstOrDefault(u => u.Id.ToString() == userDTO.Id);
                if (user != null)
                {
                    userDTO.Name = user.UserName ?? "(no username)"; // Setting in our DTO the UserName from IdentityUser
                }
            }
            // Pagination to be done here
            return View(userDTOs);
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


    }
}
