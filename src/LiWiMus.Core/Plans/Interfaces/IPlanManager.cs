namespace LiWiMus.Core.Plans.Interfaces;

public interface IPlanManager
{

    Task<bool> HasPermissionAsync(Plan plan, Permission permission);
    Task GrantPermissionAsync(Plan plan, Permission permission);
    Task GrantPermissionAsync(Plan plan, string permissionName);

    Task<bool> DeleteAsync(Plan plan);
}