using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Modules;
using SmortCat.Domain.Services;

namespace SmortCat.Domain.Persistence
{
    public class BotDbContext : DbContext
    {
        private IModuleLoader _moduleLoader;
        
        public BotDbContext(DbContextOptions<BotDbContext> options, IModuleLoader moduleLoader) : base(options)
        {
            _moduleLoader = moduleLoader;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadEntitiesFromModules(modelBuilder);
        }

        private void LoadEntitiesFromModules(ModelBuilder modelBuilder)
        {
            foreach (IModule module in _moduleLoader.Modules)
            {
                Assembly assembly = module.GetType().Assembly;

                IEnumerable<Type> entityTypes = assembly
                    .GetTypes()
                    .Where(x => x.BaseType == typeof(EntityBase));
                
                foreach (Type entityType in entityTypes)
                {
                    modelBuilder.Entity(entityType);
                }
            }
        }
    }
}