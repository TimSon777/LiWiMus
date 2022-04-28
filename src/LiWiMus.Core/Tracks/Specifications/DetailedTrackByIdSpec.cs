using Ardalis.Specification;

namespace LiWiMus.Core.Tracks.Specifications;

public sealed class DetailedTrackByIdSpec : Specification<Track>, ISingleResultSpecification
{
    public DetailedTrackByIdSpec(int id)
    {
        Query.Where(track => track.Id == id)
             .Include(track => track.Genres)
             .Include(track => track.Owners);
    }
}