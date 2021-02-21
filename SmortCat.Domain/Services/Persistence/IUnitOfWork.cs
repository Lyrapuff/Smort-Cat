using System;
using System.Threading.Tasks;
using SmortCat.Domain.Services.Persistence.Repositories;

namespace SmortCat.Domain.Services.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        T GetRepository<T>() where T : RepositoryBase;
        ValueTask SaveChangesAsync();
    }
}