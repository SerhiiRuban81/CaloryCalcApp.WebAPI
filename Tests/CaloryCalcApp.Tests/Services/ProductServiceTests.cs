using AutoMapper;
using CaloryCalcApp.Application.DTOs.Products;
using Xunit;
using CaloryCalcApp.Application.Services;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using FluentAssertions;
using Moq;

namespace CaloryCalcApp.Tests.Services
{
    public class ProductServiceTests
    {
        #region Fields & Setup

        private readonly Mock<IProductRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IProductService _sut;

        public ProductServiceTests()
        {
            _sut = new ProductService(_repoMock.Object, _mapperMock.Object);
        }

        private static Product CreateProduct(int id = 1, string name = "Apple") =>
            new() { Id = id, Name = name, Calories = 52, Fats = 0.2, Proteins = 0.3, Carbohydrates = 14 };

        private static ProductDto CreateProductDto(int id = 1, string name = "Apple") =>
            new() { Id = id, Name = name, Calories = 52, Fats = 0.2, Proteins = 0.3, Carbohydrates = 14 };

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_Always_ReturnsMappedDtos()
        {
            // Arrange
            var products = new List<Product> { CreateProduct(1), CreateProduct(2, "Banana") };
            var dtos = new List<ProductDto> { CreateProductDto(1), CreateProductDto(2, "Banana") };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(dtos);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_IfProductExists_ReturnsMappedDto()
        {
            // Arrange
            var product = CreateProduct();
            var dto = CreateProductDto();
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(dto);

            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetByIdAsync_IfProductNotFound_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region SearchByNameAsync

        [Fact]
        public async Task SearchByNameAsync_Always_ReturnsMappedDtos()
        {
            // Arrange
            var products = new List<Product> { CreateProduct(1, "Apple Juice") };
            var dtos = new List<ProductDto> { CreateProductDto(1, "Apple Juice") };
            _repoMock.Setup(r => r.SearchByNameAsync("Apple")).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(dtos);

            // Act
            var result = await _sut.SearchByNameAsync("Apple");

            // Assert
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_Always_MapsAndPersists()
        {
            // Arrange
            var dto = CreateProductDto();
            var product = CreateProduct();
            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(product);

            // Act
            await _sut.CreateAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_Always_MapsAndPersists()
        {
            // Arrange
            var dto = CreateProductDto();
            var product = CreateProduct();
            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(product);

            // Act
            await _sut.UpdateAsync(dto);

            // Assert
            _repoMock.Verify(r => r.Update(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_IfProductExists_DeletesAndSaves()
        {
            // Arrange
            var product = CreateProduct();
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            await _sut.DeleteAsync(1);

            // Assert
            _repoMock.Verify(r => r.Delete(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IfProductNotFound_DoesNotDeleteOrSave()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            // Act
            await _sut.DeleteAsync(99);

            // Assert
            _repoMock.Verify(r => r.Delete(It.IsAny<Product>()), Times.Never);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion
    }
}
