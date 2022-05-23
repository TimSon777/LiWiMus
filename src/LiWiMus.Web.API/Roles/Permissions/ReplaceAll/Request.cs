namespace LiWiMus.Web.API.Roles.Permissions.ReplaceAll;

public class Request
{
    public int RoleId { get; set; }
    public int[] Permissions { get; set; } = null!;
}