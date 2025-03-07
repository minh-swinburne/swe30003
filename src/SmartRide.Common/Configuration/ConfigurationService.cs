using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartRide.Common.Configuration;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IHostEnvironment environment)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();
    }

    public IConfiguration GetConfiguration()
    {
        return _configuration;
    }
}
