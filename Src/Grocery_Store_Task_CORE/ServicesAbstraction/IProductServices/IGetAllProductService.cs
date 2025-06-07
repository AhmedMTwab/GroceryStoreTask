using Grocery_Store_Task_CORE.DTOs.ProductDTOs;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices
{
    public interface IGetAllProductService
    {
        public Task<IEnumerable<GetProductDTO>> GetAllProductsAsync();

    }
}
