using CaloryCalcApp.Infrastructure.Data;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Infrastructure.Repositories
{
    public class DishRepository : RepositoryBase<Dish>, IDishRepository
    {
        public DishRepository(CaloriesContext context) : base(context) { }

        public async Task<IEnumerable<Dish>> GetWithProductsAsync()
        {
            return await _context.Dishes
                .Include(d => d.DishProducts)
                .ThenInclude(dp => dp.Product)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task<Dish?> GetWithProductsByIdAsync(int id)
        {
            return await _context.Dishes
                .Include(d => d.DishProducts)
                .ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Dish>> SearchByNameAsync(string term)
        {
            return await _context.Dishes
                .Where(d => d.Name.Contains(term))
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public void RemoveDishProducts(IEnumerable<DishProduct> dishProducts)
        {
            _context.DishProducts.RemoveRange(dishProducts);
        }
    }
}
