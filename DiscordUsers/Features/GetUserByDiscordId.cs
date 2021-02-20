using System.Threading;
using System.Threading.Tasks;
using DiscordUsers.Models;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Persistence;

namespace DiscordUsers.Features
{
    public class GetUserByDiscordId : IRequest<Result<DiscordUserEntity>>
    {
        public ulong DiscordId { get; }

        public GetUserByDiscordId(ulong discordId)
        {
            DiscordId = discordId;
        }
    }

    public class GetUserByDiscordIdHandler : IRequestHandler<GetUserByDiscordId, Result<DiscordUserEntity>>
    {
        private BotDbContext _db;

        public GetUserByDiscordIdHandler(BotDbContext db)
        {
            _db = db;
        }
        
        public async Task<Result<DiscordUserEntity>> Handle(GetUserByDiscordId request, CancellationToken cancellationToken)
        {
            DbSet<DiscordUserEntity> set = _db.Set<DiscordUserEntity>();
            
            DiscordUserEntity userEntity = await set
                .FirstOrDefaultAsync(e => e.DiscordId == request.DiscordId, cancellationToken);
            
            return userEntity != null ? Result.Ok(userEntity) : Result.Fail("");
        }
    }
}