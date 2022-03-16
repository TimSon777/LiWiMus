using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Specifications;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Services;

public class PlanService : IPlanService
{
    private readonly IRepository<Plan> _planRepository;
    private readonly IRepository<UserPlan> _userPlanRepository;

    public PlanService(IRepository<Plan> planRepository, IRepository<UserPlan> userPlanRepository)
    {
        _planRepository = planRepository;
        _userPlanRepository = userPlanRepository;
    }

    public async Task SetDefaultPlanAsync(User user)
    {
        var defaultPlan = await _planRepository.GetBySpecAsync(new DefaultPlanSpecification()) ??
                          throw new SystemException();
        var userPlan = new UserPlan
        {
            Plan = defaultPlan,
            Start = DateTime.Now,
            End = DateTime.MaxValue
        };
        await _userPlanRepository.AddAsync(userPlan);
        user.UserPlan = userPlan;
    }
}