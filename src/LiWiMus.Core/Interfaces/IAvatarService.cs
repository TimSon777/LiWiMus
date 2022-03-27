using LiWiMus.Core.Entities;
using LiWiMus.Core.Models;

namespace LiWiMus.Core.Interfaces;

public interface IAvatarService
{
    Task SetRandomAvatarAsync(User user, HttpClient httpClient, string contentRootPath);
    Task SetAvatarAsync(User user, ImageInfo imageInfo, string contentRootPat);
}