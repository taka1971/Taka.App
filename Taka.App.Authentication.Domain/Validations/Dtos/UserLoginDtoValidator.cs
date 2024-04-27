using FluentValidation;
using Taka.App.Authentication.Domain.Dtos;

namespace Taka.App.Authentication.Domain.Validations.Dtos
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("The provided email is not valid.");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
                .MaximumLength(16).WithMessage("The password must be no more than 16 characters long.");            
        }
    }
}
