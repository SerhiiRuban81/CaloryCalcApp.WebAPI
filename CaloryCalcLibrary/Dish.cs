using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaloryCalcLibrary
{
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name = "Dish name")]
        public string Name { get; set; } = default!;

        public List<Product> Products { get; set; } = [];

        public List<DishProduct> DishProducts { get; set; } = [];

        public List<HealthyUser> HealthyUsers { get; set; } = [];
        
        public double GetGlobalFats() => DishProducts.Select(t => t.Amount * t.Product.Fats * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        public double GetGlobalCalories() => DishProducts.Select(t => t.Amount * t.Product.Calories * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        public double GetGlobalCarbohydrates() => DishProducts.Select(t => t.Amount * t.Product.Carbohydrates * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        public double GetGlobalProteins() => DishProducts.Select(t => t.Amount * t.Product.Proteins * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();
    }
}
