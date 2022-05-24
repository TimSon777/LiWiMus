namespace LiWiMus.Core.Roles.Interfaces;

public interface IRoleManager
{
    Task<bool> IsInRoleAsync(User user, Role role);
    Task AddToRoleAsync(User user, Role role);
    Task AddToRoleAsync(User user, string roleName);

    Task RemoveFromRoleAsync(User user, Role role);
    Task RemoveFromRoleAsync(User user, string roleName);

    Task<bool> HasPermissionAsync(Role role, SystemPermission permission);
    Task GrantPermissionAsync(Role role, SystemPermission permission);
    Task GrantPermissionAsync(Role role, string permissionName);
}