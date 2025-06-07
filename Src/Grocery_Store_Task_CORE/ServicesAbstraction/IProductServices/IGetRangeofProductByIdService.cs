using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices
{
    public interface IGetRangeofProductByIdService
    {
        public Task<IEnumerable<Product>> GetRangeofProductByIdAsync(IEnumerable<Guid> productsIds);
    }
}
