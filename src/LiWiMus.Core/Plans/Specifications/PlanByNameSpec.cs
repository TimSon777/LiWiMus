using Ardalis.Specification;

namespace LiWiMus.Core.Plans.Specifications;

public sealed class PlanByNameSpec : Specification<Plan>, ISingleResultSpecification
{
    public PlanByNameSpec(string planName)
    {
        Query.Where(plan => plan.Name == planName);
    }
}