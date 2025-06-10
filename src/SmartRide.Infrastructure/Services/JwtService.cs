using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Services;

public class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var expiration = TimeSpan.FromMinutes(_jwtSettings.ExpirationInMinutes);
        var credentials = new SigningCredentials(key, _jwtSettings.Algorithm);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow.Add(TimeSpan.FromSeconds(3)),
            expires: DateTime.UtcNow.Add(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        if (string.IsNullOrEmpty(token))
            throw new BaseException("Authentication", AuthErrors.TOKEN_EMPTY);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out _);

            return principal;
        }
        catch (SecurityTokenExpiredException ex)
        {
            throw new BaseException("Authentication", AuthErrors.TOKEN_EXPIRED, ex);
        }
        // catch (SecurityTokenInvalidSignatureException ex)
        // {
        //     throw new BaseException("Authentication", AuthErrors.TOKEN_INVALID, ex);
        // }
        catch (Exception ex)
        {
            throw new BaseException("Authentication", AuthErrors.TOKEN_INVALID, ex);
        }
    }

    public Dictionary<string, object> DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Extract payload as a dictionary
        var header = jwtToken.Header.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var payload = jwtToken.Payload.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return new Dictionary<string, object>
        {
            { "Header", header },
            { "Payload", payload }
        };
    }
}