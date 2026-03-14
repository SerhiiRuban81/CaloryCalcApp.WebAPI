using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class DishesController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IProductService _productService;

        public DishesController(IDishService dishService, IProductService productService)
        {
            _dishService = dishService;
            _productService = productService;
        }

        public async Task<ActionResult> IndexAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            var dishes = await _dishService.GetAllAsync();
            var pagedDishes = dishes.OrderBy(d => d.Id).ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedDishes);
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _dishService.GetByIdAsync(id.Value);
            if (dish == null) return NotFound();

            ViewBag.ProductNames = dish.DishProducts.Select(dp => dp.ProductName).ToList();

            return View(dish);
        }

        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Products = await _productService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(
            [Bind("Id,Name")] DishDto dishDto,
            List<int> Products,
            List<int> ProductQuantities,
            List<string> MeasurementUnits)
        {
            if (Products == null || ProductQuantities == null || MeasurementUnits == null ||
                Products.Count != ProductQuantities.Count || Products.Count != MeasurementUnits.Count)
            {
                ModelState.AddModelError("", "Mismatch in products and quantities data.");
                ViewBag.Products = await _productService.GetAllAsync();
                return View(dishDto);
            }

            if (ModelState.IsValid)
            {
                for (int i = 0; i < Products.Count; i++)
                {
                    dishDto.DishProducts.Add(new Application.DTOs.DishProducts.DishProductDto
                    {
                        ProductId = Products[i],
                        Amount = ProductQuantities[i],
                        MeasurementUnit = MeasurementUnits[i]
                    });
                }
                await _dishService.CreateAsync(dishDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = await _productService.GetAllAsync();
            return View(dishDto);
        }

        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _dishService.GetByIdAsync(id.Value);
            if (dish == null) return NotFound();
            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, DishDto dishDto)
        {
            if (id != dishDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _dishService.UpdateAsync(id, dishDto);
                return RedirectToAction(nameof(Index));
            }
            return View(dishDto);
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _dishService.GetByIdAsync(id.Value);
            if (dish == null) return NotFound();
            return View(dish);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            await _dishService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchProductInDishAsync(int id, string wordProduct)
        {
            if (string.IsNullOrWhiteSpace(wordProduct))
                return Json(new { success = false });

            var matched = await _productService.SearchByNameAsync(wordProduct);
            return Json(new
            {
                success = matched.Any(),
                products = matched.Select(p => new { productId = p.Id, name = p.Name })
            });
		}
	}
}

