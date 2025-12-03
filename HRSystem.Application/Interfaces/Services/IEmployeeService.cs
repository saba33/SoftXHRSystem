using HRSystem.Application.DTOs.Employees;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse> GetByIdAsync(int id);
        Task<EmployeeResponse> CreateAsync(EmployeeCreateRequest request, int createdByUserId);
        Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdateRequest request, int updatedByUserId);
        Task DeleteAsync(int id);
        Task<List<EmployeeResponse>> SearchAsync(string keyword);
        Task<List<EmployeeResponse>> FilterAsync(EmployeeFilterRequest filter, int page = 1, int pageSize = 10, string sortBy = "FirstName",
            string sortDirection = "asc");
        Task<int> ActivatePendingEmployeesAsync();
        Task ActivateEmployeeByIdAsync(int id);
    }
}
