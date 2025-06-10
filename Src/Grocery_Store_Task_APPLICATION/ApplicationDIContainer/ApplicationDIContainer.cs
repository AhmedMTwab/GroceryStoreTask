using FluentValidation;
using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using Grocery_Store_Task_CORE.Services.CartServices;
using Grocery_Store_Task_CORE.Services.DeliveryServices;
using Grocery_Store_Task_CORE.Services.ProductServices;
using Grocery_Store_Task_CORE.Services.TimeSlotServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.ICartServics;
using Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grocery_Store_Task_CORE.ApplicationDIContainer
{
    public static class ApplicationDIContainer
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(ApplicationDIContainer).Assembly);
            services.AddAutoMapper(typeof(DeliveryDTOsProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationDIContainer).Assembly);
            services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(ApplicationDIContainer).Assembly));
            //Delivery services Registeration
            services.AddScoped<IGetDeliveryStartDateService, GetDeliveryStartDateService>();
            //Product services Registeration
            services.AddScoped<IGetAllProductService, GetAllProductService>();
            services.AddScoped<IGetMaximumDeliveryTypeService, GetMaximumDeliveryTypeService>();
            services.AddScoped<IGetProductByIdService, GetProductByIdService>();
            services.AddScoped<IGetRangeofProductByIdService, GetProductByIdService>();
            //TimeSlot services Registeration
            services.AddScoped<IGenerateTimeSlotsService, GenerateTimeslotsService>();
            //Cart services Registeration
            services.AddScoped<IGetAllCartsTimeSLotsService, GetAllCartsTimeSlotsService>();

            //Two Ways To get Green Slots
            var activeService = configuration.GetSection("GreenSlotServiceConfiguration:ActiveService").Value;
            switch (activeService)
            {
                case "GetGreenSlotsService":
                    {
                        services.AddScoped<IGetGreenSlotsService, GetGreenSlotsService>();
                        break;
                    }
                case "GetGreenSlotsAsRequiredService":
                    {
                        services.AddScoped<IGetGreenSlotsService, GetGreenSlotsAsRequiredService>();
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Invalid 'ActiveService' configured.");
                    }
            }




        }
    }
}
