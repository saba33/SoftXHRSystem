using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Domain.Entities;
using HRSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Infrastructure.Repositories
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        public PositionRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<List<Position>> GetTreeAsync()
        {
            return await _dbSet
                .Include(x => x.Children)
                .ToListAsync();
        }
    }
}
