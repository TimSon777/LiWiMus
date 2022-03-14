using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;

namespace LiWiMus.Infrastructure.Services;

public class AvatarService : IAvatarService
{
    private const string ApiUriFormat = "https://avatars.dicebear.com/api/adventurer/{0}.svg?background=%23EF6817";

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

    public async Task SetRandomAvatarAsync(User user, HttpClient httpClient, string contentRootPath, string avatarsPath)
    {
        var avatar = await GetRandomAvatarAsync(httpClient);
        user.AvatarPath = Path.Combine(avatarsPath, user.UserName + ".svg");
        await File.WriteAllBytesAsync(Path.Combine(contentRootPath, user.AvatarPath), avatar);
    }
}