using System.Threading.Tasks;
using Administration.Features;
using Discord;
using Discord.Commands;
using FluentResults;
using MediatR;

namespace Administration
{
    public class Commands : ModuleBase
    {
        private IMediator _mediator;

        public Commands(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("ban")]
        public async Task Ban(IUser user)
        {
            Result result = await _mediator.Send(new BanUser(user, Context.Guild));

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