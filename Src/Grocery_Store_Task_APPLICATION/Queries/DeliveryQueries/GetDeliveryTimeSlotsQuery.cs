using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using MediatR;

namespace Grocery_Store_Task_CORE.Queries.DeliveryQueries
{
    public class GetDeliveryTimeSlotsQuery : IRequest<IEnumerable<TimeSlotDTO>>
    {
        public IEnumerable<Guid> productIds { get; set; } = default!;

        public DateTime orderDate { get; set; }
    }
}
