using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.Services.DeliveryServices
{
    public interface IGetMaximumDeliveryTypeService
    {
        public ProductTypeEnum GetOrderMaximumDeliveryType(IEnumerable<Product> deliveryProductDTOs);
    }
}
