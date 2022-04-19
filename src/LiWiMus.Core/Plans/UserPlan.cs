﻿using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Plans;

public class UserPlan : IAggregateRoot
{
    public User User { get; set; } = null!;
    public Plan Plan { get; set; } = null!;

    public int UserId { get; set; }
    public int PlanId { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}