using FluentValidation;
using HRSystem.Application.DTOs.Employees.Requests;

namespace HRSystem.Application.Validation.Employees
{
    public class EmployeeUpdateValidator
        : BaseEmployeeRequestValidator<EmployeeUpdateRequest>
    {
        public EmployeeUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Employee Id is required for update");
        }
    }
}
