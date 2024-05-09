using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Rentals.Domain.Enums;

namespace Taka.App.Rentals.Crosscutting.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException dex)
            {
                Log.Warning(dex, "Domain exception occurred: {Message}", dex.Message);
                await HandleDomainException(dex, context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleGenericException(context, ex.Message);
            }
        }

        private async Task HandleDomainException(DomainException dex, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string response = string.Empty;

            switch (dex.ErrorCode)
            {
                case DomainErrorCode.RentalNotFound:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = ErrorResponse(dex.Message, context);
                    break;
                case DomainErrorCode.RentalPlanNotFound:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = ErrorResponse(dex.Message, context);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ErrorResponse(dex.Message, context);
                    break;
            }

            await context.Response.WriteAsync(response);
        }

        private async Task HandleGenericException(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = ErrorResponse(message, context);
            await context.Response.WriteAsync(response);
        }

        private string ErrorResponse(string message, HttpContext context)
        {
            return JsonConvert.SerializeObject(new
            {
                Error = true,
                StatusCode = context.Response.StatusCode,
                Message = message,
                Path = context.Request.Path
            });
        }
    }
}


