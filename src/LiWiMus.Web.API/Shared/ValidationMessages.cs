namespace LiWiMus.Web.API.Shared;

public static class ValidationMessages
{
    public const string DateLessOrEqualThenNow =
        "The publication date must be less than or equal to the current date in Utc format";
    
    public const string DateGreaterThenNow =
        "The publication date must be greater than the current date in Utc format";

    public static string MustHas(string subject, string @object)
    {
        return $"{subject} must has {@object}";
    }
}