using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Grocery_Store_Task_INFRASTRUCTURE.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Store_Task_INFRASTRUCTURE.Repositories
{
    public class TimeSlotRepository(GroceryStoreDbContext db) : ITimeSlotRepository
    {
        public async Task<bool> AddTimeSlot(TimeSlot timeSlot)
        {
            
                db.TimeSlots.Add(timeSlot);
                await db.SaveChangesAsync();
                return true;
           
        }
        public async Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync()
        {
            
                var slots = await db.TimeSlots.ToListAsync();
                return slots;
           

        }
        public async Task<TimeSlot> GetTimeSlotByDateAsync(DateTime startDate)
        {
            
                var timeslot = await db.TimeSlots.FindAsync(startDate);
                return timeslot;
           

        }

    }
}
