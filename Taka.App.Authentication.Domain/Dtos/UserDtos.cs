using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentication.Domain.Dtos
{
    public record UserRegisterDto(string Email, string Password, List<Microservices> AccessibleMicroservices);
    public record UserUpdateDto(string Email, string Password);
    public record UserLoginDto(string Email, string Password);
    public record UserResponseDto(string Id, string Email, List<Microservices> AccessibleMicroservices);
}
