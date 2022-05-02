#region

using LiWiMus.Core.Settings;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

#endregion

namespace LiWiMus.Web.Shared.Services;

public class FormFileSaver : IFormFileSaver
{
    private readonly DataSettings _dataSettings;

    public FormFileSaver(IOptions<DataSettings> dataSettings)
    {
        _dataSettings = dataSettings.Value;
    }

    public async Task<string> SaveWithRandomNameAsync(IFormFile file, DataType type)
    {
        var directory = type switch
        {
            DataType.Music => _dataSettings.MusicDirectory,
            DataType.Picture => _dataSettings.PicturesDirectory,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        var newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
        var newPath = Path.Combine(directory, newFileName);

        await SaveAsync(file, newPath);
        return newPath;
    }

    private static async Task SaveAsync(IFormFile file, string path)
    {
        var fileInfo = new FileInfo(path);
        await using var stream = fileInfo.OpenWrite();
        await file.CopyToAsync(stream);
    }
}