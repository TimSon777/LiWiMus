using LiWiMus.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Services;

public class ApplicationUserValidator : UserValidator<User>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        var result = await base.ValidateAsync(manager, user);
        var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

        if (user.UserName.Length > 20)
        {
            errors.Add(new IdentityError
            {
                Description = "The length of the username must not exceed 20 characters"
            });
        }

        if (user.UserName.Length < 3)
        {
            errors.Add(new IdentityError
            {
                Description = "The username must be at least 3 characters long"
            });
        }

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
}