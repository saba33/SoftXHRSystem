using FluentValidation;
using HRSystem.Application.DTOs.Employees;

namespace HRSystem.Application.Validation.Employees
{
    public class EmployeeFilterRequestValidator : AbstractValidator<EmployeeFilterRequest>
    {
        public EmployeeFilterRequestValidator()
        {
            RuleFor(x => x.PersonalNumber)
                .MinimumLength(2)
                .MaximumLength(11)
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.PersonalNumber));

            RuleFor(x => x.PositionId)
                .GreaterThan(0)
                .When(x => x.PositionId.HasValue);
        }
    }
}
