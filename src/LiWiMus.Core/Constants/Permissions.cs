namespace LiWiMus.Core.Constants;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
    }
    public static class Artist
    {
        public const string View = "Permissions.Artist.View";
        public const string Create = "Permissions.Artist.Create";
        public const string Edit = "Permissions.Artist.Edit";
        public const string Delete = "Permissions.Artist.Delete";
    }
}