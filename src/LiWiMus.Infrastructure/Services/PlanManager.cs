using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Exceptions;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Services;

public class PlanManager : IPlanManager
{
    private readonly IRepository<Plan> _planRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Permission> _permissionRepository;

    public PlanManager(IRepository<Plan> planRepository, IRepository<User> userRepository,
                       IRepository<Permission> permissionRepository)
    {
        _planRepository = planRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task AddToDefaultPlanAsync(User user, CancellationToken token = default)
    {
        var defaultPlan = await _planRepository.GetBySpecAsync(new PlanByNameSpec(DefaultPlans.Free.Name), token);

        if (defaultPlan is null)
        {
            throw new InvalidOperationException("Default plan not added to database");
        }

        var userPlan = CreateUserPlan(user, defaultPlan, TimeSpan.MaxValue);
        user.UserPlan = userPlan;
        await _userRepository.UpdateAsync(user, token);
    }

    public async Task<bool> HasPermissionAsync(Plan plan, Permission permission)
    {
        var permissions = await _permissionRepository.GetByPlanAsync(plan);
        return permissions.Any(p => p.Name == permission.Name);
    }

    public async Task GrantPermissionAsync(Plan plan, Permission permission)
    {
        if (await HasPermissionAsync(plan, permission))
        {
            throw new PlanAlreadyHasPermissionException(plan, permission);
        }

        plan.Permissions.Add(permission);
        await _planRepository.UpdateAsync(plan);
    }

    public async Task GrantPermissionAsync(Plan plan, string permissionName)
    {
        var permission = await _permissionRepository.GetByNameAsync(permissionName);
        if (permission is null)
        {
            throw new PermissionNotFoundException(permissionName);
        }

        await GrantPermissionAsync(plan, permission);
    }

    private static UserPlan CreateUserPlan(User user, Plan plan, TimeSpan time)
    {
        return new UserPlan
        {
            User = user,
            Plan = plan,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddOrMaximize(time)
        };
    }
}