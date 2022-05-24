using Ardalis.Specification;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Plans.Specifications;

public sealed class UserPlanByUserSpec : Specification<UserPlan>, ISingleResultSpecification
{
    public UserPlanByUserSpec(User user)
    {
        Query.Where(up => up.Id == user.UserPlanId)
             .Include(up => up.Plan)
             .Include(up => up.User);
    }

    public UserPlanByUserSpec(int userId)
    {
        Query.Where(up => up.User.Id == userId)
             .Include(up => up.Plan)
             .Include(up => up.User);
    }
}

public static partial class UserPlanRepositoryExtensions
{
    public static async Task<UserPlan?> GetByUserAsync(this IRepository<UserPlan> repository, User user)
    {
        var spec = new UserPlanByUserSpec(user);
        return await repository.GetBySpecAsync(spec);
    }

    public static async Task<UserPlan?> GetByUserAsync(this IRepository<UserPlan> repository, int userId)
    {
        var spec = new UserPlanByUserSpec(userId);
        return await repository.GetBySpecAsync(spec);
    }
}