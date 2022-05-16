using LiWiMus.Core.Permissions;
using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class PlansSeeder : ISeeder
{
    private readonly IRepository<Plan> _planRepository;
    private readonly IRepository<Permission> _permissionRepository;

    public PlansSeeder(IRepository<Plan> planRepository, IRepository<Permission> permissionRepository)
    {
        _planRepository = planRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task SeedAsync(EnvironmentType environmentType)
    {
        if (await _planRepository.AnyAsync() || await _permissionRepository.AnyAsync())
        {
            return;
        }

        var plans = DefaultPlans.GetAll().ToList();
        
        foreach (var plan in plans)
        {
            await _planRepository.AddAsync(plan);
        }

        foreach (var permission in DefaultPermissions.GetPrivate())
        {
            await _permissionRepository.AddAsync(permission);
        }
    }

    public int Priority => 90;
}