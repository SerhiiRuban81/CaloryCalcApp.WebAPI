using AutoMapper;
using CaloryCalcApp.Web.Data;
using CaloryCalcApp.Web.Models.DTOs.HealthyUserDishes;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;


namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class HealthyUserDishesController : Controller
    {
        private readonly CaloriesContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<HealthyUser> _userManager;

        public HealthyUserDishesController(CaloriesContext context, IMapper mapper, UserManager<HealthyUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Modified Index action with pagination support
        public async Task<ActionResult> IndexAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }
            

            // Let's get label from Dish.cs class to sign our table:
            var dishNameProperty = typeof(Dish).GetProperty("Name");
            var dishNameDisplayAttribute = dishNameProperty?.GetCustomAttribute<DisplayAttribute>();
            ViewBag.DishNameLabel = dishNameDisplayAttribute != null ? dishNameDisplayAttribute.Name : "Dish Name";

            string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            if (currentUserId == null) return NotFound();
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if(currentUser == null) return NotFound();
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            IQueryable<HealthyUserDish> healthyUserDishes;
            if (currentRoles.Contains("admin"))
            {
                healthyUserDishes = _context.HealthyUserDishes
                    .Include(h => h.Dish)
                    .Include(h => h.HealthyUser)
                    .OrderBy(h => h.Id);
                
            }
            else
            {
                healthyUserDishes = _context.HealthyUserDishes
                    .Include(h => h.Dish)
                    .Include(h => h.HealthyUser)
                    .Where(h => h.HealthyUser.Id == currentUserId)
                    .OrderBy(h => h.Id);
            }
            var healthyUserDishesDTO = _mapper.Map<List<HealthyUserDishDTO>>(healthyUserDishes);
            var pagedHealthyUserDishes = healthyUserDishesDTO.ToPagedList(page, pageSize);
            ViewBag.DishNames = _context.Dishes.ToDictionary(d => d.Id, d => d.Name);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(pagedHealthyUserDishes);


        }

        // GET: HealthyUserDishes/Details/5
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthyUserDish = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (healthyUserDish == null)
            {
                return NotFound();
            }

            return View(healthyUserDish);
        }

        // GET: HealthyUserDishes/Create
        public IActionResult Create()
        {
            // Let's get Id of our current User
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            ViewBag.CurrentUserId = userId; // Passing Id of current User to our Razor page to set it as default value for HealthyUserId field in Create form

            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", userId);
            
            return View();
        }

        // POST: HealthyUserDishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDishDTO healthyUserDishDTO)
        {

            if (ModelState.IsValid)
            {
                HealthyUserDish healthyUserDish = _mapper.Map<HealthyUserDish>(healthyUserDishDTO);
                _context.Add(healthyUserDish);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", healthyUserDishDTO.DishId);
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", healthyUserDishDTO.HealthyUserId);
            return View(healthyUserDishDTO);
        }

        // GET: HealthyUserDishes/Edit/5
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthyUserDish = await _context.HealthyUserDishes.FindAsync(id);
            if (healthyUserDish == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", healthyUserDish.DishId);
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", healthyUserDish.HealthyUserId);
            return View(healthyUserDish);
        }

        // POST: HealthyUserDishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDish healthyUserDish)
        {
            if (id != healthyUserDish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(healthyUserDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthyUserDishExists(healthyUserDish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", healthyUserDish.DishId);
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", healthyUserDish.HealthyUserId);
            return View(healthyUserDish);
        }

        // GET: HealthyUserDishes/Delete/5
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthyUserDish = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (healthyUserDish == null)
            {
                return NotFound();
            }

            return View(healthyUserDish);
        }

        // POST: HealthyUserDishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var healthyUserDish = await _context.HealthyUserDishes.FindAsync(id);
            if (healthyUserDish != null)
            {
                _context.HealthyUserDishes.Remove(healthyUserDish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> StatisticsAsync(int days = 7)
        {
            // Let's define options for our time dropdown list on Razor page:
            var timeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "24 hours" },
                new SelectListItem { Value = "7", Text = "7 days" },
                new SelectListItem { Value = "30", Text = "30 days" },
                new SelectListItem { Value = "90", Text = "90 days" },
                new SelectListItem { Value = "180", Text = "180 days" },
                new SelectListItem { Value = "365", Text = "365 days" },
                new SelectListItem { Value = "0", Text = "All recorded time" }
            };

            // Passing our time options to Razor page:
            ViewBag.TimeOptions = timeOptions;

            ViewBag.SelectedDays = days.ToString(); // Passing selected time option to Razor page to set it as default value in dropdown list

            // Let's get Id of current user:
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            // Getting our dishes, belonging to the current User
            var userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            Console.WriteLine("UserId = " + userId1);
            List<HealthyUserDish> healthyUserDishes;
            // Let's get dishes, consumed by User during selected time period. If "All recorded time" option is selected, we will get all dishes, consumed by User based of choosen time
            if (days != 0)
            {
                //Console.WriteLine("Selected time period: Last " + days + " days");
                healthyUserDishes = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .ThenInclude(d => d.DishProducts).
                ThenInclude(dp => dp.Product)
                .Where(h => h.HealthyUserId == userId && h.MealTime >= DateTime.Now.AddDays(-days))
                .ToListAsync();
            }
            else
            {
                //Console.WriteLine("Selected time period: All recorded time");
                healthyUserDishes = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .ThenInclude(d => d.DishProducts).
                ThenInclude(dp => dp.Product)
                .Where(h => h.HealthyUserId == userId)
                .ToListAsync();
                days = (int)(DateTime.Now - healthyUserDishes.Min(h => h.MealTime)).TotalDays; // Calculate total days based on the earliest recorded meal time for the user
            }

            // Mapping our dishes to DTO before transferring to razor page
            var healthyUserDishesDTO = _mapper.Map<List<HealthyUserDishDTO>>(healthyUserDishes);

            // Let's prepare data for chart: Date labels and total calories per date
            var dataPoints = healthyUserDishes
            .OrderBy(h => h.MealTime)
            .Select(h => new
            {
                DateTime = h.MealTime.ToString("yyyy-MM-dd HH:mm"), // formatted DateTime
                DishName = h.Dish.Name,
                TotalCalories = ((h.Dish.GetGlobalCalories() ?? 0) / 100) * h.Amount,
                TotalFats = ((h.Dish.GetGlobalFats() ?? 0) / 100) * h.Amount,
                TotalProteins = ((h.Dish.GetGlobalProteins() ?? 0) / 100) * h.Amount,
                TotalCarbohydrates = ((h.Dish.GetGlobalCarbohydrates() ?? 0) / 100) * h.Amount
            }).ToList();

            // Let's pass data to ViewBag for JavaScript consumptionon our Razor page
            ViewBag.MealData = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints);

            // Let's get total calories, fats, proteins and carbohydrates consumed by User during selected time period for displaying in summary section on Razor page
            ViewBag.TotalCalories = dataPoints.Sum(dp => dp.TotalCalories);
            ViewBag.TotalFats = dataPoints.Sum(dp => dp.TotalFats);
            ViewBag.TotalProteins = dataPoints.Sum(dp => dp.TotalProteins);
            ViewBag.TotalCarbohydrates = dataPoints.Sum(dp => dp.TotalCarbohydrates);
            // Calculate average daily calories for the selected time period
            ViewBag.AverageCaloriesPerDay = Math.Round(ViewBag.TotalCalories / days, 2);
            ViewBag.AverageFatsPerDay = Math.Round(ViewBag.TotalFats / days, 2);
            ViewBag.AverageProteinsPerDay = Math.Round(ViewBag.TotalProteins / days, 2);
            ViewBag.AverageCarbohydratesPerDay = Math.Round(ViewBag.TotalCarbohydrates / days, 2);

            return View(healthyUserDishesDTO);




            //Code before changes were done
            /*
            // Let's define options for our time dropdown list on Razor page:
            var timeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "24 hours" },
                new SelectListItem { Value = "7", Text = "7 days" },
                new SelectListItem { Value = "30", Text = "30 days" },
                new SelectListItem { Value = "90", Text = "90 days" },
                new SelectListItem { Value = "180", Text = "180 days" },
                new SelectListItem { Value = "365", Text = "365 days" },
                new SelectListItem { Value = "0", Text = "All recorded time" }
            };

            // Passing our time options to Razor page:
            ViewBag.TimeOptions = timeOptions;

            ViewBag.SelectedDays = days.ToString(); // Passing selected time option to Razor page to set it as default value in dropdown list

            // Let's get Id of current user:
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            // Getting our dishes, belonging to the current User
            var userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine("UserId = " + userId1);
            List<HealthyUserDish> healthyUserDishes;
            // Let's get dishes, consumed by User during selected time period. If "All recorded time" option is selected, we will get all dishes, consumed by User based of choosen time
            if (days != 0)
            {
                //Console.WriteLine("Selected time period: Last " + days + " days");
                healthyUserDishes = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .ThenInclude(d => d.DishProducts).
                ThenInclude(dp => dp.Product)
                .Where(h => h.HealthyUserId == userId && h.MealTime >= DateTime.Now.AddDays(-days))
                .ToListAsync();
            }
            else
            {
                //Console.WriteLine("Selected time period: All recorded time");
                healthyUserDishes = await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Where(h => h.HealthyUserId == userId)
                .ToListAsync();
            }

            // Mapping our dishes to DTO before transferring to razor page
            var healthyUserDishesDTO = _mapper.Map<List<HealthyUserDishDTO>>(healthyUserDishes);
            // Getting Id's of User's dishes
            var usedDishIds = healthyUserDishesDTO.Select(x => x.DishId).Distinct().ToList();
            // Getting dishes names, belonging to our Users
            var dishNames = await _context.Dishes.Where(d => usedDishIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, d => d.Name);
            // Getting dish calories for each User's dish and calculating total calories for each dish based on consumed amount of the dish by User
            var dishCalories = new Dictionary<DateTime, double>(); // Our dictionary to store data in format <MealTime, TotalCaloriesConsumed>
            foreach (var d in healthyUserDishes)
            {
                double? globalCalories = d.Dish.GetGlobalCalories(); // calories per 100g
                double totalCalories = (globalCalories ?? 0) / 100 * d.Amount;
                dishCalories[d.MealTime] = totalCalories;
            }

            ViewBag.DishCalories = dishCalories;

            // Returning ViewBag with dishes names fro further using on our Razor page
            ViewBag.DishNames = dishNames;
            return View(healthyUserDishesDTO);*/
        }

        // Function to check if a HealthyUserDish with a given Id exists in the database
        private bool HealthyUserDishExists(int id)
        {
            return _context.HealthyUserDishes.Any(e => e.Id == id);
        }
    }
}

