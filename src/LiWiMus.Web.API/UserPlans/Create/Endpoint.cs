using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Exceptions;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.UserPlans.Create;

public class Endpoint : IEndpoint<IResult, Request>
{
    private IRepository<User> _userRepository = null!;
    private IRepository<Plan> _planRepository = null!;
    private IUserPlanManager _planManager = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(Request request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Users, request.UserId);
        }

        var plan = await _planRepository.GetByIdAsync(request.PlanId);
        if (plan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Plans, request.PlanId);
        }

        try
        {
            var userPlan = await _planManager.AddToPlanAsync(user, plan, request.Start, request.End);

            var dto = _mapper.Map<Dto>(userPlan);
            dto.Updatable = true;
            return Results.Ok(dto);
        }
        catch (UserAlreadyOwnsPlanException)
        {
            return Results.Conflict("User already owns this plan");
        }
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConstants.UserPlans.Create,
            async (Request request, IRepository<User> userRepository, IRepository<Plan> planRepository,
                   IUserPlanManager planManager) =>
            {
                _userRepository = userRepository;
                _planRepository = planRepository;
                _planManager = planManager;
                return await HandleAsync(request);
            });
    }
}