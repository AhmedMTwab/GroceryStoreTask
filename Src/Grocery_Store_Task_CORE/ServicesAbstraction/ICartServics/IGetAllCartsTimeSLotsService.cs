using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.ICartServics
{
    public interface IGetAllCartsTimeSLotsService
    {
        public Task<IEnumerable<TimeSlot>> GetAllCartsTimeSlots();

    }
}