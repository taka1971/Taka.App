using MongoDB.Driver;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Enums;
using Taka.App.Authentication.Domain.Interfaces;

namespace Taka.App.Authentication.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<User>("Users");
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAdminAsync(string email, string password)
        {   
            var filter = Builders<User>.Filter.And(
                         Builders<User>.Filter.Eq(u => u.Email, email),
                         Builders<User>.Filter.AnyEq(u => u.Roles, RolesTypes.Admin));

            var update = Builders<User>.Update.Set(u => u.PasswordHash, password);
                        
            await _usersCollection.UpdateOneAsync(filter, update);
                        
            return await GetUserByEmailAsync(email);
        }
    }

}
