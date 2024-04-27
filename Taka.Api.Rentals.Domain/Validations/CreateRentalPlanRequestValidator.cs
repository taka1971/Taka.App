using FluentValidation;
using Taka.App.Rentals.Domain.Requests;

namespace Taka.App.Rentals.Domain.Validations
{
    public class CreateRentalPlanRequestValidator : AbstractValidator<CreateRentalPlanRequest>
    {
        public CreateRentalPlanRequestValidator()
        {
            RuleFor(x => x.RentalDescription)
            .NotEmpty()
            .MinimumLength(10)
            .Must((x, description) => description.Contains(x.DurationDays.ToString()))
            .WithMessage("The rental description must include the duration and be at least 10 characters long.");

            RuleFor(x => x.DurationDays)
                .Must(duration => new uint[] { 7, 15, 30, 45, 50 }.Contains(duration))
                .WithMessage("Duration must be either 7, 15, 30, 45, or 50 days.");

            RuleFor(x => x.DailyRate)
                .Must((model, rate) => ValidateDailyRate(model.DurationDays, rate))
                .WithMessage("Daily rate is not correct for the chosen duration.");

            RuleFor(x => x.EarlyReturnPenaltyRate)
                .Must((model, rate) => ValidatePenaltyRate(model.DurationDays, rate))
                .WithMessage("Penalty rate is not correct for the chosen duration.");
        }

        private bool ValidateDailyRate(uint duration, decimal rate)
        {
            return (duration == 7 && rate == 30m) ||
                   (duration == 15 && rate == 28m) ||
                   (duration == 30 && rate == 22m) ||
                   (duration == 45 && rate == 20m) ||
                   (duration == 50 && rate == 18m);
        }

        private bool ValidatePenaltyRate(uint duration, decimal penaltyRate)
        {
            var baseRate = duration switch
            {
                7 =>  20,
                15 => 40,                
                _ => 0m
            };
            return penaltyRate == baseRate;
        }
    }

}
