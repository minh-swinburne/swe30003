using Microsoft.EntityFrameworkCore;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Persistence.Providers;

public class DbProviderContext<T> where T : DbContext
{
    private IDbProvider<T>? _dbStrategy;

    public void SetStrategy(DbProviderEnum dbProvider)
    {
        _dbStrategy = dbProvider switch
        {
            DbProviderEnum.SqlServer => new SqlServerDbProvider<T>(),
            DbProviderEnum.MySql => new MySqlDbProvider<T>(),
            _ => throw new ArgumentOutOfRangeException(nameof(dbProvider), dbProvider, "Database provider is not supported.")
        };
    }

    public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        if (_dbStrategy is null)
            throw new InvalidOperationException("Database strategy is not set.");

        _dbStrategy.Apply(optionsBuilder, connectionString);
    }
}
