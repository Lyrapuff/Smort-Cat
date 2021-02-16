using System.Threading.Tasks;
using AntiStupidStuff.Domain.Services;
using Discord;
using Discord.Commands;

namespace AntiStupidStuff.Commands
{
    public class BanCommand : ModuleBase
    {
        private IBanService _banService;

        public BanCommand(IBanService banService)
        {
            _banService = banService;
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("ban")]
        public async Task Ban(IUser user, int prune = 0)
        {
            await _banService.BanAsync(Context.Guild, user, prune);
        }
    }
}