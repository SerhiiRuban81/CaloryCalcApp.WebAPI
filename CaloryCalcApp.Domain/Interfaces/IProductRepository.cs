namespace CaloryCalcLibrary.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> SearchByNameAsync(string term);
    }
}
