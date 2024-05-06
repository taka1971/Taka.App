using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Responses;

namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface ITokenService
    {
        TokenResponse GenerateJwtToken(User user);
        Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken);
        Task<TokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
