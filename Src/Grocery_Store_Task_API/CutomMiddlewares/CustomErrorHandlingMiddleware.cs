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
                var errorType = ex.GetType().ToString();

                if (ex.InnerException != null)
                {
                    var message = ex.InnerException.Message;
                    errorType = ex.InnerException.GetType().ToString();
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", errorType, message);
                    await httpContext.Response.WriteAsync($"Error Occurd: {errorType} ErrorMessage:{message}");
                    if (errorType == "NotFoundException")
                    {
                        httpContext.Response.StatusCode = 404;
                    }
                }
                else
                {
                    var message = ex.Message;
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", errorType, message);
                    await httpContext.Response.WriteAsync($"Error Occurd: {errorType} ErrorMessage:{message}");

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

