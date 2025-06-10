using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;

namespace Grocery_Store_Task_CORE.Services.TimeSlotServices
{
    public class GetGreenSlotsAsRequiredService(ICartRepository cartRepository) : IGetGreenSlotsService
    {
        public Task<bool> IsGreenSlot(TimeSlot slot, IEnumerable<TimeSlot> allTimeSLots)
        {

            if ((slot.StartDate.Hour > 13 && slot.StartDate.Hour < 15) || (slot.StartDate.Hour > 20 && slot.StartDate.Hour < 22))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);



        }
    }
}
