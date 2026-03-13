using AutoMapper;
using CaloryCalcApp.Web.Data;
using CaloryCalcApp.Web.Models.DTOs.Dishes;
using CaloryCalcApp.Web.Models.DTOs.DishProducts;
using CaloryCalcApp.Web.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using X.PagedList;
using X.PagedList.Extensions;

namespace CaloryCalcApp.Web.Controllers
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

        public ActionResult Index(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            var dishes = _context.Dishes
                .Include(d => d.DishProducts)
                .ThenInclude(dp => dp.Product)
                .OrderBy(p => p.Id).ToList();
            var dishesDTO = _mapper.Map<List<DishDTO>>(dishes);
            var pagedDishes = dishesDTO.ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedDishes);
        }

        public async Task<IActionResult> DetailsAsync(int? id)
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
            var dishDTO = _mapper.Map<DishDTO>(dish);

            var products = _context.Products.ToList();
            var dishProductIds = dish.DishProducts.Select(dp => dp.ProductId).ToHashSet();

            var productNames = new List<string>();
            foreach (var pr in products)
            {
                if (dishProductIds.Contains(pr.Id))
                {
                    productNames.Add(pr.Name);
                }
            }
            ViewBag.ProductNames = productNames;

            return View(dishDTO);
        }

        public IActionResult Create()
        {
            var products = _context.Products.ToList();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(
            [Bind("Id,Name")] DishDTO dishDTO,
            List<int> Products,
            List<int> ProductQuantities,
            List<string> MeasurementUnits)
        {
            if (Products == null || ProductQuantities == null || MeasurementUnits == null ||
                 Products.Count != ProductQuantities.Count || Products.Count != MeasurementUnits.Count)
            {
                ModelState.AddModelError("", "Mismatch in products and quantities data.");
                return View(dishDTO);
            }
            if (ModelState.IsValid)
            {
                var dish = new Dish { Name = dishDTO.Name };
                for (int i = 0; i < Products.Count; i++)
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

        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(p => p.DishProducts)
                .ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditAsync(int id, Dish updatedDish)
		{
			if (id != updatedDish.Id)
				return NotFound();

			var existingDish = await _context.Dishes
				.Include(d => d.DishProducts)
				.FirstOrDefaultAsync(d => d.Id == id);

            if (existingDish == null)
            {
                return NotFound();
            }

			existingDish.Name = updatedDish.Name;

			updatedDish.DishProducts ??= new List<DishProduct>();

			_context.DishProducts.RemoveRange(existingDish.DishProducts);

			foreach (var dp in updatedDish.DishProducts)
			{
                existingDish.DishProducts.Add(new DishProduct
				{
					ProductId = dp.ProductId,
					Amount = dp.Amount,
					MeasurementUnit = dp.MeasurementUnit
				});
		    }

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> DeleteAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var dish = await _context.Dishes
			   .Include(p => p.DishProducts)
			   .ThenInclude(dp => dp.Product)
			   .FirstOrDefaultAsync(d => d.Id == id);
			if (dish == null)
			{
				return NotFound();
			}

			return View(dish);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmedAsync(int id)
		{
			var dish = await _context.Dishes
				.Include(d => d.DishProducts)
				.FirstOrDefaultAsync(d => d.Id == id);
			if (dish != null)
            {
				_context.DishProducts.RemoveRange(dish.DishProducts);
				_context.Dishes.Remove(dish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }


		 [HttpGet]
		 public async Task<IActionResult> SearchProductInDishAsync(int id, string wordProduct)
		{
			if (string.IsNullOrWhiteSpace(wordProduct))
				return Json(new { success = false });

			var matchedProducts = await _context.Products
				.Where(p => p.Name.Contains(wordProduct))
				.Select(p => new
				{
					productId = p.Id,
					name = p.Name
				})
				.ToListAsync();

			return Json(new
			{
				success = matchedProducts.Any(),
				products = matchedProducts
			});
		}


	}
}

