using AutoMapper;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using MediatR;

namespace Grocery_Store_Task_CORE.Commands.CartCommands
{
    public class AddCartCommandHandler(ICartRepository cartRepository, ITimeSlotRepository timeSlotRepository, IMapper mapper, IGetRangeofProductByIdService getRangeofProductByIdService) : IRequestHandler<AddCartCommand>
    {
        public async Task Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newCart = mapper.Map<Cart>(request);
                var getProducts = await getRangeofProductByIdService.GetRangeofProductByIdAsync(request.CartProductsIds);
                if (getProducts != null)
                {
                    newCart.CartProducts = getProducts;
                }
                var timeSlots = await timeSlotRepository.GetTimeSlotByDateAsync(request.StartDate);
                if (timeSlots != null)
                {
                    timeSlots.IsGreen = newCart.TimeSlot.IsGreen;
                    newCart.TimeSlot = timeSlots;

                }
                newCart.Id = Guid.NewGuid();
                await cartRepository.AddCart(newCart);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Handling Add Cart Command", ex);
            }
        }
    }
}
