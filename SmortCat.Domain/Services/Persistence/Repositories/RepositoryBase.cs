using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Services.Persistence.Models;

namespace SmortCat.Domain.Services.Persistence.Repositories
{
    public abstract class RepositoryBase<T> : RepositoryBase, IRepository<T> where T : EntityBase
    {
        protected DbSet<T> _set;
        
        protected RepositoryBase(DbContext db) : base(db)
        {
            _set = _db.Set<T>();
        }
        
        public async Task<T> GetById(int id)
        {
            return await _set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Add(T entity)
        {
            await _set.AddAsync(entity);
        }

        public void Update(T entity)
        { 
            _set.Update(entity);
        }
    }

    public abstract class RepositoryBase
    {
        protected DbContext _db;

        public RepositoryBase(DbContext db)
        {
            _db = db;
        }
    }
}