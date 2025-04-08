using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartRide.Application.Commands;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Persistence.Strategies;
using SmartRide.Infrastructure.Services;
using SmartRide.Infrastructure.Settings;
using System.Reflection;

namespace SmartRide.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbSettings from configuration
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));

        // Register JwtSettings from configuration
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        // Register DbContext
        services.AddDbContext<SmartRideDbContext>((provider, options) =>
        {
            // Extract registered settings
            var dbSettings = provider.GetRequiredService<IOptions<DbSettings>>().Value;

            // Use the appropriate strategy based on database provider
            DbStrategyContext<SmartRideDbContext> dbStrategyContext = new();
            dbStrategyContext.SetStrategy(dbSettings.Provider);
            dbStrategyContext.Configure(options, dbSettings.ConnectionString);
        });

        // Register MediatR (Scan all event handlers)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.RegisterServicesFromAssembly(typeof(IDomainEvent).Assembly);
        });

        // Register SaveChangesCommandHandler
        services.AddScoped<IRequestHandler<SaveChangesCommand>, SaveChangesCommandHandler>();

        // Register repositories of all entity types
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        // Register external services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();

        // Register HttpClient for GoogleMapsService
        services.AddHttpClient<IMapService, GoogleMapsService>();

        return services;
    }
}
