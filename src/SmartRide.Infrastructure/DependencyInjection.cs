using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Services.Payment;
using SmartRide.Infrastructure.Settings;
using SmartRide.Infrastructure.Strategies;

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
            var dbSettings = provider.GetRequiredService<IOptions<DbSettings>>().Value;
            DbStrategyContext<SmartRideDbContext> dbStrategyContext = new();
            dbStrategyContext.SetStrategy(dbSettings.Provider);
            dbStrategyContext.Configure(options, dbSettings.ConnectionString);
        });

        // Register repositories of all entity types
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Register PaymentSettings from configuration
        services.Configure<PaymentGatewaySettings>(configuration.GetSection(nameof(PaymentGatewaySettings)));

        // Register PayPal as a singleton using configuration values
        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<PaymentGatewaySettings>>().Value.PayPal;
            return new PayPal(settings.ApiEndPoint, settings.ClientId, settings.ClientSecret);
        });

        // Register available payment strategies
        services.AddScoped<IPaymentGatewayStrategy, PayPalStrategy>();

        return services;
    }
}
