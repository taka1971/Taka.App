using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;
using Newtonsoft.Json;
using Taka.App.Authentication.Domain.Enums;
using Taka.App.Authentication.Domain.Responses;

namespace Taka.App.Authentication.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenService(IOptions<JwtSettings> jwtSettings, IRefreshTokenService refreshTokenService)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenService = refreshTokenService;
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

            _refreshTokenService.CreateRefreshTokenAsync(refreshToken);
            
            return new TokenResponse { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };
        }
    } 
}
