
using Taka.App.Authentication.Domain.Dtos;

namespace Taka.App.Authentication.Domain.Responses
{    public record AuthResponse(string Token, UserResponseRequest User);
}
