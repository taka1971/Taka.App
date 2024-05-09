using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Taka.App.Motor.Domain.Exceptions;

namespace Taka.App.Motor.Infra.Security.Authorization
{
    public class MicroserviceAccessHandler : AuthorizationHandler<MicroserviceAccessRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MicroserviceAccessHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MicroserviceAccessRequirement requirement)
        {
            try
            {   
                var httpContext = _httpContextAccessor.HttpContext;

                string originService = httpContext.Request.Headers["X-Origin-Service"].ToString() ?? string.Empty;                

                bool isOriginValid = originService == "Taka.App.Rentals.Api";

                var accessibleMicroservicesJson = context.User.Claims.FirstOrDefault(c => c.Type == "AccessibleMicroservices")?.Value;



                if (accessibleMicroservicesJson != null)
                {
                    var accessibleMicroservices = JsonConvert.DeserializeObject<List<int>>(accessibleMicroservicesJson) 
                        ?? throw new AppException("Claim AccessibleMicroservices not found.");
                    if (accessibleMicroservices.Contains(0) || accessibleMicroservices.Contains(1) || isOriginValid)
                    {
                        context.Succeed(requirement);
                    }
                }
                
            }
            catch {
                context.Fail();              
            }

            return Task.CompletedTask;
        }
    }
}
