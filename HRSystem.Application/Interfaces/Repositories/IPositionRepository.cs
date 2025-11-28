using HRSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.Interfaces.Repositories
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        Task<List<Position>> GetTreeAsync();
    }
}
