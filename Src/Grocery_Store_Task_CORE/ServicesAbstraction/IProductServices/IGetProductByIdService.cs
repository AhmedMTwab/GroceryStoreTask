using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices
{
    public interface IGetProductByIdService
    {
        public Task<Product> GetProductByIdAsync(Guid id);

    }
}
