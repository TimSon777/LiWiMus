using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Genres.Specifications;

public sealed class GenrePaginatedSpec : Specification<Genre>
{
    public GenrePaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        var (page, itemsPerPage, title) = paginationWithTitle;
        Query
            .Where(x => x.Name.Contains(title))
            .Paginate(page, itemsPerPage, x => x.Tracks.Count * -1);
    }
}

public static partial class GenresRepositoryExtensions
{
    public static async Task<List<Genre>> PaginateAsync(this IRepository<Genre> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new GenrePaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}