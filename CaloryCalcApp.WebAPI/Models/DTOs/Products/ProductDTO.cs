using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Products
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product")]
        public string Name { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public double Density { get; set; }

        [Range(0, double.MaxValue)]
        public double? Calories { get; set; }

        [Range(0, double.MaxValue)]
        public double? Fats { get; set; }

        [Range(0, double.MaxValue)]
        public double? Carbohydrates { get; set; }

        [Range(0, double.MaxValue)]
        public double? Proteins { get; set; }
    }
}
