﻿using Humanizer;
using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities;

namespace SmartRide.Infrastructure.Persistence;

public class SmartRideDbContext(DbContextOptions<SmartRideDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>();

        // Apply custom table name mapping
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Pluralize and convert to snake case
            string tableName = (entityType.GetTableName() ?? entityType.ClrType.Name).Pluralize().Underscore();
            modelBuilder.Entity(entityType.ClrType).ToTable(tableName);
        }
    }
}
