namespace LiWiMus.Core.Plans;

public class UserPlan
{
    public int UserId { get; set; }
    public int PlanId { get; set; }
    public User User { get; set; } = null!;
    public Plan Plan { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}