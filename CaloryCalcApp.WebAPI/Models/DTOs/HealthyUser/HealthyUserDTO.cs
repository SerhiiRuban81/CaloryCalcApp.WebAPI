using CaloryCalcApp.WebAPI.Models.DTOs.Dish;

namespace CaloryCalcApp.WebAPI.Models.DTOs.HealthyUserDTO
{
    public class HealthyUserDTO
    {
        public double Weight { get; set; } = default!; // Вага

        public int Height { get; set; } = default!; // Зріст

        public DateOnly DateOfBirth { get; set; } // Дата народження

        public string Sex { get; set; } = default!;

        public List<DishDTO> Dishes { get; set; } = [];
    }
}
