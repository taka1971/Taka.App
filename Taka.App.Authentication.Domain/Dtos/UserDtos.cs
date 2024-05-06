using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentication.Domain.Dtos
{
    public record UserRegisterRequest(string Email, string Password, List<Microservices> AccessibleMicroservices);
    public record UserUpdateRequest(string Email, string Password);
    public record UserLoginRequest(string Email, string Password);
    public record UserResponseRequest(string Id, string Email, List<Microservices> AccessibleMicroservices);
}
