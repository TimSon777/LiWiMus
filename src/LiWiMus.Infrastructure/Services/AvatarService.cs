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

    private readonly SharedSettings _settings;

    public AvatarService(IOptions<SharedSettings> settingsOptions, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settingsOptions.Value;
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
        try
        {
            RemoveAvatarIfExists(user);
            var avatar = await GetRandomAvatarAsync();
            
            var fakeDirectory = _settings.DataSettings.PicturesDirectory;
            
            var fileName = Path.ChangeExtension(Path.GetRandomFileName(), "svg");
            
            var fakePath = Path.Combine(fakeDirectory, fileName);
            var realPath = GetRealPath(fakePath);
            
            user.AvatarLocation = Path.Combine(fakePath);
            await File.WriteAllBytesAsync(realPath, avatar);
        }
        catch (Exception)
        {
            user.AvatarLocation = null;
        }
        
    }

    private string GetRealPath(string fakePath)
    {
        return Path.Combine(_settings.SharedDirectory, fakePath);
    }

    private void RemoveAvatarIfExists(User user)
    {
        if (user.AvatarLocation is null) return;

        var realPath = GetRealPath(user.AvatarLocation);

        if (File.Exists(realPath))
        {
            File.Delete(realPath);
        }
    }
}