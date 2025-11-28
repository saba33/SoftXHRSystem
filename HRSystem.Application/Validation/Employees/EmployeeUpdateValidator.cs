using FluentValidation;
using HRSystem.Application.DTOs.Employees.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
