using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Domain.Entities;
using HRSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<Employee?> GetByPersonalNumberAsync(string personalNumber)
        {
            return await _dbSet
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber);
        }

        public async Task<Employee?> GetWithPositionAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsByPersonalNumberAsync(string personalNumber)
        {
            return await AnyAsync(x => x.PersonalNumber == personalNumber);
        }
        public async Task<List<Employee>> GetAllWithPositionAsync()
        {
            return await _dbSet
                .Include(x => x.Position)
                .ToListAsync();
        }

        public async Task<List<Employee>> SearchAsync(string keyword)
        {
            return await _dbSet
                .Include(x => x.Position)
                .Where(x =>
                    x.FirstName.Contains(keyword) ||
                    x.LastName.Contains(keyword) ||
                    x.PersonalNumber.Contains(keyword)
                )
                .ToListAsync();
        }

        public async Task<List<Employee>> SearchEmployeesAsync(
            string? personalNumber,
            int? positionId)
        {
            var query = _dbSet
                .Include(x => x.Position)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(personalNumber))
                query = query.Where(x => x.PersonalNumber.StartsWith(personalNumber));

            if (positionId.HasValue)
                query = query.Where(x => x.PositionId == positionId.Value);

            return await query.ToListAsync();
        }
    }
}
