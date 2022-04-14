using System.Security.Claims;
using LiWiMus.Core.Artists;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Shared.Interfaces;
using LiWiMus.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Identity;

public class AuthorizationHandler : IAuthorizationHandler
{
    private readonly UserManager<UserIdentity> _userManager;

    public AuthorizationHandler(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();
        var user = await _userManager.GetUserAsync(context.User);

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
                                                    PermissionRequirement permissionRequirement, IEnumerable<Claim> userClaims)
    {
        var permissions = userClaims.Where(x => x.Type == Permissions.ClaimType &&
                                                         x.Value == permissionRequirement.Permission &&
                                                         x.Issuer == "LOCAL AUTHORITY");
        if (!permissions.Any())
        {
            return;
        }

        context.Succeed(permissionRequirement);
    }

    private static void HandleSameAuthorRequirement(AuthorizationHandlerContext context,
                                                    SameAuthorRequirement sameAuthorRequirement, UserIdentity userIdentity)
    {
        switch (context.Resource)
        {
            case IResource.WithOwner<User> singleOwnerResource
                when userIdentity.Aggregate.User != null && userIdentity.Aggregate.User.Id == singleOwnerResource.Owner.Id:
            case IResource.WithOwner<Artist> singleArtistOwnerResource
                when userIdentity.Aggregate.Artist is not null && userIdentity.Aggregate.Artist.Id == singleArtistOwnerResource.Owner.Id:
            case IResource.WithMultipleOwners<User> multipleOwnersResource
                when multipleOwnersResource.Owners.Select(u => u.Id).Contains(userIdentity.Id):
            case IResource.WithMultipleOwners<Artist> multipleArtistOwnersResource
                when userIdentity.Aggregate.Artist is not null && multipleArtistOwnersResource.Owners.Select(a => a.Id)
                                                                              .Contains(userIdentity.Aggregate.Artist.Id):
                context.Succeed(sameAuthorRequirement);
                break;
        }
    }
}