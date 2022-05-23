using Ardalis.Specification;
using LiWiMus.Core.Shared;
using LiWiMus.Core.Tracks;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UsersPaginatedSpec : Specification<User>
{
    public UsersPaginatedSpec(PaginationWithTitle paginationWithTitle)
    {
        var (page, itemsPerPage, title) = paginationWithTitle;
        Query
            .Where(t =>
                t.FirstName != null && t.FirstName.Contains(title)
                || t.SecondName != null && t.SecondName.Contains(title)
                || t.Patronymic != null && t.Patronymic.Contains(title)
                || t.UserName.Contains(title))
            .OrderBy(p => p.Followers.Count * -1)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage);
    }
}

public static partial class UsersRepositoryExtensions
{
    public static async Task<List<User>> PaginateWithTitleAsync(this IRepository<User> repository, PaginationWithTitle paginationWithTitle)
    {
        var spec = new UsersPaginatedSpec(paginationWithTitle);
        return await repository.ListAsync(spec);
    }
}