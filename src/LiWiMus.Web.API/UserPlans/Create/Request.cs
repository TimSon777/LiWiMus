namespace LiWiMus.Web.API.UserPlans.Create;

public class Request
{
    public int UserId { get; set; }
    public int PlanId { get; set; }
    public DateTime End { get; set; }
    public DateTime Start { get; set; } = DateTime.UtcNow;
}