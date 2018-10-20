using Autofac;
using PimpMyRide.Core.Data.UnitOfWork;

namespace PimpMyRide.Core.Data.Infrastructure.RegisterModule
{
    public class UnitOfWorkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
