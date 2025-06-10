using Grocery_Store_Task_CORE.Services.DeliveryServices;
using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.Services.ProductServices
{
    public class GetMaximumDeliveryTypeService() : IGetMaximumDeliveryTypeService
    {

        public ProductTypeEnum GetOrderMaximumDeliveryType(IEnumerable<Product> deliveryProductDTOs)
        {

            List<ProductTypeEnum> productTypes = deliveryProductDTOs.Select(p => p.Category).ToList();
            if (productTypes.Contains(ProductTypeEnum.ExternalProduct))
            {
                return ProductTypeEnum.ExternalProduct;
            }
            else if (productTypes.Contains(ProductTypeEnum.InStock))
            {
                return ProductTypeEnum.InStock;
            }
            else
            {
                return ProductTypeEnum.FreshFood;
            }

        }
    }
}
