using AutoMapper;
using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Mvc;
using X.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using X.PagedList;

namespace CaloryCalcApp.WebAPI.Controllers
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

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Products.ToListAsync());
        //}

        // Modified Index action to support pagination
        public ActionResult Index(int page = 1, int pageSize = 10, int? oldPageSize = null)
        {
            if (oldPageSize.HasValue && oldPageSize.Value != pageSize)
            {
                int firstItemIndex = (page - 1) * oldPageSize.Value;
                page = firstItemIndex / pageSize + 1;
            }

            var products = _context.Products.OrderBy(p => p.Id);

            var pagedProducts = products.ToPagedList(page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(pagedProducts);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Products/Create
        public IActionResult Create()
        {
            // Checking if the method is called
            //System.Diagnostics.Debug.WriteLine("Inside `Create` method");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Density,Calories,Fats,Carbohydrates,Proteins")] ProductDTO productDTO)
        {
            foreach (var entry in ModelState) { System.Diagnostics.Debug.WriteLine($"Here we are:" + $"{entry.Key}: {entry.Value.AttemptedValue}"); }
            System.Diagnostics.Debug.WriteLine("POST Create HIT");
            if (ModelState.IsValid)
            {
                Product product = _mapper.Map<Product>(productDTO);
                _context.Add(product);
                //System.Diagnostics.Debug.WriteLine("Creating product: " + product.Name);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Debug validation errors
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }
            }
            return View(productDTO);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Density,Calories,Fats,Carbohydrates,Proteins")] Product product)
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
