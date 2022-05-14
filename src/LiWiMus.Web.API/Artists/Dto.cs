using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Artists;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;

    public string About { get; set; } = null!;

    public string PhotoLocation { get; set; } = null!;
}