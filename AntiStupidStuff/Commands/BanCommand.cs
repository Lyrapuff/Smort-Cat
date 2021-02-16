using System.Threading.Tasks;
using AntiStupidStuff.Domain.Services;
using Discord;
using Discord.Commands;

namespace AntiStupidStuff.Commands
{
    public class BanCommand : ModuleBase
    {
        private IUserService _userService;

        public BanCommand(IUserService userService)
        {
            _userService = userService;
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("ban")]
        public async Task Ban(IUser user, int prune = 0)
        {
            await _userService.DeleteInviteLinks(Context.Guild, user);
            await _userService.BanAsync(Context.Guild, user, prune);
        }
    }
}