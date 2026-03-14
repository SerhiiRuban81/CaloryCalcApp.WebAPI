using AutoMapper;
using CaloryCalcApp.Application.DTOs.HealthyUserDishes;
using Xunit;
using CaloryCalcApp.Application.Services;
using CaloryCalcApp.Application.Services.Interfaces;
using CaloryCalcLibrary;
using CaloryCalcLibrary.Interfaces;
using FluentAssertions;
using Moq;

namespace CaloryCalcApp.Tests.Services
{
    public class HealthyUserDishServiceTests
    {
        #region Fields & Setup

        private readonly Mock<IHealthyUserDishRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IHealthyUserDishService _sut;

        public HealthyUserDishServiceTests()
        {
            _sut = new HealthyUserDishService(_repoMock.Object, _mapperMock.Object);
        }

        private static HealthyUserDish CreateEntity(int id = 1, string userId = "user1", int dishId = 10) =>
            new()
            {
                Id = id,
                DishId = dishId,
                Amount = 200f,
                HealthyUserId = userId,
                MealTime = new DateTime(2025, 1, 15, 12, 0, 0),
                Dish = new Dish
                {
                    Id = dishId,
                    Name = "Borsch",
                    DishProducts =
                    [
                        new DishProduct
                        {
                            Amount = 100f,
                            MeasurementUnit = "g",
                            Product = new Product { Calories = 50, Fats = 1, Proteins = 2, Carbohydrates = 8, Density = 1 }
                        }
                    ]
                }
            };

        private static HealthyUserDishDto CreateDto(int id = 1, string userId = "user1", int dishId = 10) =>
            new() { Id = id, DishId = dishId, Amount = 200f, HealthyUserId = userId, MealTime = new DateTime(2025, 1, 15, 12, 0, 0) };

        #endregion

        #region GetAllWithDetailsAsync

        [Fact]
        public async Task GetAllWithDetailsAsync_Always_ReturnsMappedDtos()
        {
            // Arrange
            var entities = new List<HealthyUserDish> { CreateEntity() };
            var dtos = new List<HealthyUserDishDto> { CreateDto() };
            _repoMock.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<HealthyUserDishDto>>(entities)).Returns(dtos);

            // Act
            var result = await _sut.GetAllWithDetailsAsync();

            // Assert
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion

        #region GetByUserIdAsync

        [Fact]
        public async Task GetByUserIdAsync_Always_ReturnsMappedDtos()
        {
            // Arrange
            var entities = new List<HealthyUserDish> { CreateEntity() };
            var dtos = new List<HealthyUserDishDto> { CreateDto() };
            _repoMock.Setup(r => r.GetByUserIdAsync("user1")).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<HealthyUserDishDto>>(entities)).Returns(dtos);

            // Act
            var result = await _sut.GetByUserIdAsync("user1");

            // Assert
            result.Should().BeEquivalentTo(dtos);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_IfEntityExists_ReturnsMappedDto()
        {
            // Arrange
            var entity = CreateEntity();
            var dto = CreateDto();
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<HealthyUserDishDto>(entity)).Returns(dto);

            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetByIdAsync_IfEntityNotFound_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((HealthyUserDish?)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_Always_MapsAndPersists()
        {
            // Arrange
            var dto = CreateDto();
            var entity = CreateEntity();
            _mapperMock.Setup(m => m.Map<HealthyUserDish>(dto)).Returns(entity);

            // Act
            await _sut.CreateAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(entity), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_IfEntityExists_UpdatesFieldsAndSaves()
        {
            // Arrange
            var entity = CreateEntity();
            var dto = new HealthyUserDishDto
            {
                Id = 1,
                DishId = 99,
                Amount = 350f,
                HealthyUserId = "user2",
                MealTime = new DateTime(2025, 6, 1, 8, 0, 0)
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            await _sut.UpdateAsync(dto);

            // Assert
            entity.DishId.Should().Be(99);
            entity.Amount.Should().Be(350f);
            entity.HealthyUserId.Should().Be("user2");
            entity.MealTime.Should().Be(new DateTime(2025, 6, 1, 8, 0, 0));
            _repoMock.Verify(r => r.Update(entity), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_IfEntityNotFound_DoesNotSave()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((HealthyUserDish?)null);

            // Act
            await _sut.UpdateAsync(new HealthyUserDishDto { Id = 99 });

            // Assert
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_IfEntityExists_DeletesAndSaves()
        {
            // Arrange
            var entity = CreateEntity();
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            await _sut.DeleteAsync(1);

            // Assert
            _repoMock.Verify(r => r.Delete(entity), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IfEntityNotFound_DoesNotDeleteOrSave()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((HealthyUserDish?)null);

            // Act
            await _sut.DeleteAsync(99);

            // Assert
            _repoMock.Verify(r => r.Delete(It.IsAny<HealthyUserDish>()), Times.Never);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region GetStatisticsAsync

        [Fact]
        public async Task GetStatisticsAsync_IfDaysPositive_UsesDateFilterAndReturnsCorrectActualDays()
        {
            // Arrange
            var entity = CreateEntity();
            var dtos = new List<HealthyUserDishDto> { CreateDto() };
            _repoMock.Setup(r => r.GetByUserIdWithDetailsAsync("user1", It.IsAny<DateTime?>()))
                     .ReturnsAsync([entity]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDishDto>>(It.IsAny<List<HealthyUserDish>>()))
                       .Returns(dtos);

            // Act
            var result = await _sut.GetStatisticsAsync("user1", 7);

            // Assert
            result.ActualDays.Should().Be(7);
            _repoMock.Verify(r => r.GetByUserIdWithDetailsAsync("user1", It.Is<DateTime?>(d => d.HasValue)), Times.Once);
        }

        [Fact]
        public async Task GetStatisticsAsync_IfDaysZeroAndItemsExist_CalculatesActualDaysFromItems()
        {
            // Arrange
            var oldMealTime = DateTime.Now.AddDays(-9);
            var entity = CreateEntity();
            entity.MealTime = oldMealTime;

            _repoMock.Setup(r => r.GetByUserIdWithDetailsAsync("user1", null))
                     .ReturnsAsync([entity]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDishDto>>(It.IsAny<List<HealthyUserDish>>()))
                       .Returns([CreateDto()]);

            // Act
            var result = await _sut.GetStatisticsAsync("user1", 0);

            // Assert
            result.ActualDays.Should().BeGreaterThanOrEqualTo(10);
            _repoMock.Verify(r => r.GetByUserIdWithDetailsAsync("user1", null), Times.Once);
        }

        [Fact]
        public async Task GetStatisticsAsync_IfDaysZeroAndNoItems_ActualDaysIsOne()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByUserIdWithDetailsAsync("user1", null))
                     .ReturnsAsync([]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDishDto>>(It.IsAny<List<HealthyUserDish>>()))
                       .Returns([]);

            // Act
            var result = await _sut.GetStatisticsAsync("user1", 0);

            // Assert
            result.ActualDays.Should().Be(1);
        }

        [Fact]
        public async Task GetStatisticsAsync_Always_BuildsCorrectDataPoints()
        {
            // Arrange
            var entity = CreateEntity();
            _repoMock.Setup(r => r.GetByUserIdWithDetailsAsync("user1", It.IsAny<DateTime?>()))
                     .ReturnsAsync([entity]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDishDto>>(It.IsAny<List<HealthyUserDish>>()))
                       .Returns([CreateDto()]);

            // Act
            var result = await _sut.GetStatisticsAsync("user1", 7);

            // Assert
            result.DataPoints.Should().HaveCount(1);
            result.DataPoints[0].DishName.Should().Be("Borsch");
            result.DataPoints[0].TotalCalories.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetStatisticsAsync_WithNullDish_UsesZeroForNutrients()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Dish = null!;

            _repoMock.Setup(r => r.GetByUserIdWithDetailsAsync("user1", It.IsAny<DateTime?>()))
                     .ReturnsAsync([entity]);
            _mapperMock.Setup(m => m.Map<List<HealthyUserDishDto>>(It.IsAny<List<HealthyUserDish>>()))
                       .Returns([CreateDto()]);

            // Act
            var result = await _sut.GetStatisticsAsync("user1", 7);

            // Assert
            result.DataPoints[0].TotalCalories.Should().Be(0);
            result.DataPoints[0].DishName.Should().BeEmpty();
        }

        #endregion
    }
}
