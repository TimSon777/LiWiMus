using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Enums;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Data.Seeders;

public class PlanSeeder : ISeeder
{
    private readonly IRepository<Plan> _plansRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IPlanManager _planManager;
    private readonly UserManager<User> _userManager;

    public PlanSeeder(IRepository<Plan> plansRepository, IRepository<Permission> permissionRepository,
                      IPlanManager planManager, UserManager<User> userManager)
    {
        _plansRepository = plansRepository;
        _permissionRepository = permissionRepository;
        _planManager = planManager;
        _userManager = userManager;
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

        const string userName = "MockUser_Plan";

        if (await _userManager.FindByNameAsync(userName) is not null)
        {
            return;
        }
        
        switch (environmentType)
        {
            case EnvironmentType.Testing:
                var user = new User
                {
                    Id = 180000,
                    UserName = userName,
                    Email = "mockEmail@mock.mock_Plan",
                    Gender = Gender.Male,
                };

                var result = await _userManager.CreateAsync(user, "Password");
                
                if (!result.Succeeded)
                {
                    throw new SystemException();
                }
                
                var permission = await _permissionRepository.GetByIdAsync(4);
                var permission2 = await _permissionRepository.GetByIdAsync(5);
                
                if (permission is null || permission2 is null)
                {
                    throw new SystemException();
                }
                
                var plan = new Plan
                {
                    Id = 180000,
                    Description = "Description",
                    Name = "MockPlan_Plan",
                    PricePerMonth = 90,
                    Permissions = new List<Permission> { permission, permission2 }
                };

                await _plansRepository.AddAsync(plan);
                break;
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