namespace SmartRide.Infrastructure.Settings;

public enum DbProvider
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
    public required DbProvider Provider { get; set; }
    public required string ConnectionString { get; set; }
    public bool UseSnakeCaseNaming { get; set; } = true;
}
