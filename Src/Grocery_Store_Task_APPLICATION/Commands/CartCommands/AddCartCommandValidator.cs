using FluentValidation;
using Grocery_Store_Task_CORE.Commands.CartCommands;

namespace Grocery_Store_Task_CORE.DTOs.CartDTOs
{
    public class AddCartCommandValidator : AbstractValidator<AddCartCommand>
    {
        public AddCartCommandValidator()
        {
            RuleFor(c => c.StartDate).NotNull().WithMessage("StartDate cant be Null").NotEmpty().WithMessage("StartDate cant be Empty");
            RuleFor(c => c.CartProductsIds).NotNull().WithMessage("Cart Products cant be Null").NotEmpty().WithMessage("Cart Products cant be Empty");
        }
    }
}
