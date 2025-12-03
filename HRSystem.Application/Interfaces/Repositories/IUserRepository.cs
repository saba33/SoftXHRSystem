using HRSystem.Domain.Entities.UserEntity;

namespace HRSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByPersonalNumberAsync(string personalNumber);
    }
}
