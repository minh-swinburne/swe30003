using Microsoft.EntityFrameworkCore;

namespace SmartRide.Domain.Interfaces;

public interface IDbProvider<T> where T : DbContext
{
    void Apply(DbContextOptionsBuilder optionsBuilder, string connectionString);
}
