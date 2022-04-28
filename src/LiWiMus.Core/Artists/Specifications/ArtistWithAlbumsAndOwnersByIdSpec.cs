using Ardalis.Specification;

namespace LiWiMus.Core.Artists.Specifications;

public sealed class ArtistWithAlbumsAndOwnersByIdSpec : Specification<Artist>, ISingleResultSpecification
{
    public ArtistWithAlbumsAndOwnersByIdSpec(int id)
    {
        Query.Where(artist => artist.Id == id)
             .Include(artist => artist.Albums)
             .Include(artist => artist.Owners);
    }
}