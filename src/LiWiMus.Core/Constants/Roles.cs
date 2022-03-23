using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Constants;

public static class Roles
{
    public static Role Admin = new Role("Admin", "Administrator");
    public static Role Moderator = new Role("Moderator", "Moderator");
    public static Role Artist = new Role("Artist", "Advanced user");
    public static Role User = new Role("User", "Basic role");
}