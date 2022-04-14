using LiWiMus.Core.Artists;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.IdentityAggregates;

public class IdentityAggregate : IAggregateRoot
{
    public int IdentityId { get; set; }
    public User? User { get; set; }
    public Artist? Artist { get; set; }
}