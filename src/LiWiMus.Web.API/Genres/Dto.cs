using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Genres;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;

    public int TracksCount { get; set; }
}