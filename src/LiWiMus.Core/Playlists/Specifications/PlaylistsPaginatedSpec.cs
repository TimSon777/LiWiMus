using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Playlists.Specifications;

public sealed class PlaylistsPaginatedSpec : Specification<Playlist>
{
    public PlaylistsPaginatedSpec(string name, Pagination pagination)
    {
        Query
            .Where(p => p.IsPublic && p.Name.Contains(name.ToLower()))
            .Include(p => p.Owner)
            .Paginate(pagination.Page, pagination.Take, p => p.Subscribers.Count);
    }
}