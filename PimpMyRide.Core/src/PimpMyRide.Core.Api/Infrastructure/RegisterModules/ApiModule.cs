using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PimpMyRide.Core.Api.Infrastructure.Middleware;
using PimpMyRide.Core.Data.Models;

namespace PimpMyRide.Core.Api.Infrastructure.RegisterModules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<TokenManagerMiddleware>().AsSelf().InstancePerDependency();
        }
    }
}
