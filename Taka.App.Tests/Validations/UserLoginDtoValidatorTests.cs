using FluentValidation.TestHelper;
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Validations.Dtos;

namespace Taka.App.Authentications.Tests.Validations
{
    public class UserLoginDtoValidatorTests
    {
        private readonly UserLoginDtoValidator _validator;        

        public UserLoginDtoValidatorTests()
        {
            _validator = new UserLoginDtoValidator();            
        }

        [Fact]        
        public void Should_HaveErrorWhenEmailIs_Invalid()
        {
            //Arrange
            var userLoginDto = new UserLoginDto("test.test", "12345678");

            //Act
            var result = _validator.TestValidate(userLoginDto);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_HaveErrorWhenPasswordIsTooShort()
        {
            //Arrange
            var userLoginDto = new UserLoginDto("test.test", "1234");

            //Act
            var result = _validator.TestValidate(userLoginDto);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_HaveErrorWhenPasswordIsTooLong()
        {
            //Arrange
            var userLoginDto = new UserLoginDto("test.test", new string('a', 17));
            
            //Act
            var result = _validator.TestValidate(userLoginDto);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_NotHaveErrorWhenInputIsValid()
        {
            //Arrange
            var userLoginDto = new UserLoginDto("test@test.com", "12345678");

            var result = _validator.TestValidate(userLoginDto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}


