using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;

namespace LiWiMus.Web.API.UserPlans.List;

public class Endpoint : IEndpoint<IEnumerable<Dto>, Request>
{
    private IRepository<UserPlan> _userPlanRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<Dto>> HandleAsync(Request request)
    {
        var userPlans = await _userPlanRepository.SearchAsync(request.UserId, request.PlanId, request.Active);
        var dto = _mapper.MapList<UserPlan, Dto>(userPlans).ToList();
        return dto;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.UserPlans.List,
            async (int? userId, int? planId, bool? active, IRepository<UserPlan> userPlanRepository) =>
            {
                _userPlanRepository = userPlanRepository;

                var request = new Request(userId, planId, active);
                return await HandleAsync(request);
            });
    }
}