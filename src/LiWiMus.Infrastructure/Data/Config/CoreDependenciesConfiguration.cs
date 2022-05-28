using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Plans.Interfaces;
using LiWiMus.Core.Roles.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Infrastructure.Services;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Infrastructure.Data.Config;

public static class CoreDependenciesConfiguration
{
    public static IServiceCollection AddRepositoriesAndManagers(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddTransient<IPlanManager, PlanManager>();
        services.AddTransient<IRoleManager, RoleManager>();
        services.AddTransient<IUserPlanManager, UserPlanManager>();
        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddRepositoriesAndManagers();
        services.AddTransient<IAvatarService, AvatarService>();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<IUserValidator<User>, ApplicationUserValidator>();
        // TODO: Add authorization handler
        return services;
    }
}