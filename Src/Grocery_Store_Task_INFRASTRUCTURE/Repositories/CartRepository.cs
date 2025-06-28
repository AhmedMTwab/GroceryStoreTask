using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Grocery_Store_Task_INFRASTRUCTURE.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Store_Task_INFRASTRUCTURE.Repositories
{
    public class CartRepository(GroceryStoreDbContext db) : ICartRepository
    {
        public async Task AddCart(Cart cart)
        {

            db.Carts.Add(cart);
            await db.SaveChangesAsync();

        }
        public async Task<IEnumerable<TimeSlot>> GetAllCartsTimeSlots()
        {

            var TimeSlots = await db.Carts.Select(c => c.TimeSlot).ToListAsync();
            return TimeSlots;

        }
        public async Task<IEnumerable<Cart>> GetAllCarts()
        {
            var carts = await db.Carts.Include(c => c.TimeSlot).ToListAsync();
            return carts;

        }

        public async Task<IEnumerable<TimeSlot>> GetAllCartsMatchedTimeSlotsAsync(TimeSlot Time)
        {

            var TimeSlots = await db.Carts.Select(c => c.TimeSlot).Where(t => t.StartDate == Time.StartDate).ToListAsync();
            return TimeSlots;

        }
    }
}
