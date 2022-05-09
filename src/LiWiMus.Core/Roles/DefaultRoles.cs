using LiWiMus.Core.Permissions;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Roles;

public static class DefaultRoles
{
    public static readonly IdentityRole<int> Admin = new(nameof(Admin));
    public static readonly IdentityRole<int> Moderator = new(nameof(Moderator));
    public static readonly IdentityRole<int> Consultant = new(nameof(Consultant));
    public static readonly IdentityRole<int> Artist = new(nameof(Artist));
    public static readonly IdentityRole<int> User = new(nameof(User));

    public static Dictionary<IdentityRole<int>, List<string>> GetRolesWithPermissions() =>
        new()
        {
            {Admin, DefaultPermissions.GetAll().Select(p => p.Name).ToList()},
            {Moderator, new List<string>()},
            {Consultant, new List<string> {DefaultPermissions.Chat.Answer.Name}},
            {Artist, new List<string>()},
            {User, new List<string> {DefaultPermissions.Chat.Ask.Name}}
        };
}