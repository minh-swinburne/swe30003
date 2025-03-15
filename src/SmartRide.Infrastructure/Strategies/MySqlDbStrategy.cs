using Microsoft.EntityFrameworkCore;

namespace SmartRide.Infrastructure.Strategies;

public class MySqlDbStrategy<T> : IDbStrategy<T> where T : DbContext
{
    public void Apply(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sqlOptions =>
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
