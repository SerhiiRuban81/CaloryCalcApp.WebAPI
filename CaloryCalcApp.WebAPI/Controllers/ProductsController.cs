using AutoMapper;
using CaloryCalcApp.Web.Data;
using CaloryCalcApp.Web.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;


namespace CaloryCalcApp.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CaloriesContext _context;

        public ProductsController(CaloriesContext context, IMapper mapper)
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

            var products = _context.Products.OrderBy(p => p.Id);
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            var pagedProducts = productDtos.ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedProducts);
        }


        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

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
                var product = _mapper.Map<Product>(productDto);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

		[HttpGet]
		public async Task<IActionResult> SearchProductsAsync(string term)
		{
			var products = await _context.Products
				.Where(p => p.Name.Contains(term))
				.Select(p => new { p.Id, p.Name })
				.Take(10)
				.ToListAsync();

			return Json(products);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditAsync(int id, [Bind("Id,Name,Density,Calories,Fats,Carbohydrates,Proteins")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

