using LiWiMus.Core.Permissions;
using LiWiMus.Core.Plans;
using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Infrastructure.Data.Seed;

public static class PlansSeeder
{
    public static async Task SeedPlansAndPermissionsAsync(this ApplicationContext context)
    {
        if (await context.Plans.AnyAsync() || await context.Permissions.AnyAsync())
        {
            return;
        }

        var plans = DefaultPlans.GetAll().ToList();
        await context.Plans.AddRangeAsync(plans);

        await context.Permissions.AddRangeAsync(DefaultPermissions.GetPrivate());

        await context.SaveChangesAsync();
    }
}