using Autofac;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Plans;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Services;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Infrastructure;

public class ConfigurationCoreModule : Module
{
    private readonly string _contentRootPath;

    public ConfigurationCoreModule(string contentRootPath)
    {
        _contentRootPath = contentRootPath;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EfRepository<>))
            .As(typeof(IReadRepository<>), typeof(IRepository<>))
            .InstancePerLifetimeScope();
            
        builder.RegisterType<AvatarService>().As<IAvatarService>();
        builder.RegisterType<MailService>().As<IMailService>();
        builder.RegisterType<MailRequestService>().As<IMailRequestService>();
        builder.RegisterType<ImageService>().As<IImageService>();
        builder.RegisterType<PaymentService>().As<IPaymentService>();
        builder.RegisterType<UserPlanManager>().As<IUserPlanManager>();
        AvatarService.Configure(_contentRootPath);
    }
}