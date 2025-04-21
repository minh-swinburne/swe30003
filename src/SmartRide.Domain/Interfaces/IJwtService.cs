using System.Security.Claims;

namespace SmartRide.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims);
    ClaimsPrincipal ValidateToken(string token);
    Dictionary<string, object> DecodeToken(string token);
}
