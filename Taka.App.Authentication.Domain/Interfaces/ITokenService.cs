using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Responses;

namespace Taka.App.Authentication.Domain.Interfaces
{
    public interface ITokenService
    {
        TokenResponse GenerateJwtToken(User user);
    }
}
