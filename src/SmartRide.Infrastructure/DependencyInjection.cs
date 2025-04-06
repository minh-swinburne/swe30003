using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartRide.Application.Commands;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Notification;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Persistence.Strategies;
using SmartRide.Infrastructure.Settings;
using System.Reflection;

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

        // Register repositories of all entity types
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IEmailService), typeof(EmailService));

        // Register SaveChangesCommandHandler
        services.AddScoped<IRequestHandler<SaveChangesCommand>, SaveChangesCommandHandler>();

        return services;
    }
}
