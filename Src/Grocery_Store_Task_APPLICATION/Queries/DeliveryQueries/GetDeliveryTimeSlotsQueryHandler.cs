using AutoMapper;
using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using Grocery_Store_Task_CORE.Services.DeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Exceptions;
using Grocery_Store_Task_DOMAIN.Models;
using MediatR;

namespace Grocery_Store_Task_CORE.Queries.DeliveryQueries
{
    public class GetDeliveryTimeSlotsQueryHandler(IGetRangeofProductByIdService productsByIdServices
        , IGetMaximumDeliveryTypeService getMaximumDeliveryType, IGetDeliveryStartDateService getDeliveryStartDate,
        IGenerateTimeSlotsService generateTimeSlots, IMapper mapper) : IRequestHandler<GetDeliveryTimeSlotsQuery, IEnumerable<TimeSlotDTO>>
    {
        public async Task<IEnumerable<TimeSlotDTO>> Handle(GetDeliveryTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Product> productsList = await productsByIdServices.GetRangeofProductByIdAsync(request.productIds);

                ProductTypeEnum listMinimumDeliveryType = getMaximumDeliveryType.GetOrderMaximumDeliveryType(productsList);
                DateTime firstTimeSlotDate = getDeliveryStartDate.GetStartDate(listMinimumDeliveryType, request.orderDate);
                IEnumerable<TimeSlot> generatedTimeSlots = await generateTimeSlots.GenerateSlotsFromStartDate(firstTimeSlotDate, request.orderDate, 14, listMinimumDeliveryType);
                if (generatedTimeSlots == null)
                {
                    throw new NotFoundException("Didn't Generate  Time Slots ");
                }
                IEnumerable<TimeSlotDTO> schedule_Deliveries = mapper.Map<IEnumerable<TimeSlotDTO>>(generatedTimeSlots);
                return schedule_Deliveries;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Fetching Time Slots", ex);
            }
        }

    }
}
