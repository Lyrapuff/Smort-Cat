using System;
using AntiStupidStuff.Domain.Services;
using AntiStupidStuff.Services;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services;

namespace AntiStupidStuff
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<PingRestrictor>();
        }

        public void Start(IServiceProvider provider)
        {
            ILogger logger = provider.GetService<ILogger>();

            logger.LogInformation("AntiStupidStuff started!");
        }
    }
}