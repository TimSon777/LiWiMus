using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Albums.Specifications;

public sealed class AlbumPaginatedSpec : Specification<Album>
{
    public AlbumPaginatedSpec(string title, Pagination pagination)
    {
        Query.Where(x => x.Title.Contains(title.ToLower()))
            .Include(x => x.Owners)
            .Paginate(pagination.Page, pagination.Take, x => x.Subscribers.Count * -1);
    }
}