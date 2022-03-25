using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Constants;

public static class Roles
{
    public static readonly Role Admin = new("Admin", "Administrator", TimeSpan.MaxValue);
    public static readonly Role Moderator = new("Moderator", "Moderator", TimeSpan.MaxValue);
    public static readonly Role Artist = new("Artist", "Advanced user", TimeSpan.MaxValue);
    public static readonly Role User = new("User", "Basic role", TimeSpan.MaxValue);

    public static IEnumerable<Role> GetPreconfiguredRoles() =>
        new List<Role>()
        {
            Admin,
            Moderator,
            Artist,
            User
        };
}