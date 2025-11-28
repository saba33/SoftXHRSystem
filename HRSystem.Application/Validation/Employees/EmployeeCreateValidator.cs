using FluentValidation;
using HRSystem.Application.DTOs.Employees.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Application.Validation.Employees
{
    public class EmployeeCreateValidator
        : BaseEmployeeRequestValidator<EmployeeCreateRequest>
    {
        public EmployeeCreateValidator()
        {
        }
    }
}