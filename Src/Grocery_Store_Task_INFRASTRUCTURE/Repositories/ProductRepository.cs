using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Grocery_Store_Task_INFRASTRUCTURE.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Store_Task_INFRASTRUCTURE.Repositories
{
    public class ProductRepository(GroceryStoreDbContext db) : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
           
                var products = await db.Products.ToListAsync();
                return products;
            

        }

        public async Task<Product?> GetProductByIdAsync(Guid ID)
        {
           
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == ID);
                return product;
            

        }
        public async Task<IEnumerable<Product>> GetRangeofProductByIdAsync(IEnumerable<Guid> productIds)
        {
            
                var productsWithIds = await db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                return productsWithIds;
           
        }
    }
}
