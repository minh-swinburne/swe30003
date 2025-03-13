using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Entities.Join;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Persistence;

public class SmartRideDbContext(DbContextOptions<SmartRideDbContext> options, IOptions<DbSettings> dbSettings) : DbContext(options)
{
    private readonly DbSettings _dbSettings = dbSettings.Value;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>();

        // Apply custom table name mapping
        if (_dbSettings.UseSnakeCaseNaming)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Pluralize and convert to snake_case
                string tableName = (entityType.GetTableName() ?? entityType.ClrType.Name).Pluralize().Underscore();
                modelBuilder.Entity(entityType.ClrType).ToTable(tableName);


                // Apply snake_case to all column names
                foreach (var property in entityType.GetProperties())
                {
                    string columnName = property.Name.Underscore();
                    property.SetColumnName(columnName);
                }
            }
        }
    }
}
