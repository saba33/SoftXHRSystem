using FluentValidation;
using HRSystem.Application.DTOs.Auth.Requsets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.Validation.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.PersonalNumber)
                .NotEmpty()
                .Length(11)
                .WithMessage("პირადი ნომერი უნდა იყოს 11 სიმბოლო");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("სახელი სავალდებულოა");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("გვარი სავალდებულოა");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("ელფოსტა არასწორია");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username სავალდებულოა");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("პაროლი მინიმუმ 6 სიმბოლო უნდა იყოს");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("პაროლები არ ემთხვევა");
        }
    }
}
