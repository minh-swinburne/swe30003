using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Repositories;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbSettings from configuration
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));

        // Register DbContext
        services.AddDbContext<SmartRideDbContext>((provider, options) =>
        {
            // Extract registered settings
            var dbSettings = provider.GetRequiredService<IOptions<DbSettings>>().Value;

            options.UseSqlServer(dbSettings.ConnectionString, sqlOptions =>
            {
                // Migrations assembly is required for EF Core 5
                sqlOptions.MigrationsAssembly(typeof(SmartRideDbContext).Assembly.FullName);
                // Enable retry on failure
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        // Register repositories of all entity types
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
