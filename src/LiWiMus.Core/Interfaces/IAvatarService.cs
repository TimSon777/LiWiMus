using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Interfaces;

public interface IAvatarService
{
    Task SetRandomAvatarAsync(User user, HttpClient httpClient, string contentRootPath);
}