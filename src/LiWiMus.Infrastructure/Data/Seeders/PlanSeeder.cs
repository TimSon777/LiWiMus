using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Data.Seeders;

public class PlanSeeder : ISeeder
{
    private readonly IRepository<Plan> _plansRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IPlanManager _planManager;

    public PlanSeeder(IRepository<Plan> plansRepository, IRepository<Permission> permissionRepository,
                      IPlanManager planManager)
    {
        _plansRepository = plansRepository;
        _permissionRepository = permissionRepository;
        _planManager = planManager;
    }

    public async Task SeedAsync(EnvironmentType environmentType)
    {
        await SeedPlansAsync(environmentType);
        await SeedPermissionsAsync(environmentType);

        var premium = await _plansRepository.GetByNameAsync(DefaultPlans.Premium.Name) ?? throw new SystemException();
        foreach (var permissionRaw in DefaultPermissions.GetPremium())
        {
            var permission = await _permissionRepository.GetByNameAsync(permissionRaw.Name) ??
                             throw new SystemException();
            if (!await _planManager.HasPermissionAsync(premium, permission))
            {
                await _planManager.GrantPermissionAsync(premium, permission);
            }
        }
    }

    private async Task SeedPlansAsync(EnvironmentType _)
    {
        var planByNameSpec = new PlanByNameSpec(DefaultPlans.Free.Name);
        if (await _plansRepository.AnyAsync(planByNameSpec))
        {
            return;
        }

        foreach (var plan in DefaultPlans.GetAll())
        {
            await _plansRepository.AddAsync(plan);
        }
    }

    private async Task SeedPermissionsAsync(EnvironmentType _)
    {
        var permissionByNameSpec = new PermissionByNameSpec(DefaultPermissions.Track.Download.Name);
        if (await _permissionRepository.AnyAsync(permissionByNameSpec))
        {
            return;
        }

        foreach (var permission in DefaultPermissions.GetAll())
        {
            await _permissionRepository.AddAsync(permission);
        }
    }

    public int Priority => 90;
}