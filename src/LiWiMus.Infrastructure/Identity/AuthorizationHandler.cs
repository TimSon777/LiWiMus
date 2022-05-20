using System.Security.Claims;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Shared.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Identity;

public class AuthorizationHandler : IAuthorizationHandler
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;

    public AuthorizationHandler(UserManager<User> userManager, IRepository<User> userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();
        if (context.User.Identity?.Name is null)
        {
            return;
        }
        
        var user = await _userRepository.GetBySpecAsync(
            new UserWithArtistsByNameSpec(context.User.Identity.Name));

        if (user is null)
        {
            return;
        }

        foreach (var requirement in pendingRequirements)
        {
            switch (requirement)
            {
                case PermissionRequirement permissionRequirement:
                {
                    var userClaims = context.User.Claims;
                    HandlePermissionRequirement(context, permissionRequirement, userClaims);
                    break;
                }

                case SameAuthorRequirement sameAuthorRequirement:
                {
                    HandleSameAuthorRequirement(context, sameAuthorRequirement, user);
                    break;
                }
            }
        }
    }

    private static void HandlePermissionRequirement(AuthorizationHandlerContext context,
                                                    PermissionRequirement permissionRequirement,
                                                    IEnumerable<Claim> userClaims)
    {
        var permissions = userClaims.Where(x => x.Type == Permission.ClaimType &&
                                                x.Value == permissionRequirement.Permission &&
                                                x.Issuer == "LOCAL AUTHORITY");
        if (!permissions.Any())
        {
            return;
        }

        context.Succeed(permissionRequirement);
    }

    private static void HandleSameAuthorRequirement(AuthorizationHandlerContext context,
                                                    SameAuthorRequirement sameAuthorRequirement, User user)
    {
        switch (context.Resource)
        {
            case IResource.WithOwner<User> singleOwnerResource
                when user.Id == singleOwnerResource.Owner.Id:
            case IResource.WithOwner<Artist> singleArtistOwnerResource
                when user.Artists.Select(a => a.Id).Contains(singleArtistOwnerResource.Owner.Id):
            case IResource.WithMultipleOwners<User> multipleOwnersResource
                when multipleOwnersResource.Owners.Select(u => u.Id).Contains(user.Id):
            case IResource.WithMultipleOwners<Artist> multipleArtistOwnersResource
                when multipleArtistOwnersResource.Owners.Select(a => a.Id)
                                                 .Intersect(user.Artists.Select(a => a.Id)).Any():
                context.Succeed(sameAuthorRequirement);
                break;
        }
    }
}