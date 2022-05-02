#region

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

#endregion

namespace LiWiMus.Web.Shared;

[ModelBinder(BinderType = typeof(ImageFormFileModelBinder))]
public class ImageFormFile : IFormFile
{
    private IFormFile _file = null!;

    private ImageFormFile()
    {
    }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public static async Task<ImageFormFile?> CreateAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        IImageInfo? imageInfo;

        try
        {
            imageInfo = await Image.IdentifyAsync(stream);
        }
        catch (Exception)
        {
            return default;
        }

        if (imageInfo is null)
        {
            return default;
        }

        return new ImageFormFile
        {
            _file = file,
            Width = imageInfo.Width,
            Height = imageInfo.Height
        };
    }

    public Stream OpenReadStream()
    {
        return _file.OpenReadStream();
    }

    public void CopyTo(Stream target)
    {
        _file.CopyTo(target);
    }

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new())
    {
        return _file.CopyToAsync(target, cancellationToken);
    }

    public string ContentType => _file.ContentType;

    public string ContentDisposition => _file.ContentDisposition;

    public IHeaderDictionary Headers => _file.Headers;

    public long Length => _file.Length;

    public string Name => _file.Name;

    public string FileName => _file.FileName;
}