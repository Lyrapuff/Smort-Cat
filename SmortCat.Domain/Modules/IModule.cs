using System;
using Microsoft.Extensions.DependencyInjection;

namespace SmortCat.Domain.Modules
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
        void Start(IServiceProvider provider);
    }
}