using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Albums;

public class Dto : BaseDto
{
    public string Title { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string CoverPath { get; set; } = null!;
}