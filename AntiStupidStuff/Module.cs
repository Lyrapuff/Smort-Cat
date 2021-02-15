using System;
using AntiStupidStuff.Services;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services;

namespace AntiStupidStuff
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PingRestrictor>();
        }

        public void Start(IServiceProvider provider)
        {
            ILogger logger = provider.GetService<ILogger>();

            logger.LogInformation("AntiStupidStuff started!");

            DiscordSocketClient client = provider.GetService<DiscordSocketClient>();

            PingRestrictor pingRestrictor = provider.GetService<PingRestrictor>();

            client.MessageReceived += pingRestrictor.OnMessage;
        }
    }
}