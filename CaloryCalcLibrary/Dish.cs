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

        [NotMapped]
        public double GlobalFats => DishProducts.Select(t => t.Amount * t.Product.Fats * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        [NotMapped]
        public double GlobalCalories => DishProducts.Select(t => t.Amount * t.Product.Calories * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        [NotMapped]
        public double GlobalCarbohydrates => DishProducts.Select(t => t.Amount * t.Product.Carbohydrates * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();

        [NotMapped]
        public double GlobalProteins => DishProducts.Select(t => t.Amount * t.Product.Proteins * (t.MeasurementUnit == "g" ? 1 : t.Product.Density) / 100).Sum();
    }
}
