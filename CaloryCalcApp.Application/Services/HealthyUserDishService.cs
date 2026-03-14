using AutoMapper;
using CaloryCalcApp.Application.DTOs.HealthyUserDishes;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;

namespace CaloryCalcApp.Application.Services
{
    public class HealthyUserDishService : IHealthyUserDishService
    {
        private readonly IHealthyUserDishRepository _healthyUserDishRepository;
        private readonly IMapper _mapper;

        public HealthyUserDishService(IHealthyUserDishRepository healthyUserDishRepository, IMapper mapper)
        {
            _healthyUserDishRepository = healthyUserDishRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HealthyUserDishDto>> GetAllWithDetailsAsync()
        {
            var items = await _healthyUserDishRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<HealthyUserDishDto>>(items);
        }

        public async Task<IEnumerable<HealthyUserDishDto>> GetByUserIdAsync(string userId)
        {
            var items = await _healthyUserDishRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<HealthyUserDishDto>>(items);
        }

        public async Task<HealthyUserDishDto?> GetByIdAsync(int id)
        {
            var item = await _healthyUserDishRepository.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<HealthyUserDishDto>(item);
        }

        public async Task CreateAsync(HealthyUserDishDto dto)
        {
            var item = _mapper.Map<HealthyUserDish>(dto);
            await _healthyUserDishRepository.AddAsync(item);
            await _healthyUserDishRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(HealthyUserDishDto dto)
        {
            var item = await _healthyUserDishRepository.GetByIdAsync(dto.Id);
            if (item == null) return;

            item.DishId = dto.DishId;
            item.Amount = dto.Amount;
            item.HealthyUserId = dto.HealthyUserId;
            item.MealTime = dto.MealTime;
            _healthyUserDishRepository.Update(item);
            await _healthyUserDishRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _healthyUserDishRepository.GetByIdAsync(id);
            if (item != null)
            {
                _healthyUserDishRepository.Delete(item);
                await _healthyUserDishRepository.SaveChangesAsync();
            }
        }

        public async Task<StatisticsResultDto> GetStatisticsAsync(string userId, int days)
        {
            DateTime? since = days > 0 ? DateTime.Now.AddDays(-days) : null;
            var items = (await _healthyUserDishRepository.GetByUserIdWithDetailsAsync(userId, since)).ToList();

            int actualDays = days > 0
                ? days
                : (items.Count > 0 ? (int)(DateTime.Now - items.Min(h => h.MealTime)).TotalDays + 1 : 1);

            var dataPoints = items
                .OrderBy(h => h.MealTime)
                .Select(h => new StatisticsDataPointDto
                {
                    DateTime = h.MealTime.ToString("yyyy-MM-dd HH:mm"),
                    DishName = h.Dish?.Name ?? string.Empty,
                    TotalCalories = ((h.Dish?.GetGlobalCalories() ?? 0) / 100) * h.Amount,
                    TotalFats = ((h.Dish?.GetGlobalFats() ?? 0) / 100) * h.Amount,
                    TotalProteins = ((h.Dish?.GetGlobalProteins() ?? 0) / 100) * h.Amount,
                    TotalCarbohydrates = ((h.Dish?.GetGlobalCarbohydrates() ?? 0) / 100) * h.Amount
                }).ToList();

            return new StatisticsResultDto
            {
                Dishes = _mapper.Map<List<HealthyUserDishDto>>(items),
                DataPoints = dataPoints,
                ActualDays = actualDays
            };
        }
    }
}

