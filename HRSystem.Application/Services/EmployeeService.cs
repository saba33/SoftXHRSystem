using AutoMapper;
using HRSystem.Application.DTOs.Employees;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;
using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Domain.Entities;
using HRSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace HRSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> CreateAsync(EmployeeCreateRequest request)
        {
            var employee = _mapper.Map<Employee>(request);
            employee.CreatedAt = DateTime.UtcNow;
            await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) throw new Exception("თანამშრომელი ვერ მოიძებნა");
            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            var list = await _employeeRepository.GetAllWithPositionAsync();
            return _mapper.Map<List<EmployeeResponse>>(list);
        }

        public async Task<EmployeeResponse> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetWithPositionAsync(id);
            if (employee == null) throw new Exception("თანამშრომელი ვერ მოიძებნა");
            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdateRequest request)
        {
            var existing = await _employeeRepository.GetByIdAsync(id);
            if (existing == null) throw new Exception("თანამშრომელი ვერ მოიძებნა");

            _mapper.Map(request, existing);
            await _employeeRepository.UpdateAsync(existing);
            return _mapper.Map<EmployeeResponse>(existing);
        }

        public async Task<List<EmployeeResponse>> SearchAsync(string keyword)
        {
            var list = await _employeeRepository.SearchAsync(keyword);
            return _mapper.Map<List<EmployeeResponse>>(list);
        }

        public async Task<List<EmployeeResponse>> FilterAsync(
            EmployeeFilterRequest filter,
            int page = 1,
            int pageSize = 10,
            string sortBy = "FirstName",
            string sortDirection = "asc")
        {
            var query = _employeeRepository.Query();

            query = ApplyFiltering(query, filter);
            query = ApplySorting(query, sortBy, sortDirection);

            var list = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<EmployeeResponse>>(list);
        }

        private IQueryable<Employee> ApplyFiltering(IQueryable<Employee> query, EmployeeFilterRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));

            if (!string.IsNullOrWhiteSpace(filter.LastName))
                query = query.Where(x => x.LastName.Contains(filter.LastName));

            if (!string.IsNullOrWhiteSpace(filter.PersonalNumber))
                query = query.Where(x => x.PersonalNumber.StartsWith(filter.PersonalNumber));

            if (filter.PositionId.HasValue)
                query = query.Where(x => x.PositionId == filter.PositionId.Value);

            return query;
        }

        private IQueryable<Employee> ApplySorting(IQueryable<Employee> query, string sortBy, string sortDirection)
        {
            var isDescending = sortDirection?.ToLower() == "desc";

            return sortBy?.ToLower() switch
            {
                "lastname" => isDescending ? query.OrderByDescending(e => e.LastName) : query.OrderBy(e => e.LastName),
                "position" => isDescending ? query.OrderByDescending(e => e.Position.Name) : query.OrderBy(e => e.Position.Name),
                _ => isDescending ? query.OrderByDescending(e => e.FirstName) : query.OrderBy(e => e.FirstName),
            };
        }
        public async Task<int> ActivatePendingEmployeesAsync()
        {
            var pendingEmployees = await _employeeRepository
                .Query()
                .Where(e => e.Status == EmployeeStatus.Inactive)
                .ToListAsync();

            if (!pendingEmployees.Any())
                return 0;

            foreach (var employee in pendingEmployees)
            {
                employee.Status = EmployeeStatus.Active;
                employee.TerminationDate = null;
            }

            await _employeeRepository.UpdateRangeAsync(pendingEmployees);

            return pendingEmployees.Count;
        }
        public async Task ActivateEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null) return;

            if (employee.Status == EmployeeStatus.Active)
                return;

            employee.Status = EmployeeStatus.Active;
            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
