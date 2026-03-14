using System.ComponentModel.DataAnnotations;

namespace CaloryCalcLibrary
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product")]
        public string Name { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public double? Density { get; set; }

        [Range(0, double.MaxValue)]
        public double? Calories { get; set; }

        [Range(0, double.MaxValue)]
        public double? Fats { get; set; }

        [Range(0, double.MaxValue)]
        public double? Carbohydrates { get; set; }

        [Range(0, double.MaxValue)]
        public double? Proteins { get; set; }

        public List<Dish> Dishes { get; set; } = [];

        public List<DishProduct> DishProducts { get; set; } = [];
    }
}
