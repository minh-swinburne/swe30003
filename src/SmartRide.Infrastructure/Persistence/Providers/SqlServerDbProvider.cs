using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Infrastructure.Persistence.Providers;

public class SqlServerDbProvider<T> : IDbProvider<T> where T : DbContext
{
    public void Apply(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            // Migrations assembly is required for EF Core 5
            sqlOptions.MigrationsAssembly(typeof(T).Assembly.FullName);
            // Enable retry on failure
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    }
}
