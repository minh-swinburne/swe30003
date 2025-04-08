namespace SmartRide.Infrastructure.Settings;

public class JwtSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Algorithm { get; set; } = "HS256";
    public required string SecretKey { get; set; }
    public int ExpirationInMinutes { get; set; } = 60;
}
