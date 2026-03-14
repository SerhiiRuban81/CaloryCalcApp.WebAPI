namespace CaloryCalcLibrary.Interfaces
{
    public interface IDishRepository : IRepository<Dish>
    {
        Task<IEnumerable<Dish>> GetWithProductsAsync();
        Task<Dish?> GetWithProductsByIdAsync(int id);
        Task<IEnumerable<Dish>> SearchByNameAsync(string term);
        void RemoveDishProducts(IEnumerable<DishProduct> dishProducts);
    }
}
