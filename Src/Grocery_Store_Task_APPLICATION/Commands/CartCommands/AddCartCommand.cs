using Grocery_Store_Task_DOMAIN.Models;
using MediatR;

namespace Grocery_Store_Task_CORE.Commands.CartCommands
{
    public class AddCartCommand : IRequest
    {
        public DateTime StartDate { get; set; }
        public bool IsGreen { get; set; }

        public virtual IEnumerable<Guid> CartProductsIds { get; set; } = Enumerable.Empty<Guid>();

    }
}
