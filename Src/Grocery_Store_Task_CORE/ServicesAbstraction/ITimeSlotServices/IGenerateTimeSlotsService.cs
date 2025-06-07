using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices
{
    public interface IGenerateTimeSlotsService
    {
        public Task<IEnumerable<TimeSlot>> GenerateSlotsFromStartDate(DateTime startdate, DateTime orderDate, int numberofDays, ProductTypeEnum productType);
    }
}
