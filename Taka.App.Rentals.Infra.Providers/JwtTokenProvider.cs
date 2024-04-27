using Microsoft.AspNetCore.Http;

namespace Taka.App.Rentals.Infra.Providers
{
    public class JwtTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAccessToken()
        {
            // Pega o token do contexto HTTP atual
            return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(" ").Last();
        }
    }


}
