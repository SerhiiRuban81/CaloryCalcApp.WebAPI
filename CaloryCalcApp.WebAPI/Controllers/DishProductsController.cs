using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProduct;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishProductsController : ControllerBase
    {
        private readonly CaloriesContext context;

        public DishProductsController(CaloriesContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<DishProduct>> GetDishProducts() => await context.DishProducts.ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishProduct(int id)
        {
            DishProduct? dishProduct = await context.DishProducts
                .Include(dp => dp.Product)
                .Include(dp => dp.Dish)
                .FirstOrDefaultAsync(dp => dp.Id == id);
            if (dishProduct == null) return NotFound();

            return Ok(dishProduct);
        }

        [HttpPost]
        public async Task<IActionResult> PostDishProduct(DishProductDTO dishProductDTO)
        {
            DishProduct dishProduct = new DishProduct
            {
                ProductId = dishProductDTO.ProductId,
                DishId = dishProductDTO.DishId,
                MeasurementUnit = dishProductDTO.MeasurementUnit,
                Amount = dishProductDTO.Amount
            };

            context.DishProducts.Add(dishProduct);
            await context.SaveChangesAsync();
            return Ok(dishProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDishProduct(int id, DishProductDTO dishProductDTO)
        {
            DishProduct dishProduct = new DishProduct
            {
                Id = dishProductDTO.Id,
                ProductId = dishProductDTO.ProductId,
                DishId = dishProductDTO.DishId,
                MeasurementUnit = dishProductDTO.MeasurementUnit,
                Amount = dishProductDTO.Amount
            };

            if (id != dishProduct.Id) return BadRequest();
            if (!IsDishProductExists(id)) return NotFound();

            context.Update(dishProduct);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDishProduct(int id)
        {
            DishProduct? dishProduct = await context.DishProducts.FindAsync(id);
            if (dishProduct == null) return NotFound();

            context.Remove(dishProduct);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool IsDishProductExists(int id) => context.DishProducts.Any(dp => dp.Id == id);
    }
}
