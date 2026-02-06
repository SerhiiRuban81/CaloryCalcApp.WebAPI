using AutoMapper;
using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.Dishes;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProducts;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Authorize]
    public class DishesController : Controller
    {
        private readonly CaloriesContext _context;
        private readonly IMapper _mapper;

        public DishesController(CaloriesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishProducts)
                .ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(m => m.Id == id);            
            if (dish == null)
            {
                return NotFound();
            }
            DishDTO dishDTO = _mapper.Map<DishDTO>(dish);

            dishDTO.TotalCalories = dish.GetGlobalCalories();
            dishDTO.TotalFats = dish.GetGlobalFats();
            dishDTO.TotalProteins = dish.GetGlobalProteins();
            dishDTO.TotalCarbohydrates = dish.GetGlobalCarbohydrates();

            return View(dishDTO);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            // Let's pass our products to the view using ViewBag
            var products = _context.Products.ToList();
            ViewBag.Products = products;
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name")] DishDTO dishDTO,
            List<int> Products,            // list of selected product IDs
            List<int> ProductQuantities,       // list of quantities
            List<string> MeasurementUnits) // list of measurement units
        {
            // Check if the lists are not null and have the same count
            if (Products == null || ProductQuantities == null || MeasurementUnits == null ||
                 Products.Count != ProductQuantities.Count || Products.Count != MeasurementUnits.Count)
            {
                ModelState.AddModelError("", "Mismatch in products and quantities data.");
                return View(dishDTO);
            }
            // Validate the model state
            if (ModelState.IsValid)
            {
                //Debug.WriteLine($"Creating Dish: ID={dishDTO.Id}, Name={dishDTO.Name}, measurementUnit: {MeasurementUnits}");
                //for(int i = 0; i< Products.Count; i++) { 
                //    Debug.WriteLine($"Product ID: {Products[i]}, productName: {(_context.Products.FirstOrDefault(p=>p.Id == Products[i]))?.Name}, Quantity: {ProductQuantities[i]}");
                //}

                // Check if Products and Quantities are provided
                Dish dish = new Dish
                {
                    Name = dishDTO.Name,
                    //DishProducts = new List<DishProduct>() // Initialized below, but created in the Library already
                };
                // Create DishProducts based on selected products and their quantities
                for(int i = 0; i<Products.Count; i++)
                {
                    dish.DishProducts.Add(new DishProduct
                    {
                        ProductId = Products[i],
                        Amount = ProductQuantities[i],
                        MeasurementUnit = MeasurementUnits[i]
                    });
                }

                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishDTO);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
