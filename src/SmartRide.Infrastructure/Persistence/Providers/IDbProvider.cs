using Microsoft.EntityFrameworkCore;

namespace SmartRide.Infrastructure.Persistence.Providers;

public interface IDbProvider<T> where T : DbContext
{
    void Apply(DbContextOptionsBuilder optionsBuilder, string connectionString);
}
