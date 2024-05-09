using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Taka.Common
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult SuccessResponse<T>(string message, T result)
        {
            var response = new
            {
                StatusCode = 200,
                Message = message,
                Path = Request.Path.Value,
                Result = JsonConvert.SerializeObject(result, Formatting.Indented)
            };

            return Ok(response);
        }
    }
}
