using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.Dish;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly CaloriesContext context;

        public DishController(CaloriesContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<IEnumerable<Dish>> GetDishes() => await context.Dishes.ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDish(int id)
        {
            Dish? dish = await context.Dishes
                .Include(d => d.Products)
                .Include(d => d.DishProducts)
                .Include(d => d.HealthyUsers)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null) return NotFound();

            return Ok(dish);
        }

        [HttpGet("{id}/calc")]
        public async Task<IActionResult> GetGlobalCalculations(int id)
        {
            Dish? dish = await context.Dishes
                .Include(d => d.Products)
                .Include(d => d.DishProducts)
                .Include(d => d.HealthyUsers)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null) return NotFound();

            CalculationResult result = new CalculationResult()
            {
                GlobalCalories = dish.GetGlobalCalories(),
                GlobalFats = dish.GetGlobalFats(),
                GlobalCarbohydrates = dish.GetGlobalCarbohydrates(),
                GlobalProteins = dish.GetGlobalProteins()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDish(int id, DishDTO dishDTO)
        {
            Dish dish = new Dish()
            {
                Name = dishDTO.Name
            };

            context.Dishes.Add(dish);
            await context.SaveChangesAsync();
            return Ok(dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, DishDTO dishDTO)
        {
            Dish dish = new Dish()
            {
                Id = dishDTO.Id,
                Name = dishDTO.Name,
            };

            if (id != dishDTO.Id) return BadRequest();
            if (!IsDishExists(id)) return NotFound();

            context.Update(dish);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            Dish? dish = await context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            context.Remove(dish);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool IsDishExists(int id) => context.Dishes.Any(dish => dish.Id == id);
    }
}
