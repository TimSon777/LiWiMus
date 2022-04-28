using Ardalis.Specification;

namespace LiWiMus.Core.Albums.Specifications;

public sealed class DetailedAlbumByIdSpec : Specification<Album>, ISingleResultSpecification
{
    public DetailedAlbumByIdSpec(int albumId)
    {
        Query.Where(album => album.Id == albumId)
             .Include(album => album.Tracks)
             .Include(album => album.Owners);
    }
}