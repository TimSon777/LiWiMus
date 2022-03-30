using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Constants;

public static class Roles
{
    public static readonly Role Admin = new(nameof(Admin), "Has full access to the site", TimeSpan.MaxValue);
    public static readonly Role Moderator = new(nameof(Moderator), "Removes obscene content", TimeSpan.MaxValue);
    public static readonly Role Consultant = new(nameof(Consultant), "Can help users in chat", TimeSpan.MaxValue);
    public static readonly Role Artist = new(nameof(Artist), "Advanced user", TimeSpan.MaxValue);
    public static readonly Role User = new(nameof(User), "Basic role", TimeSpan.MaxValue);

    public static Dictionary<Role, List<string>> GetPreconfiguredRoles() =>
        new()
        {
            {Admin, Permissions.GetAllPermissions()},
            {Moderator, new List<string>()},
            {Consultant, new List<string> { Permissions.Chat.Answer }},
            {Artist, new List<string>()},
            {User, new List<string> {Permissions.Chat.Ask}}
        };
}