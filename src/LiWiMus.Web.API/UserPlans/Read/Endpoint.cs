using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.UserPlans.Read;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<UserPlan> _repository = null!;
    private IUserPlanManager _userPlanManager = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var userPlan = await _repository.GetAsync(id);
        if (userPlan is null)
        {
            return Results.Extensions.NotFoundById(EntityType.UserPlans, id);
        }

        var dto = _mapper.Map<Dto>(userPlan);
        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.UserPlans.Read,
            async (int id, IRepository<UserPlan> repository, IUserPlanManager userPlanManager) =>
            {
                _repository = repository;
                _userPlanManager = userPlanManager;
                return await HandleAsync(id);
            });
    }
}