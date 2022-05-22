using Ardalis.Specification;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Roles.Specifications;

public sealed class SystemPermissionsByRoleSpec : Specification<SystemPermission>
{
    public SystemPermissionsByRoleSpec(Role role)
    {
        Query.Where(permission => permission.Roles.Any(r => r.Name == role.Name));
    }

    public SystemPermissionsByRoleSpec(string roleName)
    {
        Query.Where(permission => permission.Roles.Any(r => r.Name == roleName));
    }
}

public static partial class SystemPermissionRepositoryExtensions
{
    public static async Task<List<SystemPermission>> GetByRoleAsync(this IRepository<SystemPermission> repository,
                                                                    Role role)
    {
        var spec = new SystemPermissionsByRoleSpec(role);
        return await repository.ListAsync(spec);
    }

    public static async Task<List<SystemPermission>> GetByRoleAsync(this IRepository<SystemPermission> repository,
                                                                    string roleName)
    {
        var spec = new SystemPermissionsByRoleSpec(roleName);
        return await repository.ListAsync(spec);
    }
}