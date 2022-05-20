using Ardalis.Specification;

namespace LiWiMus.SharedKernel.Extensions;

public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> Paginate<T>(this ISpecificationBuilder<T> specificationBuilder, int page,
                                                       int itemsPerPage) where T : BaseEntity
    {
        return specificationBuilder
               .OrderBy(t => t.Id)
               .Skip((page - 1) * itemsPerPage)
               .Take(itemsPerPage);
    }
}