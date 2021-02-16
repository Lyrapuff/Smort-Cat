using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
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
            List<IModule> modules = LoadModules(services);
            IServiceProvider provider = services.BuildServiceProvider();
            await StartModules(modules, provider);
            
            provider
                .GetService<ICommandHandler>()?
                .Start();
        }

        private IServiceCollection CreateServices()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddSingleton(_logger)
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddSingleton(_botCredentialsProvider)
                .AddSingleton<ICommandHandler, CommandHandler>();
            
            return services;
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
        
        private List<IModule> LoadModules(IServiceCollection services)
        {
            _logger.LogInformation("Loading modules.");
            
            List<IModule> modules = new List<IModule>();
            
            const string path = "Modules";

            if (!Directory.Exists(path))
            {
                _logger.LogError("Can't load modules, no Modules directory found.");
                return modules;
            }

            DirectoryInfo directory = new (path);
            
            foreach (FileInfo file in directory.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);

                Type moduleType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.GetInterface("IModule") != null);

                if (moduleType == null)
                {
                    _logger.LogWarning($"No module types found in {assembly.FullName} assembly.");
                    continue;
                }
                
                IModule module = Activator.CreateInstance(moduleType) as IModule;

                if (module == null)
                {
                    continue;
                }
                
                module.ConfigureServices(services);
                modules.Add(module);
            }

            _logger.LogInformation($"Loaded {modules.Count} modules.");
            
            return modules;
        }

        private async Task StartModules(List<IModule> modules, IServiceProvider provider)
        {
            _logger.LogInformation("Starting modules.");

            foreach (IModule module in modules)
            {
                await _commandService.AddModulesAsync(module.GetType().Assembly, provider);
                module.Start(provider);
            }

            _logger.LogInformation($"Started {modules.Count} modules.");
        }
    }
}