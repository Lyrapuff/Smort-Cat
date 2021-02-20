using System.Threading.Tasks;
using Discord.Commands;
using DiscordUsers.Features;
using DiscordUsers.Models;
using FluentResults;
using MediatR;
using SmortCat.Domain.Services;

namespace DiscordUsers.ExecutionObstacles
{
    public class EnsureUserCreatedObstacle : IExecutionObstacle
    {
        private IMediator _mediator;

        public EnsureUserCreatedObstacle(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<bool> TryPass(CommandContext context)
        {
            Result<DiscordUserEntity> result = await _mediator.Send(new GetUserByDiscordId(context.User.Id));

            if (result.IsFailed)
            {
                await _mediator.Send(new CreateUser(context.User));
            }
            
            return true;
        }
    }
}