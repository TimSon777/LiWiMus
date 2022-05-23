namespace LiWiMus.Core.Plans.Interfaces;

public interface IPlanManager
{
    Task AddToDefaultPlanAsync(User user, CancellationToken token = default);

    Task<bool> HasPermissionAsync(Plan plan, Permission permission);
    Task GrantPermissionAsync(Plan plan, Permission permission);
    Task GrantPermissionAsync(Plan plan, string permissionName);

    Task<bool> DeleteAsync(Plan plan);

    Task<bool> IsInPlanAsync(User user, Plan plan);
}