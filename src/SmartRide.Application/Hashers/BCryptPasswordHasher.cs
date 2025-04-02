using Microsoft.AspNetCore.Identity;
using SmartRide.Domain.Entities;

namespace SmartRide.Application.Hashers;

public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : BaseEntity
{
    public string HashPassword(TUser user, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            throw new ArgumentNullException(nameof(hashedPassword));
        }
        if (string.IsNullOrEmpty(providedPassword))
        {
            throw new ArgumentNullException(nameof(providedPassword));
        }

        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}
