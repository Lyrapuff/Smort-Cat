using System.Threading.Tasks;
using Discord;

namespace AntiStupidStuff.Domain.Services
{
    public interface IUserService
    {
        Task DeleteInviteLinks(IGuild guild, IUser user);
        Task BanAsync(IGuild guild, IUser user, int prune = 0);
    }
}