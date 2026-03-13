namespace CaloryCalcApp.Web.Models.DTOs.HealthyUserDishes
{
    public class HealthyUserDishDto
    {
        public int Id { get; set; }

        public int DishId { get; set; }

        public float Amount { get; set; }

        public string HealthyUserId { get; set; } = default!;

        public DateTime MealTime { get; set; }
    }
}

