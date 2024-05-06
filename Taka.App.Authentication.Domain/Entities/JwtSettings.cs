namespace Taka.App.Authentication.Domain.Entities
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public List<string> Audiences { get; set; } = new List<string>();
        public int ExpiresInMinutes { get; set; }
        public int RefreshTokenExpiresInDays { get; set; }
    }
}
