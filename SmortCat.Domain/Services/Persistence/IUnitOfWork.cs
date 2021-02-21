using System;
using System.Threading.Tasks;
using SmortCat.Domain.Services.Persistence.Models;
using SmortCat.Domain.Services.Persistence.Repositories;

namespace SmortCat.Domain.Services.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : EntityBase;
        ValueTask SaveChangesAsync();
    }
}