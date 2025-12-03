using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Domain.Entities.UserEntity;
using HRSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(HRDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<User> GetByUsernameAsync(string username)
            => await _dbSet.FirstOrDefaultAsync(x => x.Username == username);

        public async Task<User> GetByPersonalNumberAsync(string personalNumber)
            => await _dbSet.FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber);
    }
}
