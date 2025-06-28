using AutoMapper;
using Grocery_Store_Task_CORE.DTOs.ProductDTOs;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Exceptions;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Microsoft.Extensions.Logging;

namespace Grocery_Store_Task_CORE.Services.ProductServices
{
    public class GetAllProductService(ILogger<GetAllProductService> logger, IProductRepository productRepository, IMapper mapper) : IGetAllProductService
    {
        public async Task<IEnumerable<GetProductDTO>> GetAllProductsAsync()
        {
            logger.LogInformation("Fetching Products");
            try
            {
                var products = await productRepository.GetAllProductsAsync();
                if (products == null)
                {
                    logger.LogWarning("products list is null");
                    throw new NotFoundException("There is No Products");
                }
                var productsDTOList = mapper.Map<IEnumerable<GetProductDTO>>(products);
                return productsDTOList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Fetching Products", ex);
            }
        }
        
    }
}
