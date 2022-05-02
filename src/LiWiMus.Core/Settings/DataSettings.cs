namespace LiWiMus.Core.Settings;

public class DataSettings
{
    public const string ConfigName = "DataSettings";

    public string PicturesDirectory { get; set; } = null!;
    public string MusicDirectory { get; set; } = null!;

    public void CreateDirectories()
    {
        Directory.CreateDirectory(PicturesDirectory);
        Directory.CreateDirectory(MusicDirectory);
    }
}

public enum DataType
{
    Picture,
    Music
}