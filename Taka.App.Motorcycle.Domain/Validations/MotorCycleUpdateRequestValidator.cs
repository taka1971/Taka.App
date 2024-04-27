using FluentValidation;
using Taka.App.Motor.Domain.Request;

namespace Taka.App.Motor.Domain.Validations
{
    public class MotorCycleUpdateRequestValidator: AbstractValidator<MotorcycleUpdateRequest>
    {
        public MotorCycleUpdateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("The Id cannot be null");
            RuleFor(x => x.Plate).NotEmpty().Matches("^[A-Z]{3}-[0-9]{4}$").WithMessage("Invalid plate.");
        }
    }
}
