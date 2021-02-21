using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Services.Persistence;
using SmortCat.Domain.Services.Persistence.Models;
using SmortCat.Domain.Services.Persistence.Repositories;

namespace SmortCat.Core.Services.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _db;
        private IServiceProvider _provider;

        public UnitOfWork(DbContext db, IServiceProvider provider)
        {
            _db = db;
            _provider = provider;
        }

        public IRepository<T> GetRepository<T>() where T : EntityBase
        {
            return _provider.GetService(typeof(T)) as IRepository<T>;
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