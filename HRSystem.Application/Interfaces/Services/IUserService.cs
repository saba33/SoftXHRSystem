using HRSystem.Domain.Entities.UserEntity;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPersonalNumberAsync(string personalNumber);
        Task AddUserAsync(User user);
    }
}
