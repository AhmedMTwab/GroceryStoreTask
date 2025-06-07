using AutoMapper;
using Grocery_Store_Task_CORE.Commands.CartCommands;
using Grocery_Store_Task_DOMAIN.Models;

namespace Grocery_Store_Task_CORE.DTOs.CartDTOs
{
    public class CartProfiles : Profile
    {
        public CartProfiles()
        {

            CreateMap<AddCartCommand, Cart>()
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => new TimeSlot
                {
                    StartDate = src.StartDate,
                    IsGreen = src.IsGreen
                }));
        }
    }
}
