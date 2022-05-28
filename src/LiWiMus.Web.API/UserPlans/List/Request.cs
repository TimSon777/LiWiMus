namespace LiWiMus.Web.API.UserPlans.List;

public class Request
{
    public int? UserId { get; set; }
    public int? PlanId { get; set; }
    public bool? Active { get; set; }

    public Request(int? userId, int? planId, bool? active)
    {
        UserId = userId;
        PlanId = planId;
        Active = active;
    }
}