using Ardalis.Specification;

namespace LiWiMus.Core.Plans.Specifications;

public sealed class UserPlanByUserAndPlanNameSpec : Specification<UserPlan>, ISingleResultSpecification
{
    public UserPlanByUserAndPlanNameSpec(User user, string planName)
    {
        Query.Where(userPlan => userPlan.UserId == user.Id && userPlan.Plan.Name == planName);
    }
}