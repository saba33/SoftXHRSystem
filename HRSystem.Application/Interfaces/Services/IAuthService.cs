using HRSystem.Application.DTOs.Auth.Requsets;
using HRSystem.Application.DTOs.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse>LoginAsync(LoginRequest request);
    }
}
