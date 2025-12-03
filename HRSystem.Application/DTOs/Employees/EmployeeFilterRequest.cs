namespace HRSystem.Application.DTOs.Employees
{
    public class EmployeeFilterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public int? PositionId { get; set; }
    }
}
