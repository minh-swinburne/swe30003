namespace SmartRide.Infrastructure.Settings;

public class DbSettings
{
    public required string ConnectionString { get; set; }
    public bool UseSnakeCaseNaming { get; set; } = true;
}
