using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordUsers.Persistence.Models;
using DiscordUsers.Persistence.Respositories;
using SmortCat.Domain.Services.Obstacles;
using SmortCat.Domain.Services.Persistence;

namespace DiscordUsers.Obstacles
{
    public class EnsureUserCreatedObstacle : IEarlyObstacle
    {
        private IUnitOfWork _unitOfWork;
        private DiscordUserRepository _repository;

        public EnsureUserCreatedObstacle(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<DiscordUserRepository>();
        }

        private void PopulateUserEntity(DiscordUserEntity userEntity, IUser user)
        {
            userEntity.DiscordId = user.Id;
            userEntity.Username = user.Username;
            userEntity.Discriminator = user.Discriminator;
        }
        
        public async Task<bool> TryPass(CommandContext context)
        {
            DiscordUserEntity userEntity = await _repository.GetByDiscordId(context.User.Id);

            if (userEntity == null)
            {
                userEntity = new DiscordUserEntity();
                PopulateUserEntity(userEntity, context.User);
                
                await _repository.Add(userEntity);
            }
            else
            {
                PopulateUserEntity(userEntity, context.User);

                _repository.Update(userEntity);
            }
            
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }
    }
}