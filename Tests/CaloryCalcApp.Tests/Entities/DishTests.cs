using CaloryCalcLibrary;
using FluentAssertions;
using Xunit;

namespace CaloryCalcApp.Tests.Entities
{
    public class DishTests
    {
        #region Helpers

        private static DishProduct CreateDishProduct(
            double? calories = 100,
            double? fats = 10,
            double? proteins = 5,
            double? carbohydrates = 20,
            double? density = 0.9,
            float amount = 100,
            string unit = "g")
            => new()
            {
                Amount = amount,
                MeasurementUnit = unit,
                Product = new Product
                {
                    Calories = calories,
                    Fats = fats,
                    Proteins = proteins,
                    Carbohydrates = carbohydrates,
                    Density = density
                }
            };

        private static Dish CreateDishWith(params DishProduct[] dishProducts)
        {
            var dish = new Dish { Name = "TestDish" };
            foreach (var dp in dishProducts)
                dish.DishProducts.Add(dp);
            return dish;
        }

        #endregion

        #region GetGlobalCalories

        [Fact]
        public void GetGlobalCalories_WithGramUnit_CalculatesWithoutDensity()
        {
            // Arrange — 100g of product with 100 kcal/100g => 100 kcal
            var dish = CreateDishWith(CreateDishProduct(calories: 100, density: 0.9, amount: 100, unit: "g"));

            // Act & Assert
            dish.GetGlobalCalories().Should().BeApproximately(100.0, 0.001);
        }

        [Fact]
        public void GetGlobalCalories_WithNonGramUnit_AppliesDensity()
        {
            // Arrange — 100ml of product with 100 kcal/100g and density 0.8 => 100 * 100 * 0.8 / 100 = 80 kcal
            var dish = CreateDishWith(CreateDishProduct(calories: 100, density: 0.8, amount: 100, unit: "ml"));

            // Act & Assert
            dish.GetGlobalCalories().Should().BeApproximately(80.0, 0.001);
        }

        [Fact]
        public void GetGlobalCalories_WithMultipleProducts_SumsAllContributions()
        {
            // Arrange
            var dp1 = CreateDishProduct(calories: 100, density: 1.0, amount: 100, unit: "g");  // 100
            var dp2 = CreateDishProduct(calories: 200, density: 1.0, amount: 50, unit: "g");   // 100
            var dish = CreateDishWith(dp1, dp2);

            // Act & Assert
            dish.GetGlobalCalories().Should().BeApproximately(200.0, 0.001);
        }

        [Fact]
        public void GetGlobalCalories_WithNoDishProducts_ReturnsZero()
        {
            // Arrange
            var dish = new Dish { Name = "Empty" };

            // Act & Assert
            dish.GetGlobalCalories().Should().Be(0);
        }

        #endregion

        #region GetGlobalFats

        [Fact]
        public void GetGlobalFats_WithGramUnit_CalculatesWithoutDensity()
        {
            // Arrange — 200g, 10 fats/100g => 20
            var dish = CreateDishWith(CreateDishProduct(fats: 10, density: 1.0, amount: 200, unit: "g"));

            // Act & Assert
            dish.GetGlobalFats().Should().BeApproximately(20.0, 0.001);
        }

        [Fact]
        public void GetGlobalFats_WithNonGramUnit_AppliesDensity()
        {
            // Arrange — 200ml, 10 fats/100g, density 0.5 => 200 * 10 * 0.5 / 100 = 10
            var dish = CreateDishWith(CreateDishProduct(fats: 10, density: 0.5, amount: 200, unit: "ml"));

            // Act & Assert
            dish.GetGlobalFats().Should().BeApproximately(10.0, 0.001);
        }

        #endregion

        #region GetGlobalProteins

        [Fact]
        public void GetGlobalProteins_WithGramUnit_CalculatesWithoutDensity()
        {
            // Arrange — 150g, 5 proteins/100g => 7.5
            var dish = CreateDishWith(CreateDishProduct(proteins: 5, density: 1.0, amount: 150, unit: "g"));

            // Act & Assert
            dish.GetGlobalProteins().Should().BeApproximately(7.5, 0.001);
        }

        [Fact]
        public void GetGlobalProteins_WithNonGramUnit_AppliesDensity()
        {
            // Arrange — 150ml, 5 proteins/100g, density 0.6 => 150 * 5 * 0.6 / 100 = 4.5
            var dish = CreateDishWith(CreateDishProduct(proteins: 5, density: 0.6, amount: 150, unit: "ml"));

            // Act & Assert
            dish.GetGlobalProteins().Should().BeApproximately(4.5, 0.001);
        }

        #endregion

        #region GetGlobalCarbohydrates

        [Fact]
        public void GetGlobalCarbohydrates_WithGramUnit_CalculatesWithoutDensity()
        {
            // Arrange — 100g, 20 carbs/100g => 20
            var dish = CreateDishWith(CreateDishProduct(carbohydrates: 20, density: 1.0, amount: 100, unit: "g"));

            // Act & Assert
            dish.GetGlobalCarbohydrates().Should().BeApproximately(20.0, 0.001);
        }

        [Fact]
        public void GetGlobalCarbohydrates_WithNonGramUnit_AppliesDensity()
        {
            // Arrange — 100ml, 20 carbs/100g, density 1.2 => 100 * 20 * 1.2 / 100 = 24
            var dish = CreateDishWith(CreateDishProduct(carbohydrates: 20, density: 1.2, amount: 100, unit: "ml"));

            // Act & Assert
            dish.GetGlobalCarbohydrates().Should().BeApproximately(24.0, 0.001);
        }

        #endregion
    }
}
