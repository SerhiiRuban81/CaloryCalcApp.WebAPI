using System.ComponentModel.DataAnnotations.Schema;

namespace CaloryCalcLibrary
{
    public class HealthyUserDish
    {
        public int Id { get; set; }

        public int DishId { get; set; }

        public float Amount { get; set; }

        public string HealthyUserId { get; set; } = default!;

        public DateTime MealTime { get; set; }

        public HealthyUser HealthyUser { get; set; } = null!;

        public Dish Dish { get; set; } = null!;
    }
}
