using CaloryCalcApp.Application.DTOs.HealthyUserDishes;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class HealthyUserDishesController : Controller
    {
        private readonly IHealthyUserDishService _healthyUserDishService;
        private readonly IDishService _dishService;
        private readonly UserManager<HealthyUser> _userManager;

        public HealthyUserDishesController(
            IHealthyUserDishService healthyUserDishService,
            IDishService dishService,
            UserManager<HealthyUser> userManager)
        {
            _healthyUserDishService = healthyUserDishService;
            _dishService = dishService;
            _userManager = userManager;
        }

        public async Task<ActionResult> IndexAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            ViewBag.DishNameLabel = "Dish Name";

            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null) return NotFound();

            IEnumerable<HealthyUserDishDto> items;
            if (User.IsInRole("admin"))
                items = await _healthyUserDishService.GetAllWithDetailsAsync();
            else
                items = await _healthyUserDishService.GetByUserIdAsync(currentUserId);

            var pagedItems = items.OrderBy(h => h.Id).ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedItems);
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null) return NotFound();
            var item = await _healthyUserDishService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            ViewBag.CurrentUserId = userId;

            var dishes = await _dishService.GetAllAsync();
            ViewData["DishId"] = new SelectList(dishes, "Id", "Name");
            ViewData["HealthyUserId"] = new SelectList(_userManager.Users, "Id", "Id", userId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDishDto dto)
        {
            if (ModelState.IsValid)
            {
                await _healthyUserDishService.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var dishes = await _dishService.GetAllAsync();
            ViewData["DishId"] = new SelectList(dishes, "Id", "Name", dto.DishId);
            ViewData["HealthyUserId"] = new SelectList(_userManager.Users, "Id", "Id", dto.HealthyUserId);
            return View(dto);
        }

        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null) return NotFound();
            var item = await _healthyUserDishService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();

            var dishes = await _dishService.GetAllAsync();
            ViewData["DishId"] = new SelectList(dishes, "Id", "Name", item.DishId);
            ViewData["HealthyUserId"] = new SelectList(_userManager.Users, "Id", "Id", item.HealthyUserId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDishDto dto)
        {
            if (id != dto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _healthyUserDishService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var dishes = await _dishService.GetAllAsync();
            ViewData["DishId"] = new SelectList(dishes, "Id", "Name", dto.DishId);
            ViewData["HealthyUserId"] = new SelectList(_userManager.Users, "Id", "Id", dto.HealthyUserId);
            return View(dto);
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var item = await _healthyUserDishService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            await _healthyUserDishService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> StatisticsAsync(int days = 7)
        {
            var timeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1",   Text = "24 hours" },
                new SelectListItem { Value = "7",   Text = "7 days" },
                new SelectListItem { Value = "30",  Text = "30 days" },
                new SelectListItem { Value = "90",  Text = "90 days" },
                new SelectListItem { Value = "180", Text = "180 days" },
                new SelectListItem { Value = "365", Text = "365 days" },
                new SelectListItem { Value = "0",   Text = "All recorded time" }
            };

            ViewBag.TimeOptions = timeOptions;
            ViewBag.SelectedDays = days.ToString();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var stats = await _healthyUserDishService.GetStatisticsAsync(userId, days);

            ViewBag.MealData = Newtonsoft.Json.JsonConvert.SerializeObject(stats.DataPoints);
            ViewBag.TotalCalories = stats.DataPoints.Sum(dp => dp.TotalCalories);
            ViewBag.TotalFats = stats.DataPoints.Sum(dp => dp.TotalFats);
            ViewBag.TotalProteins = stats.DataPoints.Sum(dp => dp.TotalProteins);
            ViewBag.TotalCarbohydrates = stats.DataPoints.Sum(dp => dp.TotalCarbohydrates);
            ViewBag.AverageCaloriesPerDay = stats.ActualDays > 0
                ? Math.Round(ViewBag.TotalCalories / stats.ActualDays, 2) : 0;
            ViewBag.AverageFatsPerDay = stats.ActualDays > 0
                ? Math.Round(ViewBag.TotalFats / stats.ActualDays, 2) : 0;
            ViewBag.AverageProteinsPerDay = stats.ActualDays > 0
                ? Math.Round(ViewBag.TotalProteins / stats.ActualDays, 2) : 0;
            ViewBag.AverageCarbohydratesPerDay = stats.ActualDays > 0
                ? Math.Round(ViewBag.TotalCarbohydrates / stats.ActualDays, 2) : 0;

            return View(stats.Dishes);
        }
    }
}
