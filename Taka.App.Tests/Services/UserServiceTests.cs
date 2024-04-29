using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taka.App.Authentications.Tests.Validations
{
    using Xunit;
    using NSubstitute;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Taka.App.Authentication.Domain.Dtos;
    using Taka.App.Authentication.Domain.Entities;
    using Taka.App.Authentication.Domain.Interfaces;
    using Taka.App.Authentication.Application.Services;
    using FluentAssertions;
    using Taka.App.Authentication.Domain.Exceptions;
    using Taka.App.Authentication.Domain.Enums;

    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserServiceTests()
        {
            _userService = new UserService(_userRepository);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var userDto = new UserRegisterRequest("test@test.com", "password123", new List<Microservices> { Microservices.ServicesRental });
            _userRepository.GetUserByEmailAsync(userDto.Email).Returns(Task.FromResult<User>(null));
            _userRepository.CreateUserAsync(Arg.Any<User>()).Returns(Task.FromResult(new User {}));

            // Act
            var result = await _userService.CreateUserAsync(userDto);

            // Assert
            result.Should().NotBeNull();            
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new UserRegisterRequest("test@test.com", "password123", new List<Microservices>());
            var existingUser = new User { Email = userDto.Email };
            _userRepository.GetUserByEmailAsync(userDto.Email).Returns(Task.FromResult(existingUser));

            // Act
            var exception = await Record.ExceptionAsync(() => _userService.CreateUserAsync(userDto));

            // Assert
            exception.Should().BeOfType<UserFailValidationException>();
            exception.Message.Should().Be($"User {userDto.Email} already exists.");
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldAuthenticateUser_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User { Email = email, PasswordHash = _passwordHasher.HashPassword(null, password) };
            _userRepository.GetUserByEmailAsync(email).Returns(Task.FromResult(user));

            // Act
            var result = await _userService.AuthenticateAsync(email, password);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var email = "fail@test.com";
            var password = "password123";
            _userRepository.GetUserByEmailAsync(email).Returns(Task.FromResult<User>(null));

            //Act
            var exception = await Record.ExceptionAsync(() => _userService.AuthenticateAsync(email, password));

            // Act & Assert
            exception.Should().BeOfType<UserFailValidationException>();
            exception.Message.Should().Be($"User not found.");
        }        
    }

}
