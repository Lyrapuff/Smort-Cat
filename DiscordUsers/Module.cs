using System;
using DiscordUsers.ExecutionObstacles;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services;

namespace DiscordUsers
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IExecutionObstacle, EnsureUserCreatedObstacle>();
        }

        public void Start(IServiceProvider provider)
        {
            
        }
    }
}