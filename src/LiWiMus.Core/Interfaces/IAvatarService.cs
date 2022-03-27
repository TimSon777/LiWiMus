using LiWiMus.Core.Entities;
using LiWiMus.Core.Models;

namespace LiWiMus.Core.Interfaces;

public interface IAvatarService
{
    Task SetRandomAvatarAsync(User user);
    Task SetAvatarAsync(User user, ImageInfo imageInfo);
}