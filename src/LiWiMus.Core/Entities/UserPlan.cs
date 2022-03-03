using System.Numerics;

namespace LiWiMus.Core.Entities;

public class UserPlan : BaseEntity
{
    public Plan Plan { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}