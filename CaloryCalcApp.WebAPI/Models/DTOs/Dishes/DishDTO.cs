using CaloryCalcApp.Web.Models.DTOs.DishProducts;
using CaloryCalcLibrary;
using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.Web.Models.DTOs.Dishes
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<DishProductDto> DishProducts { get; set; } = new();
        public double? TotalFats { get; set; }
        public double? TotalCalories { get; set; }
        public double? TotalProteins { get; set; }
        public double? TotalCarbohydrates { get; set; }
    }
}

