using System.Collections.Generic;
using System.Threading.Tasks;
using AntiStupidStuff.Domain.Services;
using Discord;
using SmortCat.Domain.Services;

namespace AntiStupidStuff.Services
{
    public class BanService : IBanService
    {
        private ILogger _logger;

        public BanService(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task BanAsync(IGuild guild, IUser user, int prune = 0)
        {
            IReadOnlyCollection<IInviteMetadata> invites = await guild.GetInvitesAsync();

            int deleted = 0;
            
            foreach (IInviteMetadata invite in invites)
            {
                if (invite.Inviter == user)
                {
                    await invite.DeleteAsync();
                    deleted++;
                }
            }

            await guild.AddBanAsync(user, prune);
            
            _logger.LogInformation($"Banned {user.Username} and deleted {deleted} invite links.");
        }
    }
}