using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Interfaces;

public interface IPlanService
{
    Task SetDefaultPlanAsync(User user);
}