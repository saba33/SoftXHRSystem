using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Application.DTOs.Employees
{
    public class EmployeeFilterRequest
    {
        public string? PersonalNumber { get; set; } 
        public int? PositionId { get; set; }
    }
}
