using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartRide.Application;
using SmartRide.Common.Extensions;
using SmartRide.Infrastructure;

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

        Console.WriteLine("Testing Configuration Loading...");
        Console.WriteLine($"AllowedHosts: {configuration["AllowedHosts"]}");
        Console.WriteLine($"ConnectionStrings: {configuration.GetConnectionString("DefaultConnection")}");
    }
}
