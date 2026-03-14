using AutoMapper;
using CaloryCalcApp.Application.DTOs.Dishes;
using Xunit;
using CaloryCalcApp.Application.DTOs.HealthyUsers;
using CaloryCalcApp.Application.DTOs.Products;
using CaloryCalcApp.Application.DTOs.Search;
using CaloryCalcApp.Application.Services;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcApp.Tests.Helpers;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CaloryCalcApp.Tests.Services
{
    public class SearchServiceTests
    {
        #region Fields & Setup

        private readonly Mock<IProductRepository> _productRepoMock = new();
        private readonly Mock<IDishRepository> _dishRepoMock = new();
        private readonly Mock<UserManager<HealthyUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ISearchService _sut;

        public SearchServiceTests()
        {
            var store = new Mock<IUserStore<HealthyUser>>();
            _userManagerMock = new Mock<UserManager<HealthyUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _sut = new SearchService(
                _productRepoMock.Object,
                _dishRepoMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object);
        }

        private void SetupUsersQueryable(List<HealthyUser> users)
        {
            var queryable = new TestAsyncEnumerable<HealthyUser>(users.AsQueryable());
            _userManagerMock.Setup(m => m.Users).Returns(queryable);
        }

        #endregion

        #region Empty input guards

        [Fact]
        public async Task SearchAsync_IfSearchTextEmpty_ReturnsEmptyResult()
        {
            // Act
            var result = await _sut.SearchAsync(string.Empty, "Product");

            // Assert
            result.ProductsFound.Should().BeEmpty();
            result.DishesFound.Should().BeEmpty();
            result.UsersFound.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_IfSearchTypeEmpty_ReturnsEmptyResult()
        {
            // Act
            var result = await _sut.SearchAsync("Apple", string.Empty);

            // Assert
            result.ProductsFound.Should().BeEmpty();
            result.DishesFound.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_IfBothEmpty_PreservesSearchTextAndType()
        {
            // Act
            var result = await _sut.SearchAsync(string.Empty, string.Empty);

            // Assert
            result.SearchText.Should().BeEmpty();
            result.SearchType.Should().BeEmpty();
        }

        #endregion

        #region SearchType = "Product"

        [Fact]
        public async Task SearchAsync_IfTypeProduct_SearchesOnlyProducts()
        {
            // Arrange
            var products = new List<Product> { new() { Id = 1, Name = "Apple" } };
            var dtos = new List<ProductDto> { new() { Id = 1, Name = "Apple" } };
            _productRepoMock.Setup(r => r.SearchByNameAsync("Apple")).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<List<ProductDto>>(products)).Returns(dtos);

            // Act
            var result = await _sut.SearchAsync("Apple", "Product");

            // Assert
            result.ProductsFound.Should().BeEquivalentTo(dtos);
            _dishRepoMock.Verify(r => r.SearchByNameAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region SearchType = "Dish"

        [Fact]
        public async Task SearchAsync_IfTypeDish_SearchesOnlyDishes()
        {
            // Arrange
            var dishes = new List<Dish> { new() { Id = 1, Name = "Borsch" } };
            var dtos = new List<DishDto> { new() { Id = 1, Name = "Borsch" } };
            _dishRepoMock.Setup(r => r.SearchByNameAsync("Borsch")).ReturnsAsync(dishes);
            _mapperMock.Setup(m => m.Map<List<DishDto>>(dishes)).Returns(dtos);

            // Act
            var result = await _sut.SearchAsync("Borsch", "Dish");

            // Assert
            result.DishesFound.Should().BeEquivalentTo(dtos);
            _productRepoMock.Verify(r => r.SearchByNameAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region SearchType = "User"

        [Fact]
        public async Task SearchAsync_IfTypeUser_SearchesOnlyUsers()
        {
            // Arrange
            var users = new List<HealthyUser> { new() { Id = "u1", UserName = "john", Email = "john@test.com" } };
            var dtos = new List<HealthyUserDto> { new() { Id = "u1", UserName = "john" } };
            SetupUsersQueryable(users);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDto>>(It.IsAny<List<HealthyUser>>())).Returns(dtos);

            // Act
            var result = await _sut.SearchAsync("john", "User");

            // Assert
            result.UsersFound.Should().BeEquivalentTo(dtos);
            _productRepoMock.Verify(r => r.SearchByNameAsync(It.IsAny<string>()), Times.Never);
            _dishRepoMock.Verify(r => r.SearchByNameAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region SearchType = "FullSearch"

        [Fact]
        public async Task SearchAsync_IfTypeFullSearch_SearchesProductsDishesAndUsers()
        {
            // Arrange
            var products = new List<Product> { new() { Id = 1, Name = "Apple" } };
            var dishes = new List<Dish> { new() { Id = 1, Name = "Salad" } };
            var users = new List<HealthyUser> { new() { Id = "u1", UserName = "Apple_fan", Email = "fan@test.com" } };

            _productRepoMock.Setup(r => r.SearchByNameAsync("Apple")).ReturnsAsync(products);
            _dishRepoMock.Setup(r => r.SearchByNameAsync("Apple")).ReturnsAsync(dishes);
            SetupUsersQueryable(users);
            _mapperMock.Setup(m => m.Map<List<ProductDto>>(products)).Returns([new ProductDto { Name = "Apple" }]);
            _mapperMock.Setup(m => m.Map<List<DishDto>>(dishes)).Returns([new DishDto { Name = "Salad" }]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDto>>(It.IsAny<List<HealthyUser>>())).Returns([new HealthyUserDto { UserName = "Apple_fan" }]);

            // Act
            var result = await _sut.SearchAsync("Apple", "FullSearch");

            // Assert
            result.ProductsFound.Should().HaveCount(1);
            result.DishesFound.Should().HaveCount(1);
            result.UsersFound.Should().HaveCount(1);
        }

        #endregion
    }
}
