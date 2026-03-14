using Microsoft.AspNetCore.Identity;

namespace CaloryCalcLibrary
{
    public enum Sex
    {
        Male,
        Female
    }

    public class HealthyUser : IdentityUser
    {
        public double Weight { get; set; } = default!;

        public int Height { get; set; } = default!;

        public DateOnly DateOfBirth { get; set; }

        public Sex Sex { get; set; } = default!;

        public List<Dish> Dishes { get; set; } = [];
    }
}
