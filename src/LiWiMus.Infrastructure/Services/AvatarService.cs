#region

using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Users;
using Microsoft.Extensions.Options;

#endregion

namespace LiWiMus.Infrastructure.Services;

public class AvatarService : IAvatarService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiUriFormat = "https://avatars.dicebear.com/api/adventurer/{0}.svg?background=%23EF6817";

    private readonly DataSettings _dataSettings;

    public AvatarService(IOptions<DataSettings> dataSettingsOptions, IHttpClientFactory httpClientFactory)
    {
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
        user.AvatarPath = Path.Combine(_dataSettings.PicturesDirectory, user.UserName + ".svg");
        await File.WriteAllBytesAsync(user.AvatarPath, avatar);
    }

    private static void RemoveAvatarIfExists(User user)
    {
        if (user.AvatarPath is null) return;

        if (File.Exists(user.AvatarPath))
        {
            File.Delete(user.AvatarPath);
        }
    }
}