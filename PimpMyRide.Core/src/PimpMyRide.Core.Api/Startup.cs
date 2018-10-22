using System;
using System.Linq;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PimpMyRide.Core.Data.Context;
using Swashbuckle.AspNetCore.Swagger;

namespace PimpMyRide.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddDbContext<PimpMyRideDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureDB")));
            services.AddAutoMapper();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("PMR-Core", new Info
                {
                    Version = "1.0",
                    Title = "PMR",
                    Description = "PMR - Swagger API Documentation"
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("PimpMyRide")).ToArray();
            builder.RegisterAssemblyModules(assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                options => { options.SwaggerEndpoint("/swagger/PMR-Core/swagger.json", "Pimp My Ride"); });
        }
    }
}
