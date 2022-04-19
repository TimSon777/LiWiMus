namespace LiWiMus.Core.Plans;

public interface IUserPlanManager
{
    Task<bool> IsInPlan(User user, string planName);
    Task AddToPlan(User user, string planName, TimeSpan time);
}