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
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return View(roleDtos);
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
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
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

            var roleDto = _mapper.Map<RoleDto>(role);
            return View(roleDto);
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
            var roleDto = _mapper.Map<RoleDto>(role);
            return View(roleDto);
        }

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
            var roleDto = _mapper.Map<RoleDto>(role);
            return View(roleDto);
        }

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
            var roleDto = _mapper.Map<RoleDto>(role);
            return View(roleDto);
        }

        public async Task<IActionResult> UserListAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
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
                await _userManager.AddToRolesAsync(healthyUser, addedRoles);
                await _userManager.RemoveFromRolesAsync(healthyUser, deletedRoles);

                return RedirectToAction("Index");
            }
            vM.AllRoles = allRoles;
            vM.UserRoles = userRoles;
            vM.Email = healthyUser.Email;
            return View(vM);
        }
    }
}

