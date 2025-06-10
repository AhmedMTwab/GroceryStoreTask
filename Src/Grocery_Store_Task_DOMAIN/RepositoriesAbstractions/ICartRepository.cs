using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_DOMAIN.RepositoriesAbstractions
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetAllCarts();
        public Task<IEnumerable<TimeSlot>> GetAllCartsTimeSlots();

        public Task<IEnumerable<TimeSlot>> GetAllCartsMatchedTimeSlotsAsync(TimeSlot Time);

        public Task AddCart(Cart cart);
    }
}
