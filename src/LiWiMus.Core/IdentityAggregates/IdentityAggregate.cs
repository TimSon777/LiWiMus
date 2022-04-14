using LiWiMus.Core.Artists;

namespace LiWiMus.Core.IdentityAggregates;

public class IdentityAggregate
{
    public int IdentityId { get; set; }
    public User? User { get; set; }
    public Artist? Artist { get; set; }
}