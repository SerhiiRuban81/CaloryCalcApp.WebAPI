namespace CaloryCalcApp.Application.DTOs.HealthyUserDishes
{
    public class StatisticsResultDto
    {
        public List<HealthyUserDishDto> Dishes { get; set; } = [];
        public List<StatisticsDataPointDto> DataPoints { get; set; } = [];
        public int ActualDays { get; set; }
    }
}
