using LiWiMus.Core.Models;

namespace LiWiMus.Core.Interfaces;

public interface IAvatarService
{
    Task SetRandomAvatarAsync(Users.User user);
    Task SetAvatarAsync(Users.User user, ImageInfo imageInfo);
}