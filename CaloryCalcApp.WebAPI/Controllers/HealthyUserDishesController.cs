using AutoMapper;
using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUserDishes;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc;

using System.ComponentModel.DataAnnotations; // this and next using were added to get `Dish name` label from Dish.cs
using System.Reflection;


namespace CaloryCalcApp.WebAPI.Controllers
{
    [Authorize]
    public class HealthyUserDishesController : Controller
    {
        private readonly CaloriesContext _context;
        private readonly IMapper _mapper;

        public HealthyUserDishesController(CaloriesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: HealthyUserDishes
        //public async Task<IActionResult> Index()
        //{
        //    var caloriesContext = _context.HealthyUserDishes.Include(h => h.Dish).Include(h => h.HealthyUser);
        //    return View(await caloriesContext.ToListAsync());
        //}

        // Modified Index action with pagination support
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }
            

            // Let's get label from Dish.cs class to sign our table:
            var dishNameProperty = typeof(Dish).GetProperty("Name");
            var dishNameDisplayAttribute = dishNameProperty.GetCustomAttribute<DisplayAttribute>();
            ViewBag.DishNameLabel = dishNameDisplayAttribute != null ? dishNameDisplayAttribute.Name : "Dish Name";


            var healthyUserDishes = _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .OrderBy(h => h.Id);
            var healthyUserDishesDTO = _mapper.Map<List<HealthyUserDishDTO>>(healthyUserDishes);
            var pagedHealthyUserDishes = healthyUserDishesDTO.ToPagedList(page, pageSize);
            ViewBag.DishNames = _context.Dishes.ToDictionary(d => d.Id, d => d.Name);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(pagedHealthyUserDishes);
        }

        // GET: HealthyUserDishes/Details/5
        public async Task<IActionResult> Details(int? id)
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
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: HealthyUserDishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDishDTO healthyUserDishDTO)
        {

            if (ModelState.IsValid)
            {
                HealthyUserDish healthyUserDish = _mapper.Map<HealthyUserDish>(healthyUserDishDTO);
                _context.Add(healthyUserDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", healthyUserDishDTO.DishId);
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", healthyUserDishDTO.HealthyUserId);
            return View(healthyUserDishDTO);
        }

        // GET: HealthyUserDishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,DishId,Amount,HealthyUserId,MealTime")] HealthyUserDish healthyUserDish)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", healthyUserDish.DishId);
            ViewData["HealthyUserId"] = new SelectList(_context.Users, "Id", "Id", healthyUserDish.HealthyUserId);
            return View(healthyUserDish);
        }

        // GET: HealthyUserDishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var healthyUserDish = await _context.HealthyUserDishes.FindAsync(id);
            if (healthyUserDish != null)
            {
                _context.HealthyUserDishes.Remove(healthyUserDish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HealthyUserDishExists(int id)
        {
            return _context.HealthyUserDishes.Any(e => e.Id == id);
        }
    }
}
