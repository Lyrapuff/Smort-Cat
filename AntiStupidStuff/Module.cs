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
            services.AddSingleton<IBanService, BanService>();
            services.AddSingleton<IPingRestrictor, PingRestrictor>();
        }

        public void Start(IServiceProvider provider)
        {
            ILogger logger = provider.GetService<ILogger>();

            logger.LogInformation("AntiStupidStuff started!");

            IPingRestrictor pingRestrictor = provider.GetService<IPingRestrictor>();
            pingRestrictor.Start();
        }
    }
}