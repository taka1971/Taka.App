namespace Taka.App.Authentication.Domain.Dtos
{
    public record RefreshTokenRequest(string Token, string UserEmail);
}
