using LiWiMus.SharedKernel;
using PermissionDto = LiWiMus.Web.API.SystemPermissions.Dto;

namespace LiWiMus.Web.API.Roles;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<PermissionDto> Permissions { get; set; } = null!;
}