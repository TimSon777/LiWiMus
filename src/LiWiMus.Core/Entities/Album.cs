namespace LiWiMus.Core.Entities;

public class Album : BaseEntity
{
    public string Title { get; set; }
    public DateOnly PublishedAt { get; set; }
    public string CoverPath { get; set; }

    public List<LikedAlbum> Subscribers { get; set; } = new();
}