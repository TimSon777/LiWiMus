using Ardalis.Specification;

namespace LiWiMus.Core.Plans.Specifications;

public sealed class PlanWithPermissionsByUserSpec : Specification<UserPlan, Plan>
{
    public PlanWithPermissionsByUserSpec(User user)
    {
        Query.Where(userPlan => userPlan.UserId == user.Id && userPlan.End > DateTime.UtcNow)
             .Include(userPlan => userPlan.Plan.Permissions);
        Query.Select(userPlan => userPlan.Plan);
    }
}