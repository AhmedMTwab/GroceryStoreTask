using FluentValidation;
using Grocery_Store_Task_CORE.Commands.CartCommands;
using Grocery_Store_Task_CORE.DTOs.CartDTOs;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Store_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(IMediator mediator, IGetRangeofProductByIdService getRangeofProductByIdService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddCart(AddCartCommand addCartCommand, IValidator<AddCartCommand> validator)
        {
            try
            {
            
                var validationResult = validator.Validate(addCartCommand);
                if (validationResult.IsValid)
                {
                    await mediator.Send(addCartCommand);
                    return Created();
                }
                return Problem(validationResult.Errors.ToString());
            }
            catch (Exception ex)
            {
                return Problem($"{ex.Message}");
            }

        }
    }
}
