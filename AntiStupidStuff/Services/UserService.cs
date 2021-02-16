using System.Collections.Generic;
using System.Threading.Tasks;
using AntiStupidStuff.Domain.Services;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using SmortCat.Domain.Services;

namespace AntiStupidStuff.Services
{
    public class UserService : IUserService
    {
        private ILogger _logger;
        private DiscordSocketClient _client;

        public UserService(ILogger logger, DiscordSocketClient client)
        {
            _logger = logger;
            _client = client;

            _client.UserBanned += UserBanned;
        }

        private async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            await DeleteInviteLinks(guild, user);
        }

        public async Task DeleteInviteLinks(IGuild guild, IUser user)
        {
            IReadOnlyCollection<IInviteMetadata> invites = await guild.GetInvitesAsync();

            int deleted = 0;
            
            foreach (IInviteMetadata inviteMetadata in invites)
            {
                if (inviteMetadata.Inviter.Id == user.Id)
                {
                    RestInviteMetadata invite = await _client.GetInviteAsync(inviteMetadata.Id);
                    await invite.DeleteAsync();
                    
                    deleted++;
                }
            }
            
            _logger.LogInformation($"Deleted {deleted} invite links of {user.Username}.");
        }

        public async Task BanAsync(IGuild guild, IUser user, int prune = 0)
        {
            await guild.AddBanAsync(user, prune);
            
            _logger.LogInformation($"Banned {user.Username}.");
        }
    }
}