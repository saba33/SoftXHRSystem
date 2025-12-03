using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Domain.Entities.UserEntity;

namespace HRSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetByUsernameAsync(string username)
            => _userRepository.GetByUsernameAsync(username);

        public Task<User> GetByEmailAsync(string email)
            => _userRepository.GetByEmailAsync(email);

        public Task<User> GetByPersonalNumberAsync(string personalNumber)
            => _userRepository.GetByPersonalNumberAsync(personalNumber);

        public async Task AddUserAsync(User user)
            => await _userRepository.AddAsync(user);

    }
}
