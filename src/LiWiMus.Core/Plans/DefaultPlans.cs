using LiWiMus.Core.Permissions;

namespace LiWiMus.Core.Plans;

public static class DefaultPlans
{
    public const string Free = "Free";
    public const string Premium = "Premium";

    public static IEnumerable<Plan> GetAll()
    {
        return new List<Plan>
        {
            new()
            {
                //Id = 1,
                Name = Free,
                PricePerMonth = 0
            },
            new()
            {
                //Id = 2,
                Name = Premium,
                PricePerMonth = 100500,
                //Permissions = DefaultPermissions.GetPublic()
            }
        };
    }
}