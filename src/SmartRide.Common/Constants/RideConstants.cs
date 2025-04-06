namespace SmartRide.Common.Constants;

public readonly record struct RideConstants
{
    public const decimal MinFare = 0.5m;
    public const decimal MaxFare = 10000.0m;

    public const int NotesMaxLength = 1000;
}
