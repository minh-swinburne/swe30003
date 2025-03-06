using SmartRide.Common.Responses;

namespace SmartRide.Application.DTOs;

public class ListResponseDTO<T>
{
    public required IEnumerable<T> Data { get; set; }
    public int Count { get; set; }
    public string? Code { get; set; }
    public string? Module { get; set; }
    public string? Message { get; set; }
    public List<ResponseInfo>? Warnings { get; set; } = [];
    public Dictionary<string, object>? Metadata { get; set; } = new Dictionary<string, object> { { "timestamp", DateTime.UtcNow } };
}
