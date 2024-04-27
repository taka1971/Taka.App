using FluentValidation;
using Taka.App.Rentals.Domain.Requests;

namespace Taka.App.Rentals.Domain.Validations
{
    public class UpdateRentalEndDateRequestValidator : AbstractValidator<CompleteRentalRequest>
    {
        public UpdateRentalEndDateRequestValidator()
        {
            RuleFor(x => x.RentalId).NotEmpty().WithMessage("Rental is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(BeAValidDate).WithMessage("Invalid end date.")
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("End date cannot be in the past.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }

}
