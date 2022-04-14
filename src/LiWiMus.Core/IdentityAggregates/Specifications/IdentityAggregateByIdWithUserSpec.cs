using Ardalis.Specification;

namespace LiWiMus.Core.IdentityAggregates.Specifications;

public sealed class IdentityAggregateByIdWithUserSpec : Specification<IdentityAggregate>, ISingleResultSpecification
{
    public IdentityAggregateByIdWithUserSpec(int id)
    {
        Query.Where(aggregate => aggregate.IdentityId == id);
        Query.Include(aggregate => aggregate.User);
    }
}