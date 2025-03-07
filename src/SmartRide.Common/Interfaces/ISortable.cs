namespace SmartRide.Common.Interfaces;

public interface ISortable
{
    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
}
