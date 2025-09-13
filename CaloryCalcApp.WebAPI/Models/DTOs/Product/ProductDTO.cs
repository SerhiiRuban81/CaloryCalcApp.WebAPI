using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Product
{
    public class ProductDTO
    {
        [Display(Name = "Product")]
        public string Name { get; set; } = default!;

        public double Density { get; set; }

        public double Calories { get; set; }

        public double Fats { get; set; }

        public double Carbohydrates { get; set; }

        public double Proteins { get; set; }
    }
}
