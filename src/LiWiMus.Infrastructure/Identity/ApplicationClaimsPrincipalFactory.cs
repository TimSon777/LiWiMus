using System.Security.Claims;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Identity;

public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    private readonly IRepository<Role> _roleRepository;

    public ApplicationClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor,
                                             IRepository<Role> roleRepository) : base(userManager, optionsAccessor)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var roles = await _roleRepository.GetByUserAsync(user);

        foreach (var role in roles)
        {
            identity.AddClaims(role.Permissions.Select(p => p.GetClaim()));
        }

        return identity;
    }
}