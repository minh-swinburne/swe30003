using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartRide.Common.Extensions;

public static class BuilderExtension
{
    public static void ApplyAppSettings(this IConfigurationBuilder configuration, string environment, string? basePath = null)
    {
        // Get solution root
        string? solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;

        // Clear default sources and manually load configuration
        configuration.Sources.Clear();
        configuration
            .SetBasePath(basePath ?? solutionRoot ?? AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    public static void ApplyAppSettings(this IConfigurationBuilder configuration, IHostEnvironment environment, string? basePath = null)
    {
        configuration.ApplyAppSettings(environment.EnvironmentName, basePath);
    }

    public static void ApplyAppSettings(this IConfigurationBuilder configuration, IWebAssemblyHostEnvironment environment, string? basePath = null)
    {
        configuration.ApplyAppSettings(environment.Environment, basePath);
    }
}
