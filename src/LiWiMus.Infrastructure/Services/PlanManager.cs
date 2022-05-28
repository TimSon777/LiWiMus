using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Exceptions;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Services;

public class PlanManager : IPlanManager
{
    private readonly IRepository<Plan> _planRepository;
    private readonly IRepository<Permission> _permissionRepository;

    public PlanManager(IRepository<Plan> planRepository,
                       IRepository<Permission> permissionRepository)
    {
        _planRepository = planRepository;
        _permissionRepository = permissionRepository;
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

    public async Task<bool> DeleteAsync(Plan plan)
    {
        if (DefaultPlans.GetAll().Select(p => p.Name).Contains(plan.Name))
        {
            throw new DeleteDefaultPlanException();
        }

        await _planRepository.DeleteAsync(plan);
        return true;
    }
}