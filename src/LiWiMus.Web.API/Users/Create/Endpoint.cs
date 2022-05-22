using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Users;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Users.Create;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private UserManager<User> _userManager = null!;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var user = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        return !result.Succeeded
            ? Results.UnprocessableEntity(result.Errors)
            : Results.NoContent();
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.Users.Create, async (Request request, UserManager<User> userManager) =>
        {
            _userManager = userManager;
            return await HandleAsync(request);
        });
    }
}