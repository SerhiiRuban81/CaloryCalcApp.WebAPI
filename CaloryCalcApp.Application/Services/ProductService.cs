using AutoMapper;
using CaloryCalcApp.Application.DTOs.Products;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;

namespace CaloryCalcApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> SearchByNameAsync(string term)
        {
            var products = await _productRepository.SearchByNameAsync(term);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task CreateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Delete(product);
                await _productRepository.SaveChangesAsync();
            }
        }
    }
}
