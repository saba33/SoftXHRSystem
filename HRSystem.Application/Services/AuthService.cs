using HRSystem.Application.Common;
using HRSystem.Application.DTOs.Auth.Requsets;
using HRSystem.Application.DTOs.Auth.Responses;
using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Domain.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {

            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new Exception("ასეთი Username უკვე გამოყენებულია");

            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new Exception("ასეთი ელფოსტა უკვე გამოყენებულია");

            var existingPersonalNumber = await _userRepository.GetByPersonalNumberAsync(request.PersonalNumber);
            if (existingPersonalNumber != null)
                throw new Exception("ასეთი პირადი ნომერი უკვე არსებობს");

            PasswordHasher.CreatePasswordHash(
                request.Password,
                out string passwordHash,
                out string passwordSalt);

            var user = new User
            {
                PersonalNumber = request.PersonalNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                BirthDate = request.BirthDate,

                Username = request.Username,
                Email = request.Email,

                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                PersonalNumber = user.PersonalNumber,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Username = user.Username
            };
        }


        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new Exception("მომხმარებელი ან პაროლი არასწორია");

            var isPasswordCorrect = PasswordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isPasswordCorrect)
                throw new Exception("მომხმარებელი ან პაროლი არასწორია");

            return new UserResponse
            {
                Id = user.Id,
                PersonalNumber = user.PersonalNumber,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Username = user.Username
            };
        }
    }
}
