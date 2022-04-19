using LiWiMus.Core.Permissions;

namespace LiWiMus.Core.Plans;

public static class DefaultPlans
{
    public static readonly Plan Free = new("Free", decimal.Zero);
    public static readonly Plan Premium = new("Premium", 100);

    public static Dictionary<Plan, IEnumerable<string>> GetPlansWithPermissions()
    {
        return new Dictionary<Plan, IEnumerable<string>>
        {
            [Free] = new List<string> { },
            [Premium] = new List<string> { }
        };
    }
}