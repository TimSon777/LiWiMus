using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure.Services;

public class UserPlanManager : IUserPlanManager
{
    private readonly IRepository<UserPlan> _userPlanRepository;
    private readonly IRepository<Plan> _planRepository;

    public UserPlanManager(IRepository<UserPlan> userPlanRepository, IRepository<Plan> planRepository)
    {
        _userPlanRepository = userPlanRepository;
        _planRepository = planRepository;
    }

    public async Task<bool> IsInPlan(User user, string planName)
    {
        var up = await GetUserPlan(user, planName);
        return IsInPlan(up);
    }

    public async Task AddToPlan(User user, string planName, TimeSpan time)
    {
        var up = await GetUserPlan(user, planName);
        if (IsInPlan(up))
        {
            up!.End += time;
            await _userPlanRepository.UpdateAsync(up);
            return;
        }

        var plan = await _planRepository.GetBySpecAsync(new PlanByNameSpec(planName));

        var userPlan = new UserPlan
        {
            User = user,
            Plan = plan,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddOrMaximize(time)
        };
        await _userPlanRepository.AddAsync(userPlan);
    }

    private Task<UserPlan?> GetUserPlan(User user, string planName)
    {
        return _userPlanRepository.GetBySpecAsync(new UserPlanByUserAndPlanNameSpec(user, planName));
    }

    private bool IsInPlan(UserPlan? userPlan)
    {
        return userPlan is not null && userPlan.End > DateTime.UtcNow;
    }
}