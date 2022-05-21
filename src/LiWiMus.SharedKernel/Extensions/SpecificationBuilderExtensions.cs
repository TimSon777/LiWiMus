using System.Linq.Expressions;
using Ardalis.Specification;

namespace LiWiMus.SharedKernel.Extensions;

public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> Paginate<T>(this ISpecificationBuilder<T> specificationBuilder, int page,
                                                       int itemsPerPage,  Expression<Func<T,object?>>? orderBy = null) where T : BaseEntity
    {
        orderBy ??= x => x.Id;
        return specificationBuilder
               .OrderBy(orderBy)
               .Skip((page - 1) * itemsPerPage)
               .Take(itemsPerPage);
    }
}