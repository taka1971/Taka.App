using FluentValidation.TestHelper;
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Validations.Dtos;
using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentications.Tests.Validations
{
    public class UserRegisterDtoValidatorTests
    {
        private readonly UserRegisterDtoValidator _validator;

        public UserRegisterDtoValidatorTests()
        {
            _validator = new UserRegisterDtoValidator();
        }

        [Theory]
        [InlineData("test@test.com", true)]
        [InlineData("bademail", false)]
        public void Email_ShouldBeValidated(string email, bool expectedIsValid)
        {
            // Arrange
            var model = new UserRegisterRequest(email, "ValidPass123!", new List<Microservices> { Microservices.ServicesRental });

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            if (expectedIsValid)
            {
                result.ShouldNotHaveValidationErrorFor(m => m.Email);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(m => m.Email)
                      .WithErrorMessage("The provided email is not valid.");
            }
        }

        [Theory]
        [InlineData("password123", true)]
        [InlineData("pass", false)]
        [InlineData("thisiswaytoolongforapassword", false)]
        public void Password_ShouldHaveProperLength(string password, bool expectedIsValid)
        {
            // Arrange
            var model = new UserRegisterRequest("test@test.com", password, new List<Microservices> { Microservices.ServicesRental });

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            if (expectedIsValid)
            {
                result.ShouldNotHaveValidationErrorFor(m => m.Password);
            }
            else
            {
                if (password.Length < 8)
                {
                    result.ShouldHaveValidationErrorFor(m => m.Password)
                          .WithErrorMessage("The password must be at least 8 characters long.");
                }
                else if (password.Length > 16)
                {
                    result.ShouldHaveValidationErrorFor(m => m.Password)
                          .WithErrorMessage("The password must be no more than 16 characters long.");
                }
            }
        }

        [Fact]        
        public void AccessibleMicroservices_ShouldNotBeEmpty()
        {
            // Arrange
            var microServices = new List<Microservices>();
            microServices.Add(Microservices.ServicesRental);

            var model = new UserRegisterRequest("test@test.com", "ValidPass123!", microServices.ToList());

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            
            result.ShouldNotHaveValidationErrorFor(m => m.AccessibleMicroservices);            
        }

        [Fact]
        public void AccessibleMicroservices_ShouldBeEmpty()
        {
            // Arrange
            var microServices = new List<Microservices>();            

            var model = new UserRegisterRequest("test@test.com", "ValidPass123!", microServices.ToList());

            // Act
            var result = _validator.TestValidate(model);

            // Assert

            result.ShouldHaveValidationErrorFor(m => m.AccessibleMicroservices);
        }
    }
}