
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;
using Taka.App.Authentication.Domain.Responses;

namespace Taka.App.Authentication.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, ITokenService tokenService)
        {
         
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            return await _refreshTokenRepository.CreateRefreshTokenAsync(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
           return await GetRefreshTokenAsync(token);
        }

        public async Task<TokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(request.Token);
            if( refreshToken is not null)
            {
                await RevokeRefreshTokenAsync(refreshToken);
            }
            var user = await _userRepository.GetUserByEmailAsync(request.UserEmail);

            return _tokenService.GenerateJwtToken(user);
            
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);
        }
    }
}
