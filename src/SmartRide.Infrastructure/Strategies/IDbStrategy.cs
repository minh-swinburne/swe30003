using Microsoft.EntityFrameworkCore;

namespace SmartRide.Infrastructure.Strategies;

public interface IDbStrategy<T> where T : DbContext
{
    void Apply(DbContextOptionsBuilder optionsBuilder, string connectionString);
}
