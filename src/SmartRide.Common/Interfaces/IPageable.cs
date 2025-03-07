namespace SmartRide.Common.Interfaces;

public interface IPageable
{
    int PageSize { get; set; }
    int PageNo { get; set; }
}
