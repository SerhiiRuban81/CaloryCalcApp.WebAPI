using AutoMapper;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcApp.Application.DTOs.Search;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcApp.Application.DTOs.Products;
using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDishRepository _dishRepository;
        private readonly UserManager<HealthyUser> _userManager;
        private readonly IMapper _mapper;

        public SearchService(
            IProductRepository productRepository,
            IDishRepository dishRepository,
            UserManager<HealthyUser> userManager,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _dishRepository = dishRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<SearchResultDto> SearchAsync(string searchText, string searchType)
        {
            var result = new SearchResultDto
            {
                SearchText = searchText,
                SearchType = searchType
            };

            if (string.IsNullOrEmpty(searchText) || string.IsNullOrEmpty(searchType))
                return result;

            if (searchType == "Product" || searchType == "FullSearch")
            {
                var products = await _productRepository.SearchByNameAsync(searchText);
                result.ProductsFound = _mapper.Map<List<ProductDto>>(products);
            }

            if (searchType == "Dish" || searchType == "FullSearch")
            {
                var dishes = await _dishRepository.SearchByNameAsync(searchText);
                result.DishesFound = _mapper.Map<List<DishDto>>(dishes);
            }

            if (searchType == "User" || searchType == "FullSearch")
            {
                var users = await _userManager.Users
                    .Where(u => u.UserName!.Contains(searchText) || u.Email!.Contains(searchText))
                    .OrderBy(u => u.Id)
                    .ToListAsync();
                result.UsersFound = _mapper.Map<List<HealthyUserDto>>(users);
            }

            return result;
        }
    }
}
