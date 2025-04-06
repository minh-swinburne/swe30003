namespace SmartRide.Common.Constants;

public readonly record struct VehicleConstants
{
    public const string VinPattern = @"^[A-HJ-NPR-Z0-9]{17}$";
    public const string PlatePattern = @"^[A-Z0-9]{1,10}$";

    public const int VinMaxLength = 17;
    public const int PlateMaxLength = 10;
    public const int MakeMaxLength = 50;
    public const int ModelMaxLength = 50;
}
