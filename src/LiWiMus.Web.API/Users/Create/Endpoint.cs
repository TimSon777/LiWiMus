using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Users;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.Users.Create;

// ReSharper disable once UnusedType.Global
public class Endpoint : IEndpoint<IResult, Request>
{
    private readonly IValidator<Request> _validator;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, 
        UserManager<User> userManager, 
        IMapper mapper)
    {
        _validator = validator;
        _userManager = userManager;
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
        app.MapPost("/api/users", HandleAsync);
    }
}