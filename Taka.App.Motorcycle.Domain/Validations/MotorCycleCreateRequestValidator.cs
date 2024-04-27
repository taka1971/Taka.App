using FluentValidation;
using Taka.App.Motor.Domain.Request;

namespace Taka.App.Motor.Domain.Validations
{
    public class MotorCycleCreateRequestValidator: AbstractValidator<MotorcycleCreateRequest>
    {
        public MotorCycleCreateRequestValidator()
        {
            RuleFor(x => x.Year).GreaterThan(2000).WithMessage("The Year must be greater than 2000."); 
            RuleFor(x => x.Model).NotEmpty().Length(10, 100).WithMessage("The model must be 1 to 100 characters long."); 
            RuleFor(x => x.Plate).NotEmpty().Matches("^[A-Z]{3}-[0-9]{4}$").WithMessage("Plate format is invalid."); 
        }
    }
}
