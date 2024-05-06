using FluentValidation;
using Taka.App.Deliverer.Domain.Requests;

namespace Taka.App.Deliverer.Domain.Validations
{
    public class DelivererCreateRequestValidator : AbstractValidator<DelivererCreateRequest>
    {
        public DelivererCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(10)
                .MaximumLength(200)
                .WithMessage("The name must be between 10 and 200 characters.");

            RuleFor(x => x.Cnpj)
                .Length(14)
                .WithMessage("Invalid CNPJ.");

            RuleFor(x => x.Cnh)
                .Length(5, 20)
                .WithMessage("CNH must be between 5 and 20 characters.");

            RuleFor(x => x.CnhType)
                .IsInEnum()
                .WithMessage("Invalid CNH type.");

            RuleFor(x => x.BirthDate)
                .Must(BeAtLeast18)
                .WithMessage("The birth date must indicate an age of at least 18 years.")
                .Must(date => date < DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("The birth date cannot be in the future.");

            RuleFor(x => x.CnhImage)
            .Must(file => !string.IsNullOrEmpty(file) || !file.EndsWith(".png"))
            .WithMessage("The CNH image is not null or must be in PNG format.");
        }


        private bool BeAtLeast18(DateOnly birthDate)
        {
            int age = DateOnly.FromDateTime(DateTime.Now).Year - birthDate.Year;
            if (DateOnly.FromDateTime(DateTime.Now) < birthDate.AddYears(age))
                age--;
            return age >= 18;
        }
    }
}
