using Bogus;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Infrastructure.Seed;

public static class RideGenerator
{
    public static List<Ride> GenerateRides(
        List<User> users,
        List<Location> locations,
        List<Vehicle> vehicles,
        int rideCount)
    {
        var rides = new List<Ride>();
        var faker = new Faker();

        // Filter drivers and passengers
        var drivers = users.Where(u => u.IsDriver()).ToList();
        var passengers = users.ToList();

        var exchangeRate = new Dictionary<PaymentCurrencyEnum, decimal>
        {
            { PaymentCurrencyEnum.USD, 1.0m },
            { PaymentCurrencyEnum.EUR, 0.88m },
            { PaymentCurrencyEnum.AUD, 1.58m },
            { PaymentCurrencyEnum.VND, 25888m }
        };

        for (int i = 0; i < rideCount; i++)
        {
            var status = faker.Random.WeightedRandom(
                [
                    RideStatusEnum.Pending,
                    RideStatusEnum.Picking,
                    RideStatusEnum.Travelling,
                    RideStatusEnum.Completed,
                    RideStatusEnum.Cancelled
                ],
                [0.1f, 0.1f, 0.2f, 0.5f, 0.1f]
            );

            // Select a random driver and their vehicle if status is not Pending
            User? driver = null;
            Vehicle? vehicle = null;
            if (status != RideStatusEnum.Pending)
            {
                driver = faker.PickRandom(drivers);
                var driverVehicles = vehicles.Where(v => v.UserId == driver.Id).ToList();
                vehicle = faker.PickRandom(driverVehicles);
            }

            // Select a random passenger
            var passenger = faker.PickRandom(passengers);

            // Ensure passenger is not the same as driver
            while (driver != null && passenger.Id == driver.Id)
            {
                passenger = faker.PickRandom(passengers);
            }

            // Select random pickup and destination locations
            var pickupLocation = faker.PickRandom(locations);
            var destinationLocation = faker.PickRandom(locations);

            // Ensure pickup and destination are not the same
            while (pickupLocation.Id == destinationLocation.Id)
            {
                destinationLocation = faker.PickRandom(locations);
            }

            // Generate ride details
            var ride = new Ride
            {
                Id = Guid.NewGuid(),
                PassengerId = passenger.Id,
                DriverId = driver?.Id,
                VehicleId = vehicle?.Id,
                Status = status,
                RideType = vehicle?.VehicleTypeId != VehicleTypeEnum.LargeCar
                    ? RideTypeEnum.Standard
                    : faker.PickRandom<RideTypeEnum>(),
                VehicleTypeId = vehicle?.VehicleTypeId ?? faker.PickRandom<VehicleTypeEnum>(),
                PickupLocationId = pickupLocation.Id,
                DestinationId = destinationLocation.Id,
                PickupETA = faker.Date.Future(),
                PickupATA = faker.Random.Bool() ? faker.Date.Future() : null,
                ArrivalETA = faker.Date.Future(),
                ArrivalATA = faker.Random.Bool() ? faker.Date.Future() : null,
                Fare = faker.Random.Decimal(1, 50),
                Notes = faker.Lorem.Sentence(),
                Passenger = passenger,
                Driver = driver,
                Vehicle = vehicle,
                PickupLocation = pickupLocation,
                Destination = destinationLocation
            };

            var paymentMethod = faker.PickRandom<PaymentMethodEnum>();
            var paymentStatus = ride.Status == RideStatusEnum.Cancelled
                ? faker.PickRandom(PaymentStatusEnum.Refunded, PaymentStatusEnum.Failed)
                : ride.Status == RideStatusEnum.Completed
                    ? PaymentStatusEnum.Completed
                    : paymentMethod == PaymentMethodEnum.PayPal
                        ? faker.PickRandom(PaymentStatusEnum.Pending, PaymentStatusEnum.Completed)
                        : PaymentStatusEnum.Pending;

            var currency = faker.PickRandom<PaymentCurrencyEnum>();

            ride.Payment = new Payment
            {
                Id = Guid.NewGuid(),
                RideId = ride.Id,
                Amount = ride.Fare * exchangeRate[currency],
                Currency = currency.ToString(),
                PaymentMethodId = paymentMethod,
                Status = paymentStatus,
                TransactionId = paymentStatus != PaymentStatusEnum.Pending
                    ? faker.Random.AlphaNumeric(10)
                    : null,
                TransactionTime = paymentStatus != PaymentStatusEnum.Pending
                    ? faker.Date.Recent()
                    : null,
                Ride = ride
            };

            // Add feedback for 50% of rides
            if (faker.Random.Bool(0.5f))
            {
                ride.Feedback = new Feedback
                {
                    Id = Guid.NewGuid(),
                    RideId = ride.Id,
                    Rating = faker.Random.Int(1, 5),
                    Comment = faker.Lorem.Sentence(),
                    Ride = ride
                };
            }

            rides.Add(ride);
        }

        return rides;
    }
}
