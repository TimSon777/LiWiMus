using System.Security.Claims;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LiWiMus.Infrastructure.Identity;

public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<int>>
{
    private readonly IRepository<Plan> _planRepository;

    public ApplicationClaimsPrincipalFactory(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
                                             IOptions<IdentityOptions> options, IRepository<Plan> planRepository) : base(userManager, roleManager,
        options)
    {
        _planRepository = planRepository;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var id = await base.GenerateClaimsAsync(user);

        var plan = await _planRepository.GetBySpecAsync(new PlanWithPermissionsByUserSpec(user));
        var permissionClaims = plan!.Permissions.Select(permission => permission.ToClaim());

        id.AddClaim(plan.ToClaim());
        id.AddClaims(permissionClaims);

        return id;
    }
}