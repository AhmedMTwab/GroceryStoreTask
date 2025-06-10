using Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices;
using Grocery_Store_Task_DOMAIN.Enums;

namespace Grocery_Store_Task_CORE.Services.DeliveryServices
{
    public class GetDeliveryStartDateService() : IGetDeliveryStartDateService
    {
        private const int FreshFoodSameDayDelveryLimit = 12;
        private const int InStockSameDayDiliveryLimit = 18;
        private const int ExternalProductLeadDays = 3;
        private const int DailyDeliveryStartHour = 8;
        public DateTime GetStartDate(ProductTypeEnum maximumType, DateTime orderDate)
        {

            DateTime startDate = orderDate;
            if ((maximumType == ProductTypeEnum.InStock && orderDate.Hour < InStockSameDayDiliveryLimit) || (maximumType == ProductTypeEnum.FreshFood && orderDate.Hour < FreshFoodSameDayDelveryLimit))
            {
                startDate = orderDate;
            }
            else if (maximumType == ProductTypeEnum.ExternalProduct)
            {
                startDate = startDate.Date.AddDays(ExternalProductLeadDays).AddHours(DailyDeliveryStartHour);
            }
            else
            {
                startDate = startDate.Date.AddDays(1).AddHours(DailyDeliveryStartHour);
            }
            if (!IsWeekDay(startDate, maximumType))
                return ToValidWeekDay(startDate, maximumType);
            return startDate;


        }
        public bool IsWeekDay(DateTime startDate, ProductTypeEnum productType)
        {
            if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday || (productType == ProductTypeEnum.ExternalProduct && startDate.DayOfWeek == DayOfWeek.Monday))
            {
                return false;
            }
            return true;
        }
        public DateTime ToValidWeekDay(DateTime startDate, ProductTypeEnum productType)
        {

            if (productType != ProductTypeEnum.ExternalProduct)
            {
                switch (startDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        {
                            startDate = startDate.AddDays(2);
                            break;
                        }
                    case DayOfWeek.Sunday:
                        {
                            startDate = startDate.AddDays(1);
                            break;
                        }
                }
            }
            else
            {

                switch (startDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        {
                            startDate = startDate.AddDays(3);
                            break;
                        }
                    case DayOfWeek.Sunday:
                        {
                            startDate = startDate.AddDays(2);
                            break;
                        }
                    case DayOfWeek.Monday:
                        {
                            startDate = startDate.AddDays(1);
                            break;
                        }
                }

            }

            return startDate.Date.AddHours(DailyDeliveryStartHour);
        }
    }
}
