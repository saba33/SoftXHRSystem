using HRSystem.Domain.Entities;

namespace HRSystem.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetWithPositionAsync(int id);

        Task<Employee?> GetByPersonalNumberAsync(string personalNumber);

        Task<List<Employee>> SearchAsync(string keyword);

        Task<bool> ExistsByPersonalNumberAsync(string personalNumber);

        Task<List<Employee>> GetAllWithPositionAsync();
        Task<List<Employee>> SearchEmployeesAsync(
            string? personalNumber,
            int? positionId);
    }
}
