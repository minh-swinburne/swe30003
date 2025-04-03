﻿using Humanizer;
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

        modelBuilder.Entity<User>()
            .HasMany(u => u.Vehicles)
            .WithOne(v => v.User)
            .HasForeignKey(v => v.UserId);

        modelBuilder.Entity<Identity>()
            .HasOne(i => i.User)
            .WithOne(u => u.Identity)
            .HasForeignKey<Identity>(i => i.UserId);

        modelBuilder.Entity<License>()
            .HasOne(l => l.User)
            .WithMany(u => u.Licenses)
            .HasForeignKey(l => l.UserId);
            // .OnDelete(DeleteBehavior.Cascade); // should have been handled by [Required] attribute, but just in case

        modelBuilder.Entity<Location>()
            .HasOne(l => l.User)
            .WithMany(u => u.Locations)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Ride>(entity =>
        {
            // Relationships
            entity.HasOne(r => r.PickupLocation)
                .WithMany()
                .HasForeignKey(r => r.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Destination)
                .WithMany()
                .HasForeignKey(r => r.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Check constraints
            entity.ToTable(t => t.HasCheckConstraint("CK_Ride_Fare", "[Fare] >= 0"));
        });

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

    public override int SaveChanges()
    {
        ProcessEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ProcessEntities()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.OnSave(entry.State);

            foreach (var domainEvent in entry.Entity.DomainEvents)
            {
                // Dispatch the domain event (e.g., using MediatR or a custom dispatcher)
                // _mediator.Publish(domainEvent, cancellationToken: CancellationToken.None);
                Console.WriteLine($"Dispatching event: {domainEvent.GetType().Name}");
            }

            entry.Entity.ClearDomainEvents();
        }
    }
}
