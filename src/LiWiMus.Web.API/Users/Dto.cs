using LiWiMus.Core.Users.Enums;
using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Users;

public class Dto : BaseDto
{
    public string? UserName { get; set; }
    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly? BirthDate { get; set; }
    public virtual Gender? Gender { get; set; }
    public decimal Balance { get; set; }
    public string? AvatarLocation { get; set; }
}