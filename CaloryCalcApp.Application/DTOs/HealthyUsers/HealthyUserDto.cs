using CaloryCalcApp.Application.DTOs.Dishes;

namespace CaloryCalcApp.Application.DTOs.HealthyUsers
{
    public class HealthyUserDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public double Weight { get; set; }
        public int Height { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Sex { get; set; } = default!;
        public List<DishDto> Dishes { get; set; } = [];
    }
}
