using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmortCat.Domain.Persistence;

namespace DiscordUsers.Features
{
    public class CreateUser : IRequest
    {
        public IUser User { get; }

        public CreateUser(IUser user)
        {
            User = user;
        }
    }

    public class CreateUserHandler : AsyncRequestHandler<CreateUser>
    {
        private BotDbContext _db;

        public CreateUserHandler(BotDbContext db)
        {
            _db = db;
        }
        
        protected override async Task Handle(CreateUser request, CancellationToken cancellationToken)
        {
            DiscordUserEntity userEntity = new ()
            {
                DiscordId = request.User.Id,
                Username = request.User.Username,
                Discriminator = request.User.Discriminator
            };
            
            DbSet<DiscordUserEntity> set = _db.Set<DiscordUserEntity>();

            await set.AddAsync(userEntity, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}