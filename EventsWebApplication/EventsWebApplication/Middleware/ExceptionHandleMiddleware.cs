using System.Net;
using FluentValidation;
using System.Text.Json;

namespace EventsWebApplication.Middleware
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case KeyNotFoundException keyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(keyNotFoundException.Data);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(unauthorizedAccessException.Data);
                    break;
                case ArgumentException argumentException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(argumentException.Data);
                    break;
                case InvalidOperationException invalidOperationException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(invalidOperationException.Data);
                    break;

            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { err0r = exception.Message });
            }
            return context.Response.WriteAsync(result);
        }
    }
}
