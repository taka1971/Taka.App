using FluentValidation.TestHelper;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Validations;

namespace Taka.App.Motor.Tests.Domain
{
    public class MotorCycleUpdateRequestValidatorTests
    {
        private readonly MotorCycleUpdateRequestValidator _validator;

        public MotorCycleUpdateRequestValidatorTests()
        {
            _validator = new MotorCycleUpdateRequestValidator();
        }

        [Fact]
        public void Should_HaveErrorWhenIdIsEmpty()
        {
            var model = new MotorcycleUpdateRequest(Guid.Empty, "ABC-1234");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Id)
                .WithErrorMessage("The Id cannot be null");
        }

        [Fact]
        public void Should_HaveErrorWhenPlateIsEmpty()
        {
            var model = new MotorcycleUpdateRequest(Guid.NewGuid(), "");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Plate)
                .WithErrorMessage("Invalid plate.");
        }

        [Fact]
        public void Should_HaveErrorWhenPlateFormatIsInvalid()
        {
            var model = new MotorcycleUpdateRequest(Guid.NewGuid(), "ABCD-123");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(motorcycle => motorcycle.Plate)
                .WithErrorMessage("Invalid plate.");
        }

        [Fact]
        public void Should_NotHaveErrorWhenAllFieldsAreValid()
        {
            var model = new MotorcycleUpdateRequest(Guid.NewGuid(), "ABC-1234");
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
