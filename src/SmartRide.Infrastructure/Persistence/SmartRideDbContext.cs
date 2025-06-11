using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Persistence;

public class SmartRideDbContext : DbContext
{
    private readonly DbSettings _dbSettings;
    private readonly IMediator _mediator;

    // DbSet properties for lookup entities
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;

    public SmartRideDbContext(DbContextOptions<SmartRideDbContext> options, IOptions<DbSettings> dbSettings, IMediator mediator) : base(options)
    {
        _dbSettings = dbSettings.Value;
        _mediator = mediator;
        // Ensure lookup values exist
        EnsureLookupValuesExist();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Base entities
        // User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Indexes
            entity.HasIndex(u => u.Email)
                .IsUnique();
            entity.HasIndex(u => u.Phone)
                .IsUnique();

            // Many-to-many relationships
            entity.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>();

            // One-to-many relationships
            entity.HasMany(u => u.Vehicles)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId);
            entity.HasMany(u => u.Locations)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);
            entity.HasMany(u => u.Licenses)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);
        });

        // Identity entity
        modelBuilder.Entity<Identity>(entity =>
        {
            // Indexes
            entity.HasIndex(i => i.NationalId)
                .IsUnique();

            // One-to-one relationships
            entity.HasOne(i => i.User)
                .WithOne(u => u.Identity)
                .HasForeignKey<Identity>(i => i.UserId);
        });

        // License entity
        modelBuilder.Entity<License>(entity =>
        {
            // Indexes
            entity.HasIndex(l => l.Number)
                .IsUnique();
        });

        // Vehicle entity
        modelBuilder.Entity<Vehicle>(entity =>
        {
            // Indexes
            entity.HasIndex(v => v.Vin)
                .IsUnique();
            entity.HasIndex(v => v.Plate)
                .IsUnique();
        });

        // Ride entity
        modelBuilder.Entity<Ride>(entity =>
        {
            // Many-to-one relationships
            entity.HasOne(r => r.PickupLocation)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Destination)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Passenger)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Driver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entity.HasOne(r => r.Vehicle)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entity.HasOne(r => r.VehicleType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-one relationships
            entity.HasOne(r => r.Payment)
                .WithOne(p => p.Ride)
                .HasForeignKey<Payment>(p => p.RideId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Feedback)
                .WithOne(f => f.Ride)
                .HasForeignKey<Feedback>(f => f.RideId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment entity
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PaymentMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        // Join entities
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });

        // Seed data for Role
        modelBuilder.Entity<Role>().HasData(
            Enum.GetValues<RoleEnum>().Select(role => new Role
            {
                Id = role,
                Name = role.ToString(),
                Description = role switch
                {
                    RoleEnum.Passenger => "User who books rides",
                    RoleEnum.Driver => "User who provides rides",
                    RoleEnum.Manager => "User who manages the system",
                    _ => string.Empty
                }
            })
        );

        // Seed data for VehicleType
        modelBuilder.Entity<VehicleType>().HasData(
            Enum.GetValues<VehicleTypeEnum>().Select(vehicleType => new VehicleType
            {
                Id = vehicleType,
                Name = vehicleType.ToString(),
                Capacity = vehicleType switch
                {
                    VehicleTypeEnum.Motorbike => 2,
                    VehicleTypeEnum.SmallCar => 4,
                    VehicleTypeEnum.LargeCar => 7,
                    _ => 0
                },
                Description = vehicleType switch
                {
                    VehicleTypeEnum.Motorbike => "Two-wheeled vehicle with only one seat",
                    VehicleTypeEnum.SmallCar => "Compact car with 4 seats",
                    VehicleTypeEnum.LargeCar => "Spacious car with 7 seats",
                    _ => string.Empty
                }
            })
        );

        // Seed data for PaymentMethod
        modelBuilder.Entity<PaymentMethod>().HasData(
            Enum.GetValues<PaymentMethodEnum>().Select(paymentMethod => new PaymentMethod
            {
                Id = paymentMethod,
                Name = paymentMethod.ToString(),
                IsEnabled = true,
                Description = paymentMethod switch
                {
                    PaymentMethodEnum.Cash => "Cash payment",
                    PaymentMethodEnum.CreditCard => "Credit card payment",
                    PaymentMethodEnum.PayPal => "PayPal payment",
                    _ => string.Empty
                }
            })
        );

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
            entry.Entity.OnSave(entry.State.ToString());

            foreach (var domainEvent in entry.Entity.DomainEvents)
            {
                // Dispatch the domain event (e.g., using MediatR or a custom dispatcher)
                _mediator.Publish(domainEvent, cancellationToken: CancellationToken.None);
                // Console.WriteLine($"Dispatching event: {domainEvent.GetType().Name}");
            }

            entry.Entity.ClearDomainEvents();
        }
    }

    private void EnsureLookupValuesExist()
    {
        if (Database.IsRelational() && Database.CanConnect())
        {
            // Ensure Role values exist
            foreach (var role in Enum.GetValues<RoleEnum>())
            {
                if (!Set<Role>().Any(r => r.Id == role))
                {
                    Set<Role>().Add(new Role { Id = role, Name = role.ToString() });
                }
            }

            // Ensure VehicleType values exist
            foreach (var vehicleType in Enum.GetValues<VehicleTypeEnum>())
            {
                if (!Set<VehicleType>().Any(vt => vt.Id == vehicleType))
                {
                    Set<VehicleType>().Add(new VehicleType
                    {
                        Id = vehicleType,
                        Name = vehicleType.ToString(),
                        Capacity = vehicleType switch
                        {
                            VehicleTypeEnum.Motorbike => 2,
                            VehicleTypeEnum.SmallCar => 4,
                            VehicleTypeEnum.LargeCar => 6,
                            _ => 0
                        }
                    });
                }
            }

            // Ensure PaymentMethod values exist
            foreach (var paymentMethod in Enum.GetValues<PaymentMethodEnum>())
            {
                if (!Set<PaymentMethod>().Any(pm => pm.Id == paymentMethod))
                {
                    Set<PaymentMethod>().Add(new PaymentMethod
                    {
                        Id = paymentMethod,
                        Name = paymentMethod.ToString(),
                        IsEnabled = true
                    });
                }
            }

            SaveChanges();
        }
    }
}
