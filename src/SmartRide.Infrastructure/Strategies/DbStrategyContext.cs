using Microsoft.EntityFrameworkCore;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Strategies;

public class DbStrategyContext<T> where T : DbContext
{
    private IDbStrategy<T>? _dbStrategy;

    public void SetStrategy(DbProvider dbProvider)
    {
        _dbStrategy = dbProvider switch
        {
            DbProvider.SqlServer => new SqlServerDbStrategy<T>(),
            DbProvider.MySql => new MySqlDbStrategy<T>(),
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
