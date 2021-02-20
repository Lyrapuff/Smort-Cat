using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Persistence;
using SmortCat.Domain.Services;

namespace SmortCat.Core.Services
{
    public class BotStarter : IBotStarter
    {
        private ILogger _logger;
        private IBotCredentialsProvider _botCredentialsProvider;
        
        private DiscordSocketClient _client;
        private CommandService _commandService;
        
        public async Task Start()
        {
            _logger = new Logger();
            _botCredentialsProvider = new BotCredentialsProvider(_logger);
            
            await LogIntoDiscord();
            
            IServiceCollection services = CreateServices();

            IModuleLoader moduleLoader = new ModuleLoader(_logger, _commandService);
            moduleLoader.Load(services);

            AddMediatR(services, moduleLoader);
            
            services.AddSingleton(moduleLoader);

            IServiceProvider provider = services.BuildServiceProvider();

            provider
                .GetService<BotDbContext>()?
                .Database.EnsureCreated();
            
            provider
                .GetService<ICommandHandler>()?
                .Start();
            
            await moduleLoader.Start(provider);
        }

        private async Task LogIntoDiscord()
        {
            _client = new DiscordSocketClient();
            
            _commandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });

            string discordToken = _botCredentialsProvider.GetCredentials().DiscordToken;
            
            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();
        }
        
        private IServiceCollection CreateServices()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddSingleton(_logger)
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddSingleton(_botCredentialsProvider)
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddDbContext<BotDbContext>(o =>
                {
                    o.UseSqlite(_botCredentialsProvider.GetCredentials().DbConnectionString);
                });
            
            return services;
        }

        private void AddMediatR(IServiceCollection services, IModuleLoader moduleLoader)
        {
            Assembly[] moduleAssemblies = moduleLoader.Modules
                .Select(m => m.GetType().Assembly)
                .ToArray();
            
            services.AddMediatR(moduleAssemblies);
        }
    }
}