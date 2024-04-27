using System.Net.Http.Headers;
using Taka.App.Rentals.Infra.Providers;

namespace Taka.App.Rentals.Application.Handlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly JwtTokenProvider _tokenProvider;

        public AuthenticationDelegatingHandler(JwtTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenProvider.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
