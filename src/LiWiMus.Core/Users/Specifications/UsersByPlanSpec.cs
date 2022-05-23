using Ardalis.Specification;
using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UsersByPlanSpec : Specification<User>
{
    public UsersByPlanSpec(Plan plan)
    {
        Query.Where(user => user.UserPlan != null && user.UserPlan.PlanId == plan.Id)
             .Include(user => user.UserPlan);
    }

    public UsersByPlanSpec(string planName)
    {
        Query.Where(user => user.UserPlan != null && user.UserPlan.Plan.Name == planName)
             .Include(user => user.UserPlan);
    }
}

public static partial class UserRepositoryExtensions
{
    public static async Task<List<User>> GetInPlanAsync(this IRepository<User> repository, Plan plan)
    {
        var spec = new UsersByPlanSpec(plan);
        return await repository.ListAsync(spec);
    }

    public static async Task<List<User>> GetInPlanAsync(this IRepository<User> repository, string planName)
    {
        var spec = new UsersByPlanSpec(planName);
        return await repository.ListAsync(spec);
    }
}