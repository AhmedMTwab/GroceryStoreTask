using Grocery_Store_Task_CORE.DTOs.ProductDTOs;
using MediatR;

namespace Grocery_Store_Task_CORE.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<GetProductDTO>>
    {
    }
}
