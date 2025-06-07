using Grocery_Store_Task_CORE.DTOs.ProductDTOs;
using Grocery_Store_Task_CORE.Queries.ProductQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Store_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetProductDTO>> GetAllProducts()
        {
            try
            {
                var products = await mediator.Send(new GetAllProductsQuery());
                if (products != null)
                {
                    return Ok(products);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem($"{ex.Message}");
            }

        }
    }
}
