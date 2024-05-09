using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taka.App.Authentication.Domain.Exceptions;

namespace Taka.Common.Middlewares
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
            context.Response.ContentType = "application/json";

            try
            {
                await _next(context);
            }
            catch (UserFailValidationException ex)
            {
                Log.Warning(ex, "Error. Fail validation. {Message}", ex.Message);                

                var response = ErrorResponse(ex.Message, context);

                await context.Response.WriteAsync(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when trying to process the request. {Message}", ex.Message);
                
                var response = ErrorResponse(ex.Message, context);
                
                await context.Response.WriteAsync(response);
            }
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
