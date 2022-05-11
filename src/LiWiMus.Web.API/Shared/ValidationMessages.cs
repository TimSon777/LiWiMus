namespace LiWiMus.Web.API.Shared;

public static class ValidationMessages
{
    public const string DateLessThenNow =
        "The publication date must be less than or equal to the current date in Utc format";

    public static string MustHas(string subject, string @object)
    {
        return $"{subject} must has {@object}";
    }
}