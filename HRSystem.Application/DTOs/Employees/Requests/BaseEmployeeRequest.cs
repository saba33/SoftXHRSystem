using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Application.DTOs.Employees.Requests
{
    public abstract class BaseEmployeeRequest
    {
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public int PositionId { get; set; }
        public string Status { get; set; }
        public DateTime? FiredDate { get; set; }
    }
}
