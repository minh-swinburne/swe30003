namespace SmartRide.Common.Responses;

public readonly record struct ResponseInfo
{
    public string Code { get; init; }
    public string Message { get; init; }
    public ResponseInfo FormatMessage(params (string Key, object Value)[] replacements)
    {
        var formattedMessage = Message;
        foreach (var (key, value) in replacements)
        {
            formattedMessage = formattedMessage.Replace($"{{{key}}}", value.ToString());
        }
        return this with { Message = formattedMessage };
    }
}
