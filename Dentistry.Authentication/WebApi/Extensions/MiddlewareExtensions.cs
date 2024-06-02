using WebApi.Middleware;

namespace WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseApplicationMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
