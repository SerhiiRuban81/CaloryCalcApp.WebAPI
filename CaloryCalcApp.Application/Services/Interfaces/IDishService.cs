using CaloryCalcApp.Application.DTOs.Dishes;

namespace CaloryCalcApp.Application.Services.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<DishDto>> GetAllAsync();
        Task<DishDto?> GetByIdAsync(int id);
        Task CreateAsync(DishDto dto);
        Task UpdateAsync(int id, DishDto dto);
        Task DeleteAsync(int id);
    }
}
