using System.Security.Claims;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Entities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Infrastructure.Identity;

public class AuthorizationHandler : IAuthorizationHandler
{
    private readonly UserManager<User> _userManager;

    public AuthorizationHandler(UserManager<User> userManager)
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
                                                    SameAuthorRequirement sameAuthorRequirement, User user)
    {
        switch (context.Resource)
        {
            case ISingleOwnerResource singleOwnerResource
                when user.Id == singleOwnerResource.UserId:
            case ISingleArtistOwnerResource singleArtistOwnerResource
                when user.ArtistId is not null && user.ArtistId == singleArtistOwnerResource.Artist.Id:
            case IMultipleOwnersResource multipleOwnersResource
                when multipleOwnersResource.Users.Select(u => u.Id).Contains(user.Id):
            case IMultipleArtistOwnersResource multipleArtistOwnersResource
                when user.ArtistId is not null && multipleArtistOwnersResource.Artists.Select(a => a.Id)
                                                                              .Contains(user.ArtistId.Value):
                context.Succeed(sameAuthorRequirement);
                break;
        }
    }
}