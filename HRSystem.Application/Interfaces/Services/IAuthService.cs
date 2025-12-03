using HRSystem.Application.DTOs.Auth.Requsets;
using HRSystem.Application.DTOs.Auth.Responses;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
