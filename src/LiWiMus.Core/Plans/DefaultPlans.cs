namespace LiWiMus.Core.Plans;

public static class DefaultPlans
{
    public static readonly Plan Free = new()
    {
        Name = nameof(Free),
        Description = "Free plan",
        PricePerMonth = 0
    };

    public static readonly Plan Premium = new()
    {
        Name = nameof(Premium),
        Description = "Premium plan",
        PricePerMonth = 100
    };

    public static IEnumerable<Plan> GetAll()
    {
        return new[] {Free, Premium};
    }
}