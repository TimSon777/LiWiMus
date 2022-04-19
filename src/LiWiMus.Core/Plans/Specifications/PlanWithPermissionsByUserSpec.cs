using Ardalis.Specification;

namespace LiWiMus.Core.Plans.Specifications;

public sealed class PlanWithPermissionsByUserSpec : Specification<Plan>, ISingleResultSpecification
{
    public PlanWithPermissionsByUserSpec(User user)
    {
        Query.Where(
            plan => plan.UserPlans.Exists(userPlan => userPlan.UserId == user.Id && userPlan.End > DateTime.Now));
        Query.Include(plan => plan.Permissions);
    }
}