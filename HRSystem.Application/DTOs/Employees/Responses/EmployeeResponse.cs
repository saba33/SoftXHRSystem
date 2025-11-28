using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Application.DTOs.Employees.Responses
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PositionName { get; set; }
        public string Status { get; set; }
        public DateTime? FiredDate { get; set; }
    }
}
