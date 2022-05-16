using System.Security.Claims;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Roles;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Data.Seeders;

// ReSharper disable once UnusedType.Global
public class RoleSeeder : ISeeder
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleSeeder(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public async Task SeedAsync(EnvironmentType environmentType)
    {
        foreach (var (role, permissions) in DefaultRoles.GetRolesWithPermissions())
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                continue;
            }
            
            await _roleManager.CreateAsync(role);
            await AddPermissionClaim(role, permissions);
        }
    }
    
    private async Task AddPermissionClaim(IdentityRole<int> role, IEnumerable<string> permissions)
    {
        var claims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions
                     .Where(permission => !claims
                         .Any(claim => claim.Type == Permission.ClaimType && claim.Value == permission)))
        {
            await _roleManager.AddClaimAsync(role, new Claim(Permission.ClaimType, permission));
        }
    }

    public int Priority => 100;
}