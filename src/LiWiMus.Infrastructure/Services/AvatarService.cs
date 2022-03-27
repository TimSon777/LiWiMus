using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Models;
using LiWiMus.Core.Settings;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Services;

public class AvatarService : IAvatarService
{
    private readonly IImageService _imageService;
    private const string ApiUriFormat = "https://avatars.dicebear.com/api/adventurer/{0}.svg?background=%23EF6817";

    private readonly DataSettings _dataSettings;

    public AvatarService(IOptions<DataSettings> dataSettingsOptions, IImageService imageService)
    {
        _imageService = imageService;
        _dataSettings = dataSettingsOptions.Value;
    }

    private static async Task<byte[]> GetRandomAvatarAsync(HttpClient httpClient)
    {
        var seed = GetRandomSeed();
        var avatar = await httpClient.GetByteArrayAsync(string.Format(ApiUriFormat, seed));
        return avatar;
    }

    private static string GetRandomSeed()
    {
        return Random.Shared.Next().ToString();
    }

    public async Task SetRandomAvatarAsync(User user, HttpClient httpClient, string contentRootPath)
    {
        var avatar = await GetRandomAvatarAsync(httpClient);
        user.AvatarPath = Path.Combine(_dataSettings.AvatarsDirectory, user.UserName + ".svg");
        await File.WriteAllBytesAsync(Path.Combine(contentRootPath, user.AvatarPath), avatar);
    }

    public async Task SetAvatarAsync(User user, ImageInfo imageInfo, string contentRootPath)
    {
        if (user.AvatarPath is not null)
        {
            var pathToAvatar = Path.Combine(contentRootPath, user.AvatarPath);

            if (File.Exists(pathToAvatar))
            {
                File.Delete(pathToAvatar);
            }
        }

        var (_, extension, image1) = imageInfo;
        var newRelativePathToAvatar = Path.Combine(_dataSettings.AvatarsDirectory, user.UserName + extension);
        var newPathToAvatar = Path.Combine(contentRootPath, newRelativePathToAvatar);
        await _imageService.SavePictureAsync(image1, newPathToAvatar);
        user.AvatarPath = newRelativePathToAvatar;
    }
}