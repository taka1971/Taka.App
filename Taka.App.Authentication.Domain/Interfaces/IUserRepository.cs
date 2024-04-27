using Taka.App.Authentication.Domain.Entities;

namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAdminAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }
}
