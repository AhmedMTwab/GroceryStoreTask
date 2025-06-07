using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery_Store_Task_CORE.ServicesAbstraction.ICartServics;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;

namespace Grocery_Store_Task_CORE.Services.CartServices
{
    public class GetAllCartsTimeSlotsService(ICartRepository cartRepository):IGetAllCartsTimeSLotsService
    {
        public async Task<IEnumerable<TimeSlot>> GetAllCartsTimeSlots()
        {
            try
            {
                var allTimeSlots = await cartRepository.GetAllCartsTimeSlots();
                return allTimeSlots;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Fetch all Carts Time Slots", ex);
            }
        }
    }
}
