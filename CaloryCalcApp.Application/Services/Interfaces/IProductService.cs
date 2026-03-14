using CaloryCalcApp.Application.DTOs.Products;

namespace CaloryCalcApp.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> SearchByNameAsync(string term);
        Task CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
    }
}
