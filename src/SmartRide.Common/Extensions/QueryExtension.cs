namespace SmartRide.Common.Extensions;

public static class QueryExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageSize, int pageNo)
    {
        if (pageNo < 1) pageNo = 1;
        return query.Skip((pageNo - 1) * pageSize).Take(pageSize);
    }
}
