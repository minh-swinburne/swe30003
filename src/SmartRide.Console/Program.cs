using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartRide.Application;
using SmartRide.Common.Extensions;
using SmartRide.Infrastructure;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                Console.WriteLine($"Environment: {context.HostingEnvironment.EnvironmentName}");
                config.ApplyAppSettings(context.HostingEnvironment);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructure(context.Configuration);
                services.AddApplication(context.Configuration);
            });

        var host = builder.Build();
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

        Console.WriteLine("Testing Configuration Loading...");
        Console.WriteLine($"\t- AllowedHosts: {configuration["AllowedHosts"]}");
        Console.WriteLine($"\t- ConnectionString: {dbSettings?.ConnectionString}");
        Console.WriteLine($"\t- UseSnakeCaseNaming: {dbSettings?.UseSnakeCaseNaming}");

        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SmartRideDbContext>();
        int idx = 0;

        Console.WriteLine("\nDatabase Schema:");
        foreach (var entityType in dbContext.Model.GetEntityTypes())
        {
            idx++;
            Console.WriteLine($"{idx}. Table: {entityType.GetTableName()}");
            foreach (var property in entityType.GetProperties())
            {
                Console.WriteLine($"\t-Column: {property.GetColumnName()} - {property.Name} ({property.ClrType.Name})");
            }
        }
    }
}
