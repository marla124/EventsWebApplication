using Microsoft.AspNetCore.Diagnostics;

namespace EventsWebApplication.Middleware
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandler(this
            IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
