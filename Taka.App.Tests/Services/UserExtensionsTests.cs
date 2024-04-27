using FluentAssertions;
using Taka.App.Authentication.Application;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentications.Tests.Validations
{
    public class UserExtensionsTests
    {
        [Fact]
        public void ToDto_ShouldConvertUserToUserResponseDtoCorrectly()
        {
            // Arrange
            var user = new User()
            {
                Id = "1",
                Email = "test@test.com" ,
                PasswordHash = "",
                AccessibleMicroservices = new List<Microservices> { Microservices.AllServices }
            };                

            // Act
            var dto = UserExtensions.ToDto(user);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(user.Id);
            dto.Email.Should().Be(user.Email);
            dto.AccessibleMicroservices.Should().BeEquivalentTo(user.AccessibleMicroservices);
        }
    }
}
