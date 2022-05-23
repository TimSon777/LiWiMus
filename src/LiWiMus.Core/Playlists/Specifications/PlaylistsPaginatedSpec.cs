using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Playlists.Specifications;

public sealed class PlaylistsPaginatedSpec : Specification<Playlist>
{
    public PlaylistsPaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        var (page, itemsPerPage, title) = paginationWithTitle;
        Query
            .Where(p => p.IsPublic && p.Name.Contains(title))
            .Include(p => p.Owner)
            .Paginate(page, itemsPerPage, p => p.Subscribers.Count);
    }
}

public static partial class PlaylistsRepositoryExtensions
{
    public static async Task<List<Playlist>> PaginateWithTitleAsync(this IRepository<Playlist> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new PlaylistsPaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}