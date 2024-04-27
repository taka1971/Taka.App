using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;
using Newtonsoft.Json;
using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentication.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateJwtToken(User user)
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
            return tokenHandler.WriteToken(token);
        }
    }
}
