using System;
using Administration.Services;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;

namespace Administration
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BanService>();
        }

        public void Start(IServiceProvider provider)
        {
            
        }
    }
}