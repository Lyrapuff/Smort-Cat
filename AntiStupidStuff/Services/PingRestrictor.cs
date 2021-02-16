using System.Threading.Tasks;
using AntiStupidStuff.Domain.Services;
using Discord.WebSocket;

namespace AntiStupidStuff.Services
{
    public class PingRestrictor : IPingRestrictor
    {
        private DiscordSocketClient _client;
        
        private SocketTextChannel _notificationChannel;

        public PingRestrictor(DiscordSocketClient client)
        {
            _client = client;
        }
        
        public void Start()
        {
            _client.MessageReceived += MessageReceived;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            SocketUser user = message.Author;
            
            if (!user.IsBot)
            {
                if (message.MentionedUsers.Count > 3)
                {
                    if (_notificationChannel == null)
                    {
                        if (message.Channel is SocketGuildChannel channel)
                        {
                            _notificationChannel = channel.Guild.GetTextChannel(810139097322618950);

                            if (channel is SocketTextChannel textChannel)
                            {
                                await textChannel.SendMessageAsync($"Hey {user.Mention}, please stop pinging people or you'll get banned.");
                            }
                        }
                    }

                    if (_notificationChannel != null)
                    {
                        await _notificationChannel.SendMessageAsync($"{user.Mention} pings > 3 members.");
                    }
                }
            }
        }
    }
}