using System.Threading.Tasks;
using Discord;

namespace AntiStupidStuff.Domain.Services
{
    public interface IBanService
    {
        Task BanAsync(IGuild guild, IUser user, int prune = 0);
    }
}