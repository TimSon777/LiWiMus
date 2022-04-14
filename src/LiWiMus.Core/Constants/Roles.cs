using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Constants;

public static class Roles
{
    public static readonly IdentityRole<int> Admin = new(nameof(Admin));
    public static readonly IdentityRole<int> Moderator = new(nameof(Moderator));
    public static readonly IdentityRole<int> Consultant = new(nameof(Consultant));
    public static readonly IdentityRole<int> Artist = new(nameof(Artist));
    public static readonly IdentityRole<int> User = new(nameof(User));

    public static Dictionary<IdentityRole<int>, List<string>> GetPreconfiguredRoles() =>
        new()
        {
            {Admin, Permissions.GetAllPermissions()},
            {Moderator, new List<string>()},
            {Consultant, new List<string> { Permissions.Chat.Answer }},
            {Artist, new List<string>()},
            {User, new List<string> {Permissions.Chat.Ask}}
        };
}