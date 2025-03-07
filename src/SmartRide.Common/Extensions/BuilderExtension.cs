using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartRide.Common.Extensions;

public static class BuilderExtension
{
    public static void ApplyAppSettings(this IConfigurationBuilder configuration, IHostEnvironment environment)
    {
        // Get solution root
        string? solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;

        // Clear default sources and manually load configuration
        configuration.Sources.Clear();
        configuration
            .SetBasePath(solutionRoot ?? AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}
