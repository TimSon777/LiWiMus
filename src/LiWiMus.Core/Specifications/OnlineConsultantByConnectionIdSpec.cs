﻿using Ardalis.GuardClauses;
using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class OnlineConsultantByConnectionIdSpec : Specification<OnlineConsultant>, ISingleResultSpecification
{
    public OnlineConsultantByConnectionIdSpec(string connectionId)
    {
        Guard.Against.NullOrEmpty(connectionId);
        Query
            .Where(oc => oc.ConnectionId == connectionId)
            .Include(c => c.Chats)
            .ThenInclude(c => c.User);

        Query
            .Include(ch => ch.Chats)
            .ThenInclude(c => c.Messages);
    }
}