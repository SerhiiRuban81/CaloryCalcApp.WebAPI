using CaloryCalcApp.Infrastructure.Data;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(CaloriesContext context) : base(context) { }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string term)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(term))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
