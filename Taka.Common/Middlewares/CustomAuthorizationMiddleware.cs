using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Taka.Common.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Status = 403,
                    Message = "Access Denied. You do not have permission to access this resource."
                }));
            }
        }
    }
}
