namespace SmartRide.Common.Responses.Success;

public class UserSuccess
{
    public static readonly string Module = "User";
    public static readonly ResponseInfo CREATED = new()
    {
        Code = $"{Module}.CREATED",
        Message = "User created successfully."
    };
    public static readonly ResponseInfo UPDATED = new()
    {
        Code = $"{Module}.UPDATED",
        Message = "User updated successfully."
    };
    public static readonly ResponseInfo DELETED = new()
    {
        Code = $"{Module}.DELETED",
        Message = "User deleted successfully."
    };
}
