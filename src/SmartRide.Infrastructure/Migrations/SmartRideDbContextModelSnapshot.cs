﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartRide.Infrastructure.Persistence;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    [DbContext(typeof(SmartRideDbContext))]
    partial class SmartRideDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Feedback", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<sbyte>("Rating")
                        .HasColumnType("TINYINT")
                        .HasColumnName("rating");

                    b.Property<byte[]>("RideId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("ride_id");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.HasKey("Id");

                    b.HasIndex("RideId")
                        .IsUnique();

                    b.ToTable("feedbacks", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Identity", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("address");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("DATE")
                        .HasColumnName("birth_date");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("city");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<string>("LegalName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("legal_name");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("national_id");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("nationality");

                    b.Property<sbyte>("Sex")
                        .HasColumnType("TINYINT")
                        .HasColumnName("sex");

                    b.Property<sbyte>("Status")
                        .HasColumnType("TINYINT")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("NationalId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("identities", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.License", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<string>("IssuedCountry")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("issued_country");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("DATE")
                        .HasColumnName("issued_date");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnName("number");

                    b.Property<sbyte>("Status")
                        .HasColumnType("TINYINT")
                        .HasColumnName("status");

                    b.Property<sbyte>("Type")
                        .HasColumnType("TINYINT")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("licenses", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Location", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("address");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<double?>("Latitude")
                        .HasColumnType("DOUBLE")
                        .HasColumnName("latitude");

                    b.Property<double?>("Longitude")
                        .HasColumnType("DOUBLE")
                        .HasColumnName("longitude");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.Property<byte[]>("UserId")
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("locations", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Payment", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("DECIMAL(18,2)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<sbyte>("PaymentMethodId")
                        .HasColumnType("TINYINT")
                        .HasColumnName("payment_method_id");

                    b.Property<byte[]>("RideId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("ride_id");

                    b.Property<sbyte>("Status")
                        .HasColumnType("TINYINT")
                        .HasColumnName("status");

                    b.Property<DateTime?>("TransactionTime")
                        .HasColumnType("DATETIME")
                        .HasColumnName("transaction_time");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.HasKey("Id");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("RideId")
                        .IsUnique();

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Ride", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<DateTime?>("ArrivalATA")
                        .HasColumnType("DATETIME")
                        .HasColumnName("arrival_ata");

                    b.Property<DateTime?>("ArrivalETA")
                        .HasColumnType("DATETIME")
                        .HasColumnName("arrival_eta");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<byte[]>("DestinationId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("destination_id");

                    b.Property<byte[]>("DriverId")
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("driver_id");

                    b.Property<decimal>("Fare")
                        .HasColumnType("DECIMAL(18,2)")
                        .HasColumnName("fare");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT")
                        .HasColumnName("notes");

                    b.Property<byte[]>("PassengerId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("passenger_id");

                    b.Property<DateTime?>("PickupATA")
                        .HasColumnType("DATETIME")
                        .HasColumnName("pickup_ata");

                    b.Property<DateTime?>("PickupETA")
                        .HasColumnType("DATETIME")
                        .HasColumnName("pickup_eta");

                    b.Property<byte[]>("PickupLocationId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("pickup_location_id");

                    b.Property<sbyte>("RideType")
                        .HasColumnType("TINYINT")
                        .HasColumnName("ride_type");

                    b.Property<sbyte>("Status")
                        .HasColumnType("TINYINT")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.Property<byte[]>("UserId")
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id");

                    b.Property<byte[]>("VehicleId")
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("vehicle_id");

                    b.Property<sbyte>("VehicleTypeId")
                        .HasColumnType("TINYINT")
                        .HasColumnName("vehicle_type_id");

                    b.HasKey("Id");

                    b.HasIndex("DestinationId");

                    b.HasIndex("DriverId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("PickupLocationId");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("rides", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.User", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("phone");

                    b.Property<string>("Picture")
                        .HasColumnType("TEXT")
                        .HasColumnName("picture");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Vehicle", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("created_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedTime"));

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("make");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("model");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("plate");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("DATE")
                        .HasColumnName("registered_date");

                    b.Property<DateTime>("UpdatedTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasColumnName("updated_time");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("UpdatedTime"));

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id")
                        .HasColumnOrder(0);

                    b.Property<sbyte>("VehicleTypeId")
                        .HasColumnType("TINYINT")
                        .HasColumnName("vehicle_type_id")
                        .HasColumnOrder(1);

                    b.Property<string>("Vin")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("CHAR")
                        .HasColumnName("vin");

                    b.Property<int>("Year")
                        .HasColumnType("INT")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleTypeId");

                    b.HasIndex("Vin")
                        .IsUnique();

                    b.ToTable("vehicles", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Join.UserRole", b =>
                {
                    b.Property<byte[]>("UserId")
                        .HasColumnType("BINARY(16)")
                        .HasColumnName("user_id")
                        .HasColumnOrder(0);

                    b.Property<sbyte>("RoleId")
                        .HasColumnType("TINYINT")
                        .HasColumnName("role_id")
                        .HasColumnOrder(1);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Lookup.PaymentMethod", b =>
                {
                    b.Property<sbyte>("Id")
                        .HasColumnType("TINYINT")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("description");

                    b.Property<ulong>("IsEnabled")
                        .HasColumnType("BIT")
                        .HasColumnName("is_enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("payment_methods", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (sbyte)1,
                            Description = "Cash payment",
                            IsEnabled = 1ul,
                            Name = "Cash"
                        },
                        new
                        {
                            Id = (sbyte)2,
                            Description = "Credit card payment",
                            IsEnabled = 1ul,
                            Name = "CreditCard"
                        },
                        new
                        {
                            Id = (sbyte)3,
                            Description = "PayPal payment",
                            IsEnabled = 1ul,
                            Name = "PayPal"
                        });
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Lookup.Role", b =>
                {
                    b.Property<sbyte>("Id")
                        .HasColumnType("TINYINT")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (sbyte)1,
                            Description = "User who books rides",
                            Name = "Passenger"
                        },
                        new
                        {
                            Id = (sbyte)2,
                            Description = "User who provides rides",
                            Name = "Driver"
                        },
                        new
                        {
                            Id = (sbyte)3,
                            Description = "User who manages the system",
                            Name = "Manager"
                        });
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Lookup.VehicleType", b =>
                {
                    b.Property<sbyte>("Id")
                        .HasColumnType("TINYINT")
                        .HasColumnName("id");

                    b.Property<sbyte>("Capacity")
                        .HasColumnType("TINYINT")
                        .HasColumnName("capacity");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("vehicle_types", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (sbyte)1,
                            Capacity = (sbyte)2,
                            Description = "Two-wheeled vehicle with only one seat",
                            Name = "Motorbike"
                        },
                        new
                        {
                            Id = (sbyte)2,
                            Capacity = (sbyte)4,
                            Description = "Compact car with 4 seats",
                            Name = "SmallCar"
                        },
                        new
                        {
                            Id = (sbyte)3,
                            Capacity = (sbyte)7,
                            Description = "Spacious car with 7 seats",
                            Name = "LargeCar"
                        });
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Feedback", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.Ride", "Ride")
                        .WithOne("Feedback")
                        .HasForeignKey("SmartRide.Domain.Entities.Base.Feedback", "RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ride");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Identity", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.User", "User")
                        .WithOne("Identity")
                        .HasForeignKey("SmartRide.Domain.Entities.Base.Identity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.License", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.User", "User")
                        .WithMany("Licenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Location", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.User", "User")
                        .WithMany("Locations")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Payment", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Lookup.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Base.Ride", "Ride")
                        .WithOne("Payment")
                        .HasForeignKey("SmartRide.Domain.Entities.Base.Payment", "RideId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PaymentMethod");

                    b.Navigation("Ride");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Ride", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.Location", "Destination")
                        .WithMany()
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Base.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartRide.Domain.Entities.Base.User", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Base.Location", "PickupLocation")
                        .WithMany()
                        .HasForeignKey("PickupLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Base.User", null)
                        .WithMany("Rides")
                        .HasForeignKey("UserId");

                    b.HasOne("SmartRide.Domain.Entities.Base.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartRide.Domain.Entities.Lookup.VehicleType", "VehicleType")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Destination");

                    b.Navigation("Driver");

                    b.Navigation("Passenger");

                    b.Navigation("PickupLocation");

                    b.Navigation("Vehicle");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Vehicle", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Base.User", "User")
                        .WithMany("Vehicles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Lookup.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Join.UserRole", b =>
                {
                    b.HasOne("SmartRide.Domain.Entities.Lookup.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartRide.Domain.Entities.Base.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.Ride", b =>
                {
                    b.Navigation("Feedback");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Base.User", b =>
                {
                    b.Navigation("Identity");

                    b.Navigation("Licenses");

                    b.Navigation("Locations");

                    b.Navigation("Rides");

                    b.Navigation("UserRoles");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Lookup.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SmartRide.Domain.Entities.Lookup.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
