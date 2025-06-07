using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Grocery_Store_Task_INFRASTRUCTURE.ApplicationContext;
using Grocery_Store_Task_INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grocery_Store_Task_INFRASTRUCTURE.InfrastructureDIContainer
{
    public static class InfrastructureDIContainer
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GroceryStoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("GroceryStoreDb")));
            //Repositories Registration
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

    }
}
