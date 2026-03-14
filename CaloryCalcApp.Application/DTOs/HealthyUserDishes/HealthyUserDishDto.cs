namespace CaloryCalcApp.Application.DTOs.HealthyUserDishes
{
    public class HealthyUserDishDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public float Amount { get; set; }
        public string HealthyUserId { get; set; } = default!;
        public DateTime MealTime { get; set; }
        public string DishName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
