namespace SmartRide.Common.Responses;

public readonly record struct ResponseInfo
{
    public string Code { get; init; }
    public string Message { get; init; }
}
