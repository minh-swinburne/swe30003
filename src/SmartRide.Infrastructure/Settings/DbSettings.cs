namespace SmartRide.Infrastructure.Settings;

public enum DbProviderEnum
{
    InMemory,
    //Sqlite,
    MySql,
    //MariaDb,
    SqlServer,
    //PostgreSql,
}

public class DbSettings
{
    public required DbProviderEnum Provider { get; set; }
    public required string ConnectionString { get; set; }
    public bool UseSnakeCaseNaming { get; set; } = true;
}
