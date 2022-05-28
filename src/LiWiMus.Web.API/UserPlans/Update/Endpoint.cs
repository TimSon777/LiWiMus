using AutoMapper;
using FluentValidation;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Exceptions;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.UserPlans.Update;

public class Endpoint : IEndpoint<IResult, int, Request>
{
    private IRepository<UserPlan> _repository = null!;
    private IUserPlanManager _planManager = null!;
    private readonly IValidator<Request> _validator;
    private readonly IMapper _mapper;

    public Endpoint(IValidator<Request> validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id, Request request)
    {
        var userPlan = await _repository.GetAsync(id);
        if (userPlan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.UserPlans, id);
        }

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        _mapper.Map(request, userPlan);
        try
        {
            await _planManager.UpdateUserPlanAsync(userPlan);
        }
        catch (DefaultUserPlanIsReadonlyException)
        {
            return Results.UnprocessableEntity("Default user plan is readonly");
        }

        var dto = _mapper.Map<Dto>(userPlan);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPatch(RouteConstants.UserPlans.Update,
            async (int id, Request request, IRepository<UserPlan> repository, IUserPlanManager planManager) =>
            {
                _planManager = planManager;
                _repository = repository;
                return await HandleAsync(id, request);
            });
    }
}