using AutoMapper;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Exceptions;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Microsoft.Extensions.Logging;

namespace Grocery_Store_Task_CORE.Services.ProductServices
{
    public class GetProductByIdService(ILogger<GetProductByIdService> logger, IProductRepository productRepository, IMapper mapper) : IGetProductByIdService, IGetRangeofProductByIdService
    {
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    logger.LogWarning($"Product id is Empty in method:{nameof(GetProductByIdAsync)} in type:{nameof(GetProductByIdService)}");
                    throw new ArgumentException("Product id cant be Empty");
                }
                logger.LogInformation($"Getting  Product By iD:{id}");
                var product = await productRepository.GetProductByIdAsync(id);
                if (product == null)
                    throw new NotFoundException(nameof(Product), id.ToString());
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Fetching product with id: {id}", ex);
            }
        }
        public async Task<IEnumerable<Product>> GetRangeofProductByIdAsync(IEnumerable<Guid> productsIds)
        {
            try
            {
                if (productsIds == null)
                {
                    logger.LogWarning($"Productids list  is null in method:{nameof(GetRangeofProductByIdAsync)} in type:{nameof(GetProductByIdService)}");
                    throw new ArgumentException("Productids List cant be Null");
                }
                if (productsIds.Count() < 1)
                {
                    logger.LogWarning($"Productids list  is Empty in method:{nameof(GetRangeofProductByIdAsync)} in type:{nameof(GetProductByIdService)}");
                    throw new ArgumentException("Productids List cant be Empty");
                }
                var products = await productRepository.GetRangeofProductByIdAsync(productsIds);
                if (products == null)
                    throw new NotFoundException("Product list not found");
                return products;
            }
            catch (Exception ex) {
                throw new Exception("Error Fetching Products", ex);
                    }
        }

    }
}
