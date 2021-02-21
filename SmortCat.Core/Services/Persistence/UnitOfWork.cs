using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmortCat.Domain.Services.Persistence;
using SmortCat.Domain.Services.Persistence.Repositories;

namespace SmortCat.Core.Services.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private BotDbContext _db;

        private Dictionary<Type, RepositoryBase> _repositories;

        public UnitOfWork(BotDbContext db)
        {
            _db = db;

            _repositories = new Dictionary<Type, RepositoryBase>();
        }

        public T GetRepository<T>() where T : RepositoryBase
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories[typeof(T)] = Activator.CreateInstance(typeof(T), _db) as RepositoryBase;
            }

            return _repositories[typeof(T)] as T;
        }

        public async ValueTask SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _db.DisposeAsync();
        }
    }
}