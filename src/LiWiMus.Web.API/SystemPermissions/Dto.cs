using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.SystemPermissions;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}