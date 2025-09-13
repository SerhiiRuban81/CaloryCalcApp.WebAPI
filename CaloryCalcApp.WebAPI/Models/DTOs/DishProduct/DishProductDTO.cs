using System.ComponentModel.DataAnnotations.Schema;

namespace CaloryCalcApp.WebAPI.Models.DTOs.DishProduct
{
    public class DishProductDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DishId { get; set; }
        public string MeasurementUnit { get; set; } = default!;
        public float Amount { get; set; }
    }
}
