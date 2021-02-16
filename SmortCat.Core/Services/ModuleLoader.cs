using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services;

namespace SmortCat.Core.Services
{
    public class ModuleLoader : IModuleLoader
    {
        public List<IModule> Modules { get; private set; }

        private ILogger _logger;
        private CommandService _commandService;

        public ModuleLoader(ILogger logger, CommandService commandService)
        {
            _logger = logger;
            _commandService = commandService;
        }

        public void Load(IServiceCollection services)
        {
            _logger.LogInformation("Loading modules.");
            
            List<IModule> modules = new List<IModule>();
            
            const string path = "Modules";

            if (!Directory.Exists(path))
            {
                _logger.LogError("Can't load modules, no Modules directory found.");
                return;
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

            Modules = modules;
        }

        public async Task Start(IServiceProvider provider)
        {
            _logger.LogInformation("Starting modules.");

            foreach (IModule module in Modules)
            {
                await _commandService.AddModulesAsync(module.GetType().Assembly, provider);
                module.Start(provider);
            }

            _logger.LogInformation($"Started {Modules.Count} modules.");
        }
    }
}