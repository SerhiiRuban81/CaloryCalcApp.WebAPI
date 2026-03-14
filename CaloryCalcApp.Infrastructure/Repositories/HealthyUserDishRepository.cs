using CaloryCalcApp.Infrastructure.Data;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Infrastructure.Repositories
{
    public class HealthyUserDishRepository : RepositoryBase<HealthyUserDish>, IHealthyUserDishRepository
    {
        public HealthyUserDishRepository(CaloriesContext context) : base(context) { }

        public override async Task<HealthyUserDish?> GetByIdAsync(int id)
        {
            return await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<HealthyUserDish>> GetByUserIdAsync(string userId)
        {
            return await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .Where(h => h.HealthyUserId == userId)
                .OrderBy(h => h.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthyUserDish>> GetAllWithDetailsAsync()
        {
            return await _context.HealthyUserDishes
                .Include(h => h.Dish)
                .Include(h => h.HealthyUser)
                .OrderBy(h => h.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthyUserDish>> GetByUserIdWithDetailsAsync(string userId, DateTime? since)
        {
            var query = _context.HealthyUserDishes
                .Include(h => h.Dish)
                .ThenInclude(d => d.DishProducts)
                .ThenInclude(dp => dp.Product)
                .Include(h => h.HealthyUser)
                .Where(h => h.HealthyUserId == userId);

            if (since.HasValue)
                query = query.Where(h => h.MealTime >= since.Value);

            return await query.OrderBy(h => h.MealTime).ToListAsync();
        }
    }
}
