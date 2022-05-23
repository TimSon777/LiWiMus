using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Artists.Specifications;

public sealed class ArtistPaginatedSpec : Specification<Artist>
{
    public ArtistPaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        Query
            .Where(x => x.Name.Contains(paginationWithTitle.Title))
            .Paginate(paginationWithTitle.Page, paginationWithTitle.ItemsPerPage, x => x.Subscribers.Count * -1);
    }    
}

public static partial class ArtistsRepositoryExtensions
{
    public static async Task<List<Artist>> PaginateAsync(this IRepository<Artist> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new ArtistPaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}