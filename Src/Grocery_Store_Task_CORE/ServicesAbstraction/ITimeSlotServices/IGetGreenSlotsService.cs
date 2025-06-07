using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices
{
    public interface IGetGreenSlotsService
    {
        public Task<bool> IsGreenSlot(TimeSlot slot, IEnumerable<TimeSlot> allTimeSLots);
    }
}
