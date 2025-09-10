using CaloryCalcApp.WebAPI.Data;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CaloriesContext context;

        public ProductController(CaloriesContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product? product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts() => await context.Products.ToListAsync();

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            if (!IsProductExists(id)) return NotFound();

            context.Update(product);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();

            context.Remove(product);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool IsProductExists(int id) => context.Products.Any(product => product.Id == id);
    }
}
