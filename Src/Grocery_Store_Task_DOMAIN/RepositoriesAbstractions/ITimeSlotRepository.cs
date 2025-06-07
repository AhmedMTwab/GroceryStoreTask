using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_DOMAIN.RepositoriesAbstractions
{
    public interface ITimeSlotRepository
    {
        public Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync();
        public Task<TimeSlot> GetTimeSlotByDateAsync(DateTime startDate);
        public Task<bool> AddTimeSlot(TimeSlot timeSlot);

    }
}
