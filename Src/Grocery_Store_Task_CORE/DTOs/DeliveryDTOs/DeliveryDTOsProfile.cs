using AutoMapper;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.DTOs.DeliveryDTOs
{
    public class DeliveryDTOsProfile : Profile
    {
        public DeliveryDTOsProfile()
        {
            CreateMap<TimeSlot, TimeSlotDTO>();
        }
    }
}
