using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Entities;

namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserRegisterRequest userRegisterDto);
        Task<User> UpdateUserAdminAsync(string email, string password);
        Task<User> AuthenticateAsync(string email, string password);
        Task EnsureAdminUserAsync();        
    }
}
