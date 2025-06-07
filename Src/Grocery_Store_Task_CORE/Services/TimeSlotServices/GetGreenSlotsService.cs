using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;

namespace Grocery_Store_Task_CORE.Services.TimeSlotServices
{
    public class GetGreenSlotsService(ICartRepository cartRepository) : IGetGreenSlotsService
    {
        public async Task<bool> IsGreenSlot(TimeSlot slot,IEnumerable<TimeSlot> allTimeSLots)
        {
            try
            {
                var matchedTimeSlots= allTimeSLots.Where(t=>t.StartDate==slot.StartDate).ToList();
                if (matchedTimeSlots.Any(s => s.IsGreen == false) || matchedTimeSlots.Count() >= 4)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Fetching Matched Time SLots", ex);
            }
          
            
        }
    }
}
