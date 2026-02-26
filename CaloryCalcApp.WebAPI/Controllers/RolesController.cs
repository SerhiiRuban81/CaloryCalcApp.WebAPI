using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using CaloryCalcApp.WebAPI.Models.DTOs.Roles;
using CaloryCalcApp.WebAPI.Models.ViewModels.Roles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Linq;
using X.PagedList.Extensions;

namespace CaloryCalcApp.WebAPI.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<HealthyUser> userManager;
        private readonly IMapper mapper;

        public RolesController(RoleManager<IdentityRole> roleManager,
            UserManager<HealthyUser> userManager,
            IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            IEnumerable<RoleDTO> roleDTOs = mapper.Map<IEnumerable<RoleDTO>>(roles);
            return View(roleDTOs);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View(model: roleManager);
            }
            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }

        // Delete method to show delete confirmation page
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Map role to RoleDTO if needed, or create a ViewModel
            var roleDTO = mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            // handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            var roleDTO = mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Get method to show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var roleDTO = mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Post method to handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "Role ID and name cannot be empty.");
                return View();
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            var roleDTO = mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Method to change user roles
        public async Task<IActionResult> UserList(int page = 1, int pageSize = 10, int? oldPageSize = null)
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

            var pagedUsers =userDTOs.ToPagedList(page, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedUsers);
        }

        public async Task<IActionResult> ChangeRoles(string? id)
        {
            if (id == null)
                return NotFound();
            HealthyUser? healthyUser = await userManager.FindByIdAsync(id);
            if(healthyUser == null)
                return NotFound();
            var allRoles = await roleManager.Roles.ToListAsync();
            var userRoles = await userManager.GetRolesAsync(healthyUser);
            ChangeRolesVM vM = new ChangeRolesVM()
            {
                Id = healthyUser.Id,
                Email = healthyUser.Email,
                AllRoles = allRoles,
                UserRoles = userRoles
            };
            return View(vM);
            
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoles(ChangeRolesVM vM)
        {            
            HealthyUser? healthyUser = await userManager.FindByIdAsync(vM.Id);
            if(healthyUser == null)
                return NotFound();
            var allRoles = await roleManager.Roles.ToListAsync();
            
            var userRoles = await userManager.GetRolesAsync(healthyUser);
            if (ModelState.IsValid)
            {
                var addedRoles = vM.Roles!.Except(userRoles);
                var deletedRoles = userRoles.Except(vM.Roles);
                await userManager.AddToRolesAsync(healthyUser, addedRoles); // Adding new roles if any
                await userManager.RemoveFromRolesAsync(healthyUser, deletedRoles); // Deleting old roles if any

                return RedirectToAction("Index");
            }
            vM.AllRoles = allRoles;
            vM.UserRoles = userRoles;
            vM.Email = healthyUser.Email;
            return View(vM);
        }
    }
}
