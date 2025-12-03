using FluentValidation;
using HRSystem.Application.DTOs.Auth.Requsets;

namespace HRSystem.Application.Validation.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username სავალდებულოა");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("პაროლი სავალდებულოა");
        }
    }
}
