using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Permissions;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}