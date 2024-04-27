using NSubstitute;
using Microsoft.Extensions.Options;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using FluentAssertions;
using Taka.App.Authentication.Domain.Enums;
using System.Security.Cryptography;

namespace Taka.App.Authentications.Tests.Validations
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly IOptions<JwtSettings> _jwtSettings = Substitute.For<IOptions<JwtSettings>>();

        public TokenServiceTests()
        {            
            var jwtSettings = new JwtSettings
            {
                Secret = GenerateSecureKey(), 
                Issuer = "Issuer",
                Audiences = new List<string> { "Audience" },
                ExpiresInMinutes = 60
            };

            _jwtSettings.Value.Returns(jwtSettings);
            _tokenService = new TokenService(_jwtSettings);
        }

        [Fact]
        public void GenerateJwtToken_ShouldContainCorrectClaims_WhenGivenValidUser()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                Email = "user@test.com",
                Roles = new List<RolesTypes> { RolesTypes.Admin },
                AccessibleMicroservices = new List<Microservices> { Microservices.AllServices },
                Active = true
            };

            // Act
            var token = _tokenService.GenerateJwtToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.Claims.Should().ContainSingle(claim => claim.Type == "nameid" && claim.Value == user.Id);
            jwtToken.Claims.Should().ContainSingle(claim => claim.Type == "email" && claim.Value == user.Email);
            jwtToken.Claims.Should().ContainSingle(claim => claim.Type == "Roles" && claim.Value == JsonConvert.SerializeObject(user.Roles));
            jwtToken.Claims.Should().ContainSingle(claim => claim.Type == "AccessibleMicroservices" && claim.Value == JsonConvert.SerializeObject(user.AccessibleMicroservices));
            jwtToken.Claims.Should().ContainSingle(claim => claim.Type == "Active" && claim.Value == JsonConvert.SerializeObject(user.Active));
            jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
            jwtToken.Issuer.Should().Be(_jwtSettings.Value.Issuer);
        }

        private string GenerateSecureKey()
        {
            using (var random = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[32]; 
                random.GetBytes(key);
                return Convert.ToBase64String(key); 
            }
        }
    }
}