using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Infrastructure.Services;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Infrastructure.Data.Config;

public static class CoreDependenciesConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddTransient<IAvatarService, AvatarService>();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<IUserPlanManager, UserPlanManager>();
        services.AddTransient<IUserValidator<User>, ApplicationUserValidator>();
        services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
        return services;
    }
}