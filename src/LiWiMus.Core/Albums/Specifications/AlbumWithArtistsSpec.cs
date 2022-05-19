using Ardalis.Specification;

namespace LiWiMus.Core.Albums.Specifications;

public sealed class AlbumWithArtistsSpec : Specification<Album>, ISingleResultSpecification
{
    public AlbumWithArtistsSpec(int id)
    {
        Query.Where(album => album.Id == id)
             .Include(album => album.Owners);
    }
}