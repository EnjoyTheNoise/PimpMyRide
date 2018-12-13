using Autofac;
using PimpMyRide.Core.Cars;
using PimpMyRide.Core.RentCars;
using PimpMyRide.Core.Tokens;
using PimpMyRide.Core.Users;

namespace PimpMyRide.Core.Infrastructure.RegisterModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CarService>().As<ICarService>().InstancePerDependency();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerDependency();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterType<TokenManager>().As<ITokenManager>().InstancePerDependency();
            builder.RegisterType<RentCarService>().As<IRentCarService>().InstancePerDependency();
        }
    }
}
