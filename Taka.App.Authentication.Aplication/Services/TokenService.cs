using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;
using Newtonsoft.Json;
using Taka.App.Authentication.Domain.Responses;
using Taka.App.Authentication.Domain.Dtos;

namespace Taka.App.Authentication.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public TokenService(IOptions<JwtSettings> jwtSettings, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }

        public TokenResponse GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Roles", JsonConvert.SerializeObject(user.Roles)),
                new Claim("AccessibleMicroservices", JsonConvert.SerializeObject(user.AccessibleMicroservices)),
                new Claim("Active", JsonConvert.SerializeObject(user.Active))

                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var audience in _jwtSettings.Audiences)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString().Replace("-", ""),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresInDays),
                Created = DateTime.UtcNow,
                UserEmail = user.Email,
            };

            _refreshTokenRepository.CreateRefreshTokenAsync(refreshToken);
            
            return new TokenResponse { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };
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
            if (refreshToken is not null)
            {
                await RevokeRefreshTokenAsync(refreshToken);
            }
            var user = await _userRepository.GetUserByEmailAsync(request.UserEmail);

            return GenerateJwtToken(user);

        }

        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);
        }
    } 
}
