﻿using Autofac;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.MVC.Configuration;

public class ConfigurationWebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RazorViewRenderer>().As<IRazorViewRenderer>();
        builder.RegisterType<AuthorizationHandler>().As<IAuthorizationHandler>().InstancePerLifetimeScope();
        builder.RegisterType<ApplicationUserValidator>().As<IUserValidator<User>>();
    }
}