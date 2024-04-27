using FluentValidation;
using Taka.App.Rentals.Domain.Requests;

namespace Taka.App.Rentals.Domain.Validations
{
    public class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
    {
        public CreateRentalRequestValidator()
        {
            RuleFor(x => x.RentalPlanId).NotEmpty().WithMessage("Rental plan is required.");
            RuleFor(x => x.MotorcycleId).NotEmpty().WithMessage("Motorcycle  is required.");
            RuleFor(x => x.DelivererId).NotEmpty().WithMessage("Deliverer is required.");
            RuleFor(x => x.PredictedEndDate)
                .Must((datePrev) => BeAtLeastTomorrow(datePrev))
                .WithMessage("Penalty rate is not correct for the chosen duration.");

        }    

        private bool BeAtLeastTomorrow(DateTime? date)
        {
            if (!date.HasValue)
                return true; 
            
            return date.Value.Date >= DateTime.UtcNow.AddDays(1).Date;
        }
    }
}
