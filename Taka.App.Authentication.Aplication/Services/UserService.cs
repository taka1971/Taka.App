using Microsoft.AspNetCore.Identity;
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;
using Taka.App.Authentication.Domain.Exceptions;
using Taka.App.Authentication.Domain.Enums;


namespace Taka.App.Authentication.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(UserRegisterRequest userRegisterDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userRegisterDto.Email);
            var msg = string.Empty;

            if (existingUser != null)
            {
                msg = $"User {userRegisterDto.Email} already exists.";
                throw new UserFailValidationException(msg);
            }

            var newUser = new User
            {
                Email = userRegisterDto.Email,
                PasswordHash = _passwordHasher.HashPassword(null, userRegisterDto.Password)
            };

            return await _userRepository.CreateUserAsync(newUser);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email) ?? throw new UserFailValidationException("User not found.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UserFailValidationException("Invalid password.");
            }

            return user;
        }

        public async Task<User> UpdateUserAdminAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email) ?? throw new UserFailValidationException("User not found.");

            if (!existingUser.Roles.Contains(Domain.Enums.RolesTypes.Admin))
                throw new UserFailValidationException("User is not Admin.");

            var newPassword = _passwordHasher.HashPassword(null, password);

            return await _userRepository.UpdateUserAdminAsync(email, newPassword);
        }

        public async Task EnsureAdminUserAsync()
        {
            var adminExists = await _userRepository.GetUserByEmailAsync("admin@admin.com") ;
            if (adminExists is null)
            {
                var adminUser = new User
                {
                    Email = "admin@admin.com",
                    PasswordHash = _passwordHasher.HashPassword(null, "1234mudar"),
                    Roles = new List<RolesTypes> { RolesTypes.Admin },
                    AccessibleMicroservices = new List<Microservices> { Microservices.AllServices }
                };
                await _userRepository.CreateUserAsync(adminUser);
            };
        }
    }
}

