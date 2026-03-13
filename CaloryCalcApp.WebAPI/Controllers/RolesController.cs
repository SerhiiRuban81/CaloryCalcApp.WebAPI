using AutoMapper;
using CaloryCalcApp.Web.Models.DTOs.HealthyUsers;
using CaloryCalcApp.Web.Models.DTOs.Roles;
using CaloryCalcApp.Web.Models.ViewModels.Roles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace CaloryCalcApp.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<HealthyUser> _userManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager,
            UserManager<HealthyUser> userManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            IEnumerable<RoleDTO> roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return View(roleDTOs);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
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
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var roleDTO = _mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            var roleDTO = _mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Get method to show edit form
        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var roleDTO = _mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Post method to handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(string id, string name)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "Role ID and name cannot be empty.");
                return View();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            var roleDTO = _mapper.Map<RoleDTO>(role);
            return View(roleDTO);
        }

        // Method to change user roles
        public async Task<IActionResult> UserListAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
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

        public async Task<IActionResult> ChangeRolesAsync(string? id)
        {
            if (id == null)
                return NotFound();
            HealthyUser? healthyUser = await _userManager.FindByIdAsync(id);
            if (healthyUser == null)
                return NotFound();
            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(healthyUser);
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
        public async Task<IActionResult> ChangeRolesAsync(ChangeRolesVM vM)
        {
            HealthyUser? healthyUser = await _userManager.FindByIdAsync(vM.Id);
            if (healthyUser == null)
                return NotFound();
            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(healthyUser);
            if (ModelState.IsValid)
            {
                var addedRoles = vM.Roles!.Except(userRoles);
                var deletedRoles = userRoles.Except(vM.Roles);
                await _userManager.AddToRolesAsync(healthyUser, addedRoles); // Adding new roles if any
                await _userManager.RemoveFromRolesAsync(healthyUser, deletedRoles); // Deleting old roles if any

                return RedirectToAction("Index");
            }
            vM.AllRoles = allRoles;
            vM.UserRoles = userRoles;
            vM.Email = healthyUser.Email;
            return View(vM);
        }
    }
}

