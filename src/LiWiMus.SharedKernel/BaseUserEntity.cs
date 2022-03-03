using Microsoft.AspNetCore.Identity;

namespace LiWiMus.SharedKernel;

public class BaseUserEntity : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}