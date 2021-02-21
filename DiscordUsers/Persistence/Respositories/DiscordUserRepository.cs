using System.Threading.Tasks;
using DiscordUsers.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Services.Persistence.Repositories;

namespace DiscordUsers.Persistence.Respositories
{
    public class DiscordUserRepository : RepositoryBase<DiscordUserEntity>
    {
        public DiscordUserRepository(DbContext db) : base(db)
        {
        }

        public Task<DiscordUserEntity> GetByDiscordId(ulong discordId)
        {
            return _set.FirstOrDefaultAsync(e => e.DiscordId == discordId);
        }
    }
}