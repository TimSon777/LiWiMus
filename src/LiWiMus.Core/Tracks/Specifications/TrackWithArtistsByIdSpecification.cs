using Ardalis.Specification;

namespace LiWiMus.Core.Tracks.Specifications;

public sealed class TrackWithArtistsByIdSpecification : Specification<Track>, ISingleResultSpecification
{
    public TrackWithArtistsByIdSpecification(int trackId)
    {
        Query.Where(track => track.Id == trackId)
             .Include(track => track.Owners);
    }
}