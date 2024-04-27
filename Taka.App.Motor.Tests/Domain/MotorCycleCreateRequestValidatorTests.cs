using FluentValidation.TestHelper;
using Taka.App.Motor.Domain.Validations;
using Taka.App.Motor.Domain.Request;

namespace Taka.App.Motor.Tests.Domain
{
    public class MotorCycleCreateRequestValidatorTests
    {
        private readonly MotorCycleCreateRequestValidator _validator;

        public MotorCycleCreateRequestValidatorTests()
        {
            _validator = new MotorCycleCreateRequestValidator();
        }

        [Fact]
        public void Should_HaveErrorWhenYearIsLessThan2001()
        {
            var model = new MotorcycleCreateRequest(2000, "ValidModel012", "ABC-1234");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Year)
                .WithErrorMessage("The Year must be greater than 2000.");
        }

        [Fact]
        public void Should_HaveErrorWhenModelIsTooShortOrTooLong()
        {
            var model = new MotorcycleCreateRequest(2021, "Short", "ABC-1234");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Model)
                .WithErrorMessage("The model must be 1 to 100 characters long.");

            model = new MotorcycleCreateRequest(2021, new string('A', 101), "ABC-1234");
            result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Model)
                .WithErrorMessage("The model must be 1 to 100 characters long.");
        }

        [Fact]
        public void Should_HaveErrorWhenPlateIsInInvalidFormat()
        {
            var model = new MotorcycleCreateRequest(2021, "ValidModel012", "123-ABCD");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Plate)
                .WithErrorMessage("Plate format is invalid.");
        }

        [Fact]
        public void Should_NotHaveErrorWhenAllFieldsAreValid()
        {
            var model = new MotorcycleCreateRequest(2021, "ValidModel012", "ABC-1234");
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
