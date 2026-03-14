using AutoMapper;
using CaloryCalcApp.Application.DTOs.DishProducts;
using Xunit;
using CaloryCalcApp.Application.DTOs.Dishes;
using CaloryCalcApp.Application.Services;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using FluentAssertions;
using Moq;

namespace CaloryCalcApp.Tests.Services
{
    public class DishServiceTests
    {
        #region Fields & Setup

        private readonly Mock<IDishRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IDishService _sut;

        public DishServiceTests()
        {
            _sut = new DishService(_repoMock.Object, _mapperMock.Object);
        }

        private static Dish CreateDish(int id = 1, string name = "Salad") =>
            new()
            {
                Id = id,
                Name = name,
                DishProducts =
                [
                    new DishProduct { Id = 1, ProductId = 10, Amount = 100, MeasurementUnit = "g", Product = new Product { Name = "Tomato" } }
                ]
            };

        private static DishDto CreateDishDto(int id = 1, string name = "Salad") =>
            new()
            {
                Id = id,
                Name = name,
                DishProducts =
                [
                    new DishProductDto { Id = 1, ProductId = 10, Amount = 100, MeasurementUnit = "g" }
                ]
            };

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_Always_ReturnsMappedDtos()
        {
            // Arrange
            var dishes = new List<Dish> { CreateDish() };
            var dtos = new List<DishDto> { CreateDishDto() };
            _repoMock.Setup(r => r.GetWithProductsAsync()).ReturnsAsync(dishes);
            _mapperMock.Setup(m => m.Map<IEnumerable<DishDto>>(dishes)).Returns(dtos);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_IfDishExists_ReturnsMappedDto()
        {
            // Arrange
            var dish = CreateDish();
            var dto = CreateDishDto();
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(1)).ReturnsAsync(dish);
            _mapperMock.Setup(m => m.Map<DishDto>(dish)).Returns(dto);

            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetByIdAsync_IfDishNotFound_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(99)).ReturnsAsync((Dish?)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_Always_BuildsDishWithProductsAndSaves()
        {
            // Arrange
            var dto = new DishDto
            {
                Name = "New Dish",
                DishProducts =
                [
                    new DishProductDto { ProductId = 5, Amount = 200, MeasurementUnit = "g" },
                    new DishProductDto { ProductId = 7, Amount = 100, MeasurementUnit = "ml" }
                ]
            };

            Dish? capturedDish = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Dish>()))
                     .Callback<Dish>(d => capturedDish = d)
                     .Returns(Task.CompletedTask);

            // Act
            await _sut.CreateAsync(dto);

            // Assert
            capturedDish!.Name.Should().Be("New Dish");
            capturedDish.DishProducts.Should().HaveCount(2);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_IfDishExists_UpdatesNameAndProductsAndSaves()
        {
            // Arrange
            var existing = CreateDish();
            var updateDto = new DishDto
            {
                Name = "Updated Salad",
                DishProducts =
                [
                    new DishProductDto { ProductId = 20, Amount = 150, MeasurementUnit = "g" }
                ]
            };
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(1)).ReturnsAsync(existing);

            // Act
            await _sut.UpdateAsync(1, updateDto);

            // Assert
            existing.Name.Should().Be("Updated Salad");
            existing.DishProducts.Should().HaveCount(1)
                .And.Contain(dp => dp.ProductId == 20);
            _repoMock.Verify(r => r.RemoveDishProducts(It.IsAny<IEnumerable<DishProduct>>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_IfDishNotFound_DoesNotSave()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(99)).ReturnsAsync((Dish?)null);

            // Act
            await _sut.UpdateAsync(99, CreateDishDto());

            // Assert
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_IfDishExists_DeletesAndSaves()
        {
            // Arrange
            var dish = CreateDish();
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(1)).ReturnsAsync(dish);

            // Act
            await _sut.DeleteAsync(1);

            // Assert
            _repoMock.Verify(r => r.Delete(dish), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IfDishNotFound_DoesNotDeleteOrSave()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithProductsByIdAsync(99)).ReturnsAsync((Dish?)null);

            // Act
            await _sut.DeleteAsync(99);

            // Assert
            _repoMock.Verify(r => r.Delete(It.IsAny<Dish>()), Times.Never);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion
    }
}
