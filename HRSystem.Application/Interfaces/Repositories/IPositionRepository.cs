using HRSystem.Domain.Entities;

namespace HRSystem.Application.Interfaces.Repositories
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        Task<List<Position>> GetTreeAsync();
    }
}
