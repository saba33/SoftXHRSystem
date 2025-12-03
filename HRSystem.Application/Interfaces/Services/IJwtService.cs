using HRSystem.Domain.Entities.UserEntity;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
