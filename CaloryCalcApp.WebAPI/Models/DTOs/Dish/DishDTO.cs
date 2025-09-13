using CaloryCalcLibrary;
using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Dish
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
