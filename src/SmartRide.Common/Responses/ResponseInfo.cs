namespace SmartRide.Common.Responses;

public readonly record struct ResponseInfo
{
    public string Code { get; init; }
    public string Message { get; init; }
    public ResponseInfo FormatMessage(params object[] args)
    {
        return this with { Message = string.Format(Message, args) };
    }
}
