using Ardalis.Specification;
using LiWiMus.Core.Playlists.Specifications;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Tracks.Specifications;

public sealed class TracksPaginatedSpec : Specification<Track>
{
    public TracksPaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        var (page, itemsPerPage, title) = paginationWithTitle;
        Query
            .Where(t => t.Name.ToLower().Contains(title.ToLower()))
            .Include(t => t.Album)
            .ThenInclude(a => a.Subscribers)
            .Include(t => t.Owners)
            .Paginate(page, itemsPerPage, p => p.Subscribers.Count * -1);
    }
}

public static partial class TracksRepositoryExtensions
{
    public static async Task<List<Track>> PaginateWithTitleAsync(this IRepository<Track> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new TracksPaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}