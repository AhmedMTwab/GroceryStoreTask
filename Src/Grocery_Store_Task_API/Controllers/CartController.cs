using FluentValidation;
using Grocery_Store_Task_CORE.Commands.CartCommands;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
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

            var validationResult = validator.Validate(addCartCommand);
            if (validationResult.IsValid)
            {
                await mediator.Send(addCartCommand);
                return Created();
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(
                        key: error.PropertyName,
                        errorMessage: error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }
        }


    }
}
