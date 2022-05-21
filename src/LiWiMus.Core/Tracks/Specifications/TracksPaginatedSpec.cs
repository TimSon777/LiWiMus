using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Tracks.Specifications;

public sealed class TracksPaginatedSpec : Specification<Track>
{
    public TracksPaginatedSpec(string name, Pagination pagination)
    {
        Query
            .Where(t => t.Name.ToLower().Contains(name.ToLower()))
            .Include(t => t.Album)
            .ThenInclude(a => a.Subscribers)
            .Include(t => t.Owners)
            .Paginate(pagination.Page, pagination.Take, p => p.Subscribers.Count * -1);
    }
}