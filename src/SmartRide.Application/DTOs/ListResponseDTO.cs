using SmartRide.Common.Responses;

namespace SmartRide.Application.DTOs;

public class ListResponseDTO<T>
{
    public List<T>? Data { get; set; }
    public int Count { get; set; }
    public ResponseInfo? Info { get; set; }
    public List<ResponseInfo>? Warnings { get; set; } = [];
    public Dictionary<string, object>? Metadata { get; set; } = new Dictionary<string, object> { { "timestamp", DateTime.UtcNow } };
}
