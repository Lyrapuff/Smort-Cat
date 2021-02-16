using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmortCat.Domain.Modules;

namespace SmortCat.Domain.Services
{
    public interface IModuleLoader
    {
        List<IModule> Modules { get; }
        
        void Load(IServiceCollection services);
        Task Start(IServiceProvider provider);
    }
}