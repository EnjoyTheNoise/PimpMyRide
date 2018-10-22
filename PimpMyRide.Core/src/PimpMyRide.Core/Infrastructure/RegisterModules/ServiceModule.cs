using Autofac;
using PimpMyRide.Core.Cars;

namespace PimpMyRide.Core.Infrastructure.RegisterModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CarService>().As<ICarService>().InstancePerDependency();
        }
    }
}
