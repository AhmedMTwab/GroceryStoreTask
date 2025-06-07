namespace Grocery_Store_Task_API.CutomMiddlewares
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomErrorHandlingMiddleware> _logger;


        public CustomErrorHandlingMiddleware(RequestDelegate next, ILogger<CustomErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", ex.InnerException.GetType().ToString(), ex.Message);
                }
                else
                {
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", ex.GetType().ToString(), ex.Message);
                }
                httpContext.Response.StatusCode = 500;
                throw;

            }
        }
    }
    public static class CustomErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomErrorHandlingMiddleware>();
        }
    }
}

