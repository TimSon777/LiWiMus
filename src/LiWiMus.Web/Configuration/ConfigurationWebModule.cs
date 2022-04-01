using Autofac;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.Configuration;

public class ConfigurationWebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RazorViewRenderer>().As<IRazorViewRenderer>();
        builder.RegisterType<PermissionPolicyProvider>().As<IAuthorizationPolicyProvider>().SingleInstance();
        builder.RegisterType<AuthorizationHandler>().As<IAuthorizationHandler>().InstancePerLifetimeScope();
        builder.RegisterType<ApplicationUserValidator>().As<IUserValidator<User>>();
    }
}