using AutoMapper;
using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;

namespace CaloryCalcApp.Application.Services
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public DishService(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DishDto>> GetAllAsync()
        {
            var dishes = await _dishRepository.GetWithProductsAsync();
            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }

        public async Task<DishDto?> GetByIdAsync(int id)
        {
            var dish = await _dishRepository.GetWithProductsByIdAsync(id);
            return dish == null ? null : _mapper.Map<DishDto>(dish);
        }

        public async Task CreateAsync(DishDto dto)
        {
            var dish = new Dish { Name = dto.Name };
            foreach (var dp in dto.DishProducts)
            {
                dish.DishProducts.Add(new DishProduct
                {
                    ProductId = dp.ProductId,
                    Amount = dp.Amount,
                    MeasurementUnit = dp.MeasurementUnit
                });
            }
            await _dishRepository.AddAsync(dish);
            await _dishRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, DishDto dto)
        {
            var dish = await _dishRepository.GetWithProductsByIdAsync(id);
            if (dish == null) return;

            dish.Name = dto.Name;

            _dishRepository.RemoveDishProducts(dish.DishProducts);
            dish.DishProducts.Clear();

            foreach (var dpDto in dto.DishProducts)
            {
                dish.DishProducts.Add(new DishProduct
                {
                    ProductId = dpDto.ProductId,
                    Amount = dpDto.Amount,
                    MeasurementUnit = dpDto.MeasurementUnit
                });
            }

            await _dishRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dish = await _dishRepository.GetWithProductsByIdAsync(id);
            if (dish != null)
            {
                _dishRepository.Delete(dish);
                await _dishRepository.SaveChangesAsync();
            }
        }
    }
}
