using Taka.App.Authentication.Domain.Entities;

namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
