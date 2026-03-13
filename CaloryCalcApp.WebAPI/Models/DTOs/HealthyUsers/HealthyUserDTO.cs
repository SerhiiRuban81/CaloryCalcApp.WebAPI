using CaloryCalcApp.Web.Models.DTOs.Dishes;

namespace CaloryCalcApp.Web.Models.DTOs.HealthyUsers
{
    public class HealthyUserDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; } = default!; // Ім'я користувача

        public double Weight { get; set; } = default!; // Вага

        public int Height { get; set; } = default!; // Зріст

        public DateOnly DateOfBirth { get; set; } // Дата народження

        public string Sex { get; set; } = default!;

        public List<DishDTO> Dishes { get; set; } = [];
    }
}

