using CaloryCalcApp.Application.DTOs.Products;
using CaloryCalcApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;


namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ActionResult> IndexAsync(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            var products = await _productService.GetAllAsync();
            var pagedProducts = products.OrderBy(p => p.Id).ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedProducts);
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Create()
        {
            return View(new ProductDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,Name,Density,Calories,Fats,Carbohydrates,Proteins")] ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> SearchProductsAsync(string term)
        {
            var products = await _productService.SearchByNameAsync(term);
            return Json(products.Take(10).Select(p => new { p.Id, p.Name }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id,Name,Density,Calories,Fats,Carbohydrates,Proteins")] ProductDto productDto)
        {
            if (id != productDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

