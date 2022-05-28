using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Users.LockOut;

public class Endpoint : IEndpoint<IResult, string, Request>
{
    private UserManager<User> _userManager = null!;
    private IRoleManager _roleManager = null!;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(string id, Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var user = await _userManager.FindByIdAsync(id);

        if (await _roleManager.IsInRoleAsync(user, DefaultRoles.Admin.Name))
        {
            return Results.UnprocessableEntity("Can't ban admin");
        }
        
        if (!await _userManager.GetLockoutEnabledAsync(user))
        {
            var enabledAsync = await _userManager.SetLockoutEnabledAsync(user, true);
            if (!enabledAsync.Succeeded)
            {
                return Results.BadRequest(enabledAsync);
            }
        }

        var endDateResult = await _userManager.SetLockoutEndDateAsync(user, request.End);
        if (!endDateResult.Succeeded)
        {
            return Results.BadRequest(endDateResult);
        }

        var dto = _mapper.Map<Dto>(user);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Users.LockOut,
            async (string id, Request request, UserManager<User> userManager, IRoleManager roleManager) =>
        {
            _roleManager = roleManager;
            _userManager = userManager;
            return await HandleAsync(id, request);
        });
    }
}