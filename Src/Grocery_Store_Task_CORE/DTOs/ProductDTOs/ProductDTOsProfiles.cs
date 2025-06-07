using AutoMapper;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.DTOs.ProductDTOs
{
    public class ProductDTOsProfiles : Profile
    {
        public ProductDTOsProfiles()
        {
            CreateMap<Product, GetProductDTO>().ForMember(p => p.Category, opt => opt.MapFrom(p => p.Category.ToString()));
        }
    }
}
