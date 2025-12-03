using FluentValidation;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Domain.Enums;

namespace HRSystem.Application.Validation.Employees
{
    public class BaseEmployeeRequestValidator<T> : AbstractValidator<T>
        where T : BaseEmployeeRequest
    {
        public BaseEmployeeRequestValidator()
        {
            RuleFor(x => x.PersonalNumber)
                .NotEmpty().WithMessage("PersonalNumber is required")
                .Length(11).WithMessage("PersonalNumber must be 11 digits")
                .Matches("^[0-9]+$").WithMessage("PersonalNumber must contain only digits");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MaximumLength(50);

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid Gender value")
                .NotEmpty().WithMessage("Gender is required");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .LessThan(DateTime.UtcNow)
                .WithMessage("BirthDate cannot be in the future");

            RuleFor(x => x.PositionId)
                .GreaterThan(0)
                .WithMessage("PositionId is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid Status value")
                .NotEmpty().WithMessage("Status is required");

            RuleFor(x => x.FiredDate)
                .Must((req, firedDate) =>
                {
                    if (req.Status == EmployeeStatus.Terminated && firedDate == null)
                        return false;

                    return true;
                })
                .WithMessage("FiredDate is required when Status = Terminated");
        }
    }
}