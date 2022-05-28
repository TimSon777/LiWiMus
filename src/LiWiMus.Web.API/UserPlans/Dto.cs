using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.UserPlans;

public class Dto : BaseDto
{
    public string PlanName { get; set; } = null!;
    public string PlanDescription { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int UserId { get; set; }
    public int PlanId { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public bool Updatable { get; set; }
}