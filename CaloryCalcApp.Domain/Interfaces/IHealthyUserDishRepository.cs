namespace CaloryCalcLibrary.Interfaces
{
    public interface IHealthyUserDishRepository : IRepository<HealthyUserDish>
    {
        Task<IEnumerable<HealthyUserDish>> GetByUserIdAsync(string userId);
        Task<IEnumerable<HealthyUserDish>> GetAllWithDetailsAsync();
        Task<IEnumerable<HealthyUserDish>> GetByUserIdWithDetailsAsync(string userId, DateTime? since);
    }
}
