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
        // EnsureLookupValuesExist();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity and related configurations
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Vehicles)
            .WithOne(v => v.User)
            .HasForeignKey(v => v.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Locations)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Licenses)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        // Identity entity
        modelBuilder.Entity<Identity>()
            .HasOne(i => i.User)
            .WithOne(u => u.Identity)
            .HasForeignKey<Identity>(i => i.UserId);

        // Ride entity and related configurations
        modelBuilder.Entity<Ride>()
            .HasOne(r => r.PickupLocation)
            .WithMany()
            .HasForeignKey(r => r.PickupLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Destination)
            .WithMany()
            .HasForeignKey(r => r.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Passenger)
            .WithMany()
            .HasForeignKey(r => r.PassengerId);

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Driver)
            .WithMany()
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // Make the Driver relationship optional

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Vehicle)
            .WithMany()
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // Make the Vehicle relationship optional

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Payment)
            .WithOne(p => p.Ride)
            .HasForeignKey<Payment>(p => p.RideId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment entity
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PaymentMethod)
            .WithMany()
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Feedback entity
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Ride)
            .WithOne(r => r.Feedback)
            .HasForeignKey<Feedback>(f => f.RideId)
            .OnDelete(DeleteBehavior.Cascade); // Ensure proper delete behavior

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
            entry.Entity.OnSave(entry.State);

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
