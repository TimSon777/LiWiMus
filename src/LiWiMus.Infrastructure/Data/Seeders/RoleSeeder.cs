﻿using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Interfaces;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Data.Seeders;

public class RoleSeeder : ISeeder
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<SystemPermission> _systemPermissionRepository;
    private readonly IRoleManager _roleManager;
    private readonly ApplicationContext _applicationContext;

    public RoleSeeder(IRepository<Role> roleRepository, IRepository<SystemPermission> systemPermissionRepository,
                      IRoleManager roleManager, ApplicationContext applicationContext)
    {
        _roleRepository = roleRepository;
        _systemPermissionRepository = systemPermissionRepository;
        _roleManager = roleManager;
        _applicationContext = applicationContext;
    }

    public async Task SeedAsync(EnvironmentType environmentType)
    {
        await SeedPermissionsAsync(environmentType);
        await SeedRolesAsync(environmentType);
        await _applicationContext.SaveChangesAsync();

        var admin = await _roleRepository.GetByNameAsync(DefaultRoles.Admin.Name) ?? throw new SystemException();
        var adminAccessPermission =
            await _systemPermissionRepository.GetByNameAsync(DefaultSystemPermissions.Admin.Access.Name) ??
            throw new SystemException();

        if (!await _roleManager.HasPermissionAsync(admin, adminAccessPermission))
        {
            await _roleManager.GrantPermissionAsync(admin, adminAccessPermission);
        }
    }

    private async Task SeedRolesAsync(EnvironmentType _)
    {
        if (await _roleRepository.AnyAsync(new RoleByNameSpec(DefaultRoles.User.Name)))
        {
            return;
        }

        foreach (var role in DefaultRoles.GetAll())
        {
            _applicationContext.Add(role);
        }
    }

    private async Task SeedPermissionsAsync(EnvironmentType _)
    {
        var spec = new SystemPermissionByNameSpec(DefaultSystemPermissions.Admin.Access.Name);
        if (await _systemPermissionRepository.AnyAsync(spec))
        {
            return;
        }

        foreach (var permission in DefaultSystemPermissions.GetAll())
        {
            _applicationContext.Add(permission);
        }
    }

    public int Priority => 100;
}