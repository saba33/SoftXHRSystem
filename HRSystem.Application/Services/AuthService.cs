using HRSystem.Application.Common;
using HRSystem.Application.DTOs.Auth.Requsets;
using HRSystem.Application.DTOs.Auth.Responses;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Domain.Entities.UserEntity;
using Microsoft.Extensions.Logging;

namespace HRSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;

        public AuthService(IUserService userService,
                            ILogger<AuthService> logger,
                            IJwtService jwtService)
        {
            _userService = userService;
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (await _userService.GetByUsernameAsync(request.Username) != null)
                {
                    _logger.LogWarning("Register failed: Username '{Username}' already exists", request.Username);
                    throw new Exception("ასეთი Username უკვე გამოყენებულია");
                }

                if (await _userService.GetByEmailAsync(request.Email) != null)
                {
                    _logger.LogWarning("Register failed: Email '{Email}' already exists", request.Email);
                    throw new Exception("ასეთი ელფოსტა უკვე გამოყენებულია");
                }

                if (await _userService.GetByPersonalNumberAsync(request.PersonalNumber) != null)
                {
                    _logger.LogWarning("Register failed: PersonalNumber '{PersonalNumber}' already exists", request.PersonalNumber);
                    throw new Exception("ასეთი პირადი ნომერი უკვე არსებობს");
                }
                PasswordHasher.CreatePasswordHash(request.Password, out string hash, out string salt);

                var user = new User
                {
                    PersonalNumber = request.PersonalNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    BirthDate = request.BirthDate,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };

                await _userService.AddUserAsync(user);

                _logger.LogInformation("User '{Username}' registered successfully", request.Username);

                string token = _jwtService.GenerateToken(user);

                return new LoginResponse
                {
                    Token = token,
                    Username = user.Username
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for Username '{Username}'", request.Username);
                throw;
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userService.GetByUsernameAsync(request.Username);
                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogWarning("Login failed: Invalid credentials for Username '{Username}'", request.Username);
                    throw new Exception("მომხმარებელი ან პაროლი არასწორია");
                }

                string token = _jwtService.GenerateToken(user);
                _logger.LogInformation("User '{Username}' logged in successfully, Token generated", user.Username);

                return new LoginResponse
                {
                    Token = token,
                    Username = user.Username
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred during login for Username '{Username}'", request.Username);
                throw;
            }
        }
    }
}
