using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Exceptions;
using LiWiMus.Core.Roles.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Users.Roles.Remove;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<User> _userRepository = null!;
    private IRepository<Role> _roleRepository = null!;
    private IRoleManager _roleManager = null!;

    public async Task<IResult> HandleAsync(Request request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Users, request.UserId);
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Roles, request.RoleId);
        }

        try
        {
            await _roleManager.RemoveFromRoleAsync(user, role);
            return Results.Ok();
        }
        catch (UserNotInRoleException)
        {
            return Results.BadRequest("User not this in role");
        }
        catch (RemoveFromAdminRoleException)
        {
            return Results.UnprocessableEntity("Can't remove user from admin role");
        }
        catch (RemoveFromDefaultRoleException)
        {
            return Results.UnprocessableEntity("Can't remove user from default role");
        }
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(RouteConstants.Users.Roles.Remove,
            async ([FromBody] Request request, IRepository<User> userRepository, IRepository<Role> roleRepository,
                   IRoleManager roleManager) =>
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _roleManager = roleManager;
                return await HandleAsync(request);
            });
    }
}