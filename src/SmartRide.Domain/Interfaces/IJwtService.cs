using System.Security.Claims;

namespace SmartRide.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, TimeSpan expiration);
    ClaimsPrincipal? ValidateToken(string token);
}
