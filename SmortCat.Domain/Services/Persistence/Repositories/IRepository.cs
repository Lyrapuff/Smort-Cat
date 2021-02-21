using System.Threading.Tasks;
using SmortCat.Domain.Services.Persistence.Models;

namespace SmortCat.Domain.Services.Persistence.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetById(int id);
        Task Add(T entity);
    }
}