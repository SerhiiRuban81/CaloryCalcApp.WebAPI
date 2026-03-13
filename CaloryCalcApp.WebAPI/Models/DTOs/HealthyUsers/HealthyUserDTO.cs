using CaloryCalcApp.Web.Models.DTOs.Dishes;

namespace CaloryCalcApp.Web.Models.DTOs.HealthyUsers
{
    public class HealthyUserDto
    {
        public string? Id { get; set; }

        public string? Name { get; set; } = default!;

        public double Weight { get; set; } = default!;

        public int Height { get; set; } = default!;

        public DateOnly DateOfBirth { get; set; }

        public string Sex { get; set; } = default!;

        public List<DishDto> Dishes { get; set; } = [];
    }
}
