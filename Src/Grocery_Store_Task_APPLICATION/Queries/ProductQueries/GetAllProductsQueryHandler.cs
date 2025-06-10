using Grocery_Store_Task_CORE.DTOs.ProductDTOs;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Exceptions;
using MediatR;

namespace Grocery_Store_Task_CORE.Queries.ProductQueries
{
    public class GetAllProductsQueryHandler(IGetAllProductService getAllProductService) : IRequestHandler<GetAllProductsQuery, IEnumerable<GetProductDTO>>
    {
        public async Task<IEnumerable<GetProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<GetProductDTO> allProducts = await getAllProductService.GetAllProductsAsync();
                if (allProducts == null)
                {
                    throw new NotFoundException("Error Fetching Products");
                }
                return allProducts;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Fetching Products", ex);
            }
        }
    }
}
