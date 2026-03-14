namespace CaloryCalcApp.Application.DTOs.HealthyUserDishes
{
    public class StatisticsDataPointDto
    {
        public string DateTime { get; set; } = string.Empty;
        public string DishName { get; set; } = string.Empty;
        public double TotalCalories { get; set; }
        public double TotalFats { get; set; }
        public double TotalProteins { get; set; }
        public double TotalCarbohydrates { get; set; }
    }
}
