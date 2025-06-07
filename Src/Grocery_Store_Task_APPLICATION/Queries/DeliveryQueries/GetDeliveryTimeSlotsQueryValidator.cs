using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Grocery_Store_Task_CORE.Queries.DeliveryQueries;

namespace Grocery_Store_Task_APPLICATION.Queries.DeliveryQueries
{
    public class GetDeliveryTimeSlotsQueryValidator:AbstractValidator<GetDeliveryTimeSlotsQuery>
    {
        public GetDeliveryTimeSlotsQueryValidator()
        {
            RuleFor(q=>q.productIds).NotNull().NotEmpty().WithMessage("Products List Can't be null or Empty");

            RuleFor(q=>q.orderDate).NotNull().NotEmpty().WithMessage("Products List Can't be null or Empty").Must(date => date != DateTime.MinValue)
             .WithMessage("Order Date cannot be the default value.").Must(date => date.Day == DateTime.Today.Day) 
             .WithMessage("Order Date must be today."); 
        }
    }
}
