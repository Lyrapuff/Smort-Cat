using System.Threading.Tasks;
using Discord.WebSocket;

namespace Colors.Services
{
    public class PingRestrictor
    {
        private SocketTextChannel _notificationChannel;

        public async Task OnMessage(SocketMessage message)
        {
            SocketUser user = message.Author;
            
            if (!user.IsBot)
            {
                int mentionCount = message.MentionedUsers.Count + message.MentionedRoles.Count;

                if (mentionCount > 3)
                {
                    if (_notificationChannel == null)
                    {
                        SocketGuildChannel channel = message.Channel as SocketGuildChannel;
                        
                        _notificationChannel = channel.Guild.GetTextChannel(810139097322618950);
                    }
                    
                    await _notificationChannel.SendMessageAsync($"{user.Mention} pings > 3 members.");
                }
            }
        }
    }
}