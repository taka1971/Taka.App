using MongoDB.Driver;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Interfaces;

namespace Taka.App.Authentication.Infra.Data.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _refreshTokensCollection;
        public RefreshTokenRepository(IMongoDatabase database)
        {
            _refreshTokensCollection = database.GetCollection<RefreshToken>("RefreshToken");
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokensCollection.InsertOneAsync(refreshToken);
            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _refreshTokensCollection.Find(t => t.Token == token && t.IsActive && t.Expires > DateTime.UtcNow).FirstOrDefaultAsync();
        }
        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            refreshToken.Revoked = DateTime.UtcNow;
            await _refreshTokensCollection.ReplaceOneAsync(rt => rt.Id == refreshToken.Id, refreshToken);
        }
    }
}
