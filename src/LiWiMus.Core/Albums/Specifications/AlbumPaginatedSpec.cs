using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Albums.Specifications;

public sealed class AlbumPaginatedSpec : Specification<Album>
{
    public AlbumPaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        Query.Where(x => x.Title.Contains(paginationWithTitle.Title))
            .Include(x => x.Owners)
            .Paginate(paginationWithTitle.Page, paginationWithTitle.ItemsPerPage, x => x.Subscribers.Count * -1);
    }
}

public static partial class AlbumsRepositoryExtensions
{
    public static async Task<List<Album>> PaginateWithTitleAsync(this IRepository<Album> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new AlbumPaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}