namespace LiWiMus.Web.API.Plans.Permissions.ReplaceAll;

public class Request
{
    public int PlanId { get; set; }
    public int[] Permissions { get; set; } = null!;
}