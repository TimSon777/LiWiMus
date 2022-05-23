namespace LiWiMus.Core.Plans;

public static class DefaultPlans
{
    public static class Free
    {
        public const string Name = nameof(Free);
        public const string Description = "Free plan";
        public const int PricePerMonth = 0;
    }

    public static class Premium
    {
        public const string Name = nameof(Premium);
        public const string Description = "Premium plan";
        public const int PricePerMonth = 100;
    }

    public static IEnumerable<Plan> GetAll()
    {
        yield return new Plan
        {
            Name = Free.Name,
            Description = Free.Description,
            PricePerMonth = Free.PricePerMonth
        };

        yield return new Plan
        {
            Name = Premium.Name,
            Description = Premium.Description,
            PricePerMonth = Premium.PricePerMonth
        };
    }
}