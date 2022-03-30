using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Models;
using LiWiMus.Core.Settings;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Services;

public class AvatarService : IAvatarService
{
    private static string _contentRootPath = null!;

    public static void Configure(string contentRootPath)
    {
        _contentRootPath = contentRootPath;
    }

    private readonly IImageService _imageService;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiUriFormat = "https://avatars.dicebear.com/api/adventurer/{0}.svg?background=%23EF6817";

    private readonly DataSettings _dataSettings;

    public AvatarService(IOptions<DataSettings> dataSettingsOptions, IImageService imageService, IHttpClientFactory httpClientFactory)
    {
        _imageService = imageService;
        _httpClientFactory = httpClientFactory;
        _dataSettings = dataSettingsOptions.Value;
    }

    private async Task<byte[]> GetRandomAvatarAsync()
    {
        var seed = GetRandomSeed();
        var httpClient = _httpClientFactory.CreateClient();
        var avatar = await httpClient.GetByteArrayAsync(string.Format(ApiUriFormat, seed));
        return avatar;
    }

    private static string GetRandomSeed()
    {
        return Random.Shared.Next().ToString();
    }

    public async Task SetRandomAvatarAsync(User user)
    {
        RemoveAvatarIfExists(user);
        var avatar = await GetRandomAvatarAsync();
        user.AvatarPath = Path.Combine(_dataSettings.AvatarsDirectory, user.UserName + ".svg");
        await File.WriteAllBytesAsync(Path.Combine(_contentRootPath, user.AvatarPath), avatar);
    }

    public async Task SetAvatarAsync(User user, ImageInfo imageInfo)
    {
        RemoveAvatarIfExists(user);
        var (_, extension, image1) = imageInfo;
        var newRelativePathToAvatar = Path.Combine(_dataSettings.AvatarsDirectory, user.UserName + extension);
        var newPathToAvatar = Path.Combine(_contentRootPath, newRelativePathToAvatar);
        await _imageService.SavePictureAsync(image1, newPathToAvatar);
        user.AvatarPath = newRelativePathToAvatar;
    }

    private static void RemoveAvatarIfExists(User user)
    {
        if (user.AvatarPath is null) return;
        
        var pathToAvatar = Path.Combine(_contentRootPath, user.AvatarPath);

        if (File.Exists(pathToAvatar))
        {
            File.Delete(pathToAvatar);
        }
    }
}