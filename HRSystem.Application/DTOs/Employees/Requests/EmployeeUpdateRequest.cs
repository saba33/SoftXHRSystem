using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Application.DTOs.Employees.Requests
{
    public class EmployeeUpdateRequest : BaseEmployeeRequest
    {
        public int Id { get; set; }
    }
}
