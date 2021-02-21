using System.Threading.Tasks;
using Administration.Services;
using Discord;
using Discord.Commands;
using FluentResults;

namespace Administration.Commands
{
    public class BanCommands : ModuleBase
    {
        private BanService _ban;

        public BanCommands(BanService ban)
        {
            _ban = ban;
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("ban")]
        public async Task Ban(IUser user)
        {
            Result result = await _ban.BanUserAsync(user, Context.Guild);

            if (result.IsSuccess)
            {
                await ReplyAsync($"Banned {user.Username}. owo");
            }
            else
            {
                await ReplyAsync($"Can't ban {user.Username}. p.p");
            }
        }
    }
}