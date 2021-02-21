using System.Threading.Tasks;
using Discord.Commands;
using SmortCat.Domain.Services.Obstacles;
using SmortCat.Domain.Services.Persistence;

namespace DiscordUsers.Obstacles
{
    public class EnsureUserCreatedObstacle : IEarlyObstacle
    {
        private IUnitOfWork _unitOfWork;

        public EnsureUserCreatedObstacle(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<bool> TryPass(CommandContext context)
        {
            return true;
        }
    }
}