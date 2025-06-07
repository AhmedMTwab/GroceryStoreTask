using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using Grocery_Store_Task_CORE.Queries.DeliveryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Store_Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeSlotDTO>>> GetTimeSlots([FromQuery] GetDeliveryTimeSlotsQuery getDeliveryTimeSlotsQuery,IValidator<GetDeliveryTimeSlotsQuery> validator)
        {
            try
            {
                    var validationResult = validator.Validate(getDeliveryTimeSlotsQuery);
                    if (validationResult.IsValid)
                    {
                        var timeSlots = await mediator.Send(getDeliveryTimeSlotsQuery);
                        return Ok(timeSlots);
                    }
                    else
                    {
                        return Problem(detail: validationResult.Errors.ToString(), statusCode: 400);
                    }
               
            }
            catch (Exception ex)
            {
               return Problem($"{ex.Message}");
            }

        }
    }
}
