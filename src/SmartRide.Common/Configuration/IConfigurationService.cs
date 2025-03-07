using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartRide.Common.Configuration;

public interface IConfigurationService
{
    IConfiguration GetConfiguration();
}
