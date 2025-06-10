using Grocery_Store_Task_CORE.ServicesAbstraction.ICartServics;
using Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.Services.TimeSlotServices
{
    public class GenerateTimeslotsService(IGetDeliveryStartDateService getDeliveryStartDate, IGetGreenSlotsService getGreenSlotsService, IGetAllCartsTimeSLotsService getAllCartsTimeSLotsService) : IGenerateTimeSlotsService
    {
        public async Task<IEnumerable<TimeSlot>> GenerateSlotsFromStartDate(DateTime startdate, DateTime orderDate, int numberofDays, ProductTypeEnum productType)
        {
            List<TimeSlot> slots = new List<TimeSlot>();
            var allCartsTimeSlots = await getAllCartsTimeSLotsService.GetAllCartsTimeSlots();

            DateTime newDay = startdate.Date;
            DateTime lastValidDay = orderDate.AddDays(numberofDays - 1);
            for (DateTime start = newDay; start <= lastValidDay; start = start.AddDays(1))
            {

                if (start.Day == orderDate.Day && orderDate.Hour >= 8 && orderDate.Hour <= 22)
                {
                    DateTime sameDaySlotDate = startdate;
                    for (int i = startdate.Hour; i < 21; i++)
                    {
                        sameDaySlotDate = sameDaySlotDate.AddHours(1);
                        TimeSlot sameDaySlot = new TimeSlot();
                        sameDaySlot.StartDate = new DateTime(sameDaySlotDate.Year, sameDaySlotDate.Month, sameDaySlotDate.Day, sameDaySlotDate.Hour, 0, 0);
                        sameDaySlot.IsGreen = await getGreenSlotsService.IsGreenSlot(sameDaySlot, allCartsTimeSlots);
                        slots.Add(sameDaySlot);
                    }
                    continue;
                }
                if (!getDeliveryStartDate.IsWeekDay(start, productType))
                    continue;
                for (int time = 8; time < 22; time++)
                {
                    DateTime nextdayHour = start.AddHours(time);
                    TimeSlot newTimeSlot = new TimeSlot
                    {
                        StartDate = nextdayHour,
                    };
                    newTimeSlot.IsGreen = await getGreenSlotsService.IsGreenSlot(newTimeSlot, allCartsTimeSlots);
                    slots.Add(newTimeSlot);

                }

            }
            var orderdSlots = slots.OrderBy(s => s.StartDate.Date).ThenByDescending(s => s.IsGreen);
            return orderdSlots;

        }


    }
}
