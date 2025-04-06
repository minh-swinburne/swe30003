namespace SmartRide.Common.Responses.Errors;

public class QueryErrors
{
    public static readonly string Module = "Query";

    public static readonly ResponseInfo INVALID_ORDERBY = new()
    {
        Code = $"{Module}.INVALID_ORDERBY",
        Message = "The specified OrderBy property is invalid."
    };

    public static readonly ResponseInfo INVALID_PAGE_SIZE = new()
    {
        Code = $"{Module}.INVALID_PAGE_SIZE",
        Message = "PageSize must be greater than 0."
    };

    public static readonly ResponseInfo INVALID_PAGE_NO = new()
    {
        Code = $"{Module}.INVALID_PAGE_NO",
        Message = "PageNo must be greater than 0."
    };
}
