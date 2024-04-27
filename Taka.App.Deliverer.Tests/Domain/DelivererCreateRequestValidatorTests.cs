using FluentValidation.TestHelper;
using Taka.App.Deliverer.Domain.Requests;
using Taka.App.Deliverer.Domain.Validations;
using Taka.Common.Middlewares.Enums;

namespace Taka.App.Deliverer.Tests.Domain
{
    public class DelivererCreateRequestValidatorTests
    {
        private readonly DelivererCreateRequestValidator _validator;

        public DelivererCreateRequestValidatorTests()
        {
            _validator = new DelivererCreateRequestValidator();
        }

        [Fact]
        public async Task Name_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new DelivererCreateRequest("short", "32874114000157", "123456", (CnhValidType)1, DateOnly.FromDateTime(DateTime.Now.AddYears(25)), "123456789");

            // Act

            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
                
        }        

        [Fact]
        public async Task Cnpj_WhenNotExactLength_ShouldHaveValidationError()
        {
            // Arrange
            var model = new DelivererCreateRequest("Test max name", "328741140001", "123456", (CnhValidType)1, DateOnly.FromDateTime(DateTime.Now.AddYears(25)), "123456789");

            // Act
            var result = await _validator.TestValidateAsync(model);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Cnpj);
        }

        [Fact]
        public async Task Cnh_WhenOutOfRange_ShouldHaveValidationError()
        {
            // Arrange
            var model = new DelivererCreateRequest("Test max name", "32874114000157", "1234", (CnhValidType)1, DateOnly.FromDateTime(DateTime.Now.AddYears(25)), "123456789");

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert

            result.ShouldHaveValidationErrorFor(x => x.Cnh);
        }        

        [Fact]
        public async Task BirthDate_WhenTooYoung_ShouldHaveValidationError()
        {
            // Arrange            
            var model = new DelivererCreateRequest("Test max name", "32874114000157", "123456", (CnhValidType)1, DateOnly.FromDateTime(DateTime.Now.AddYears(-17)), "123456789");

            // Act
            var result = await _validator.TestValidateAsync(model);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);  
        }

        [Fact]
        public async Task BirthDate_WhenInFuture_ShouldHaveValidationError()
        {
            // Arrange            

            var model = new DelivererCreateRequest("Test max name", "32874114000157", "123456", (CnhValidType)1,
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "151151515151515");

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);                      
        }

        [Fact]
        public async Task CnhImage_WhenNotPng_ShouldHaveValidationError()
        {
            // Arrange
            var model = new DelivererCreateRequest("Test max name", "32874114000157", "123456", (CnhValidType)1, DateOnly.FromDateTime(DateTime.Now.AddYears(25)), "");

            // Act
            var result = await _validator.TestValidateAsync(model);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CnhImage);                      
        }
    }

}
