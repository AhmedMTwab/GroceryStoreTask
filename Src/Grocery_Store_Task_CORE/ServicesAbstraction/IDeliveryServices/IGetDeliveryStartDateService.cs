using Grocery_Store_Task_DOMAIN.Enums;

namespace Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices
{
    public interface IGetDeliveryStartDateService
    {
        public DateTime GetStartDate(ProductTypeEnum minimumType, DateTime orderDate);
        public bool IsWeekDay(DateTime startDate, ProductTypeEnum productType);
        public DateTime ToValidWeekDay(DateTime startDate, ProductTypeEnum productType);
    }
}
