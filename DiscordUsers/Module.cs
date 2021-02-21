using System;
using DiscordUsers.Obstacles;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services.Obstacles;

namespace DiscordUsers
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEarlyObstacle, EnsureUserCreatedObstacle>();
        }

        public void Start(IServiceProvider provider)
        {
            
        }
    }
}