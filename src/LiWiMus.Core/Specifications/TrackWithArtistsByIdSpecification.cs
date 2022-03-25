﻿using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public class TrackWithArtistsByIdSpecification : Specification<Track>, ISingleResultSpecification
{
    public TrackWithArtistsByIdSpecification(int trackId)
    {
        Query.Where(track => track.Id == trackId)
             .Include(track => track.Artists);
    }
}