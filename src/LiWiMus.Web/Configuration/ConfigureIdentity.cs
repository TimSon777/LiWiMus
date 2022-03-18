using LiWiMus.Core.Entities;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.Configuration;

public static class ConfigureIdentity
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;

                if (environment.IsDevelopment())
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                }
                else
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.User.RequireUniqueEmail = true;
                }
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationContext>();

        return services;
    }
}