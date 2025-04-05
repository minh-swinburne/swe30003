namespace SmartRide.Common.Helpers;

public class ResponseInfoHelper
{
    public static string GetResponseInfoCode(string code)
    {
        return code switch
        {
            "Success" => "200",
            "NotFound" => "404",
            "BadRequest" => "400",
            "Unauthorized" => "401",
            _ => "500"
        };
    }
}
