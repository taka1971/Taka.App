using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Taka.Common
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ApiResponse<T>(T result)
        {
            var resultName = result.GetType().Name;            

            if (result is null)
            {
                Log.Information($"Not Found. {resultName}");

                var notFoundResponse = new
                {
                    StatusCode = 404,
                    Message = $"Not Found. {resultName}",
                    Path = Request.Path.Value                    
                };
                return NotFound(notFoundResponse);
            }

            Log.Information($"Success. {resultName}");

            var response = new
            {
                StatusCode = 200,
                Message = $"Success. {resultName}",
                Path = Request.Path.Value,
                Result = JsonConvert.SerializeObject(result)
            };

            return Ok(response);
        }

        protected IActionResult ApiResponse<T>(string message, T result)
        {
            Log.Information(message);

            var response = new
            {
                StatusCode = 200,
                Message = message,
                Path = Request.Path.Value,
                Result = JsonConvert.SerializeObject(result, Formatting.Indented)
            };

            return Ok(response);
        }

        protected IActionResult ApiResponse(string message)
        {
            Log.Information(message);

            var response = new
            {
                StatusCode = 202,
                Message = message,
                Path = Request.Path.Value                
            };

            return Accepted(response);
        }
    }
}
