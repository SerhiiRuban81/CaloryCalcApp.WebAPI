using AutoMapper;
using CaloryCalcApp.WebAPI.Models.DTOs.Roles;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                return RedirectToAction("Index");
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
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
    }
}
