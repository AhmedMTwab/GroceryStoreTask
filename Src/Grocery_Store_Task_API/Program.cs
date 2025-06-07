using Grocery_Store_Task_API.CutomMiddlewares;
using Grocery_Store_Task_CORE.ApplicationDIContainer;
using Grocery_Store_Task_INFRASTRUCTURE.InfrastructureDIContainer;
using Serilog;
namespace Grocery_Store_Task_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //Add Infrastructure Services
            builder.Services.AddInfrastructureServices(builder.Configuration);
            //Add Applicaiton Services
            builder.Services.AddApplicationServices(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
            {

                loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
            });
            //Add Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default",
                    opt =>
                    {
                        opt.WithOrigins(builder.Configuration.GetSection("AllowedHosts:Hosts").Get<string[]>())
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCustomErrorHandlingMiddleware();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseCors("default");
            app.MapControllers();

            app.Run();
        }
    }
}
