using FluentValidation;
using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using Grocery_Store_Task_CORE.Queries.DeliveryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Store_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ScheduleController(IMediator mediator, ILogger<ScheduleController> logger) : ControllerBase
    {
        /// <summary>
        /// Green Slots are Slots with less than 4 ordered carts
        /// </summary>
        /// <param name="getDeliveryTimeSlotsQuery"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeSlotDTO>>> GetTimeSlots([FromQuery] GetDeliveryTimeSlotsQuery getDeliveryTimeSlotsQuery, IValidator<GetDeliveryTimeSlotsQuery> validator)
        {

            var validationResult = validator.Validate(getDeliveryTimeSlotsQuery);
            if (validationResult.IsValid)
            {
                var timeSlots = await mediator.Send(getDeliveryTimeSlotsQuery);
                return Ok(timeSlots);
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(
                        key: error.PropertyName,
                        errorMessage: error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }




        }
    }
}
