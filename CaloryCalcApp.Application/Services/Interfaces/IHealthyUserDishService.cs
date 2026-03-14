using CaloryCalcApp.Application.DTOs.HealthyUserDishes;

namespace CaloryCalcApp.Application.Services.Interfaces
{
    public interface IHealthyUserDishService
    {
        Task<IEnumerable<HealthyUserDishDto>> GetAllWithDetailsAsync();
        Task<IEnumerable<HealthyUserDishDto>> GetByUserIdAsync(string userId);
        Task<HealthyUserDishDto?> GetByIdAsync(int id);
        Task CreateAsync(HealthyUserDishDto dto);
        Task UpdateAsync(HealthyUserDishDto dto);
        Task DeleteAsync(int id);
        Task<StatisticsResultDto> GetStatisticsAsync(string userId, int days);
    }
}
