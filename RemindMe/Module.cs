using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;

namespace RemindMe
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Bumpers>();
        }

        public void Start(IServiceProvider provider)
        {
            Task.Run(provider.GetService<Bumpers>().Start);
        }
    }
}