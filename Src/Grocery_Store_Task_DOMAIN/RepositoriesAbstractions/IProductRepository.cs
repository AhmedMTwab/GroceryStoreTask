using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_DOMAIN.RepositoriesAbstractions
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(Guid ID);
        public Task<IEnumerable<Product>> GetRangeofProductByIdAsync(IEnumerable<Guid> productIds);
    }
}
