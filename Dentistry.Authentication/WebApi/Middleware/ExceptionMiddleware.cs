using System.Net;
using System.Text.Json;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
            }.ToString());
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
