using HRSystem.Domain.Enums;

namespace HRSystem.Application.DTOs.Employees.Requests
{
    public abstract class BaseEmployeeRequest
    {
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public int PositionId { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime? FiredDate { get; set; }
    }
}
