namespace SmartRide.Common.Responses.Errors;

public class QueryErrors
{
    public static readonly string Module = "Query";

    public static readonly ResponseInfo ORDERBY_INVALID = new()
    {
        Code = $"{Module}.ORDERBY_INVALID",
        Message = "The specified OrderBy property is invalid."
    };

    public static readonly ResponseInfo PAGE_SIZE_INVALID = new()
    {
        Code = $"{Module}.PAGE_SIZE_INVALID",
        Message = "PageSize must be greater than 0."
    };

    public static readonly ResponseInfo PAGE_NO_INVALID = new()
    {
        Code = $"{Module}.PAGE_NO_INVALID",
        Message = "PageNo must be greater than 0."
    };
}
