using FluentValidation;
using HRSystem.Application.DTOs.Employees.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .NotEmpty().WithMessage("Gender is required")
                .Must(v => v == "M" || v == "F")
                .WithMessage("Gender must be 'M' or 'F'");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .LessThan(DateTime.UtcNow)
                .WithMessage("BirthDate cannot be in the future");

            RuleFor(x => x.PositionId)
                .GreaterThan(0)
                .WithMessage("PositionId is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required");

            RuleFor(x => x.FiredDate)
                .Must((req, firedDate) =>
                {
                    if (req.Status == "Fired" && firedDate == null)
                        return false;

                    return true;
                })
                .WithMessage("FiredDate is required when Status = Fired");
        }
    }
}
