namespace LiWiMus.Core.Roles;

public static class DefaultRoles
{
    public static readonly Role User = new()
    {
        Name = nameof(User),
        Description = "Basic role"
    };

    public static readonly Role Admin = new()
    {
        Name = nameof(Admin),
        Description = "Administration role"
    };

    public static readonly Role Consultant = new()
    {
        Name = nameof(Consultant),
        Description = "Consultant role"
    };

    public static IEnumerable<Role> GetAll()
    {
        return new[] {User, Consultant, Admin};
    }
}