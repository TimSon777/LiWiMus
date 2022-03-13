using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class DefaultPlanSpecification : Specification<Plan>, ISingleResultSpecification
{
    public DefaultPlanSpecification()
    {
        Query.Where(plan => plan.PricePerMonth == 0);
    }
}