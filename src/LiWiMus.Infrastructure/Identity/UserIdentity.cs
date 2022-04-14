using LiWiMus.Core.IdentityAggregates;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Identity;

public class UserIdentity : IdentityUser<int>
{
    public IdentityAggregate Aggregate { get; set; } = null!;
}