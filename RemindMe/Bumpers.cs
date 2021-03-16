using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace RemindMe
{
    public class Bumpers
    {
        private SocketTextChannel _channel;

        public Bumpers(DiscordSocketClient client)
        {
            SocketGuild guild = client.GetGuild(808725004367822920);
            _channel = guild.GetTextChannel(808815737674006578);
        }
        
        public async ValueTask Start()
        {
            while (true)
            {
                // Wait for 2 hours
                await Task.Delay(1000 * 60 * 60 * 2);
                await _channel.SendMessageAsync($"<@&821486024898773033>{Environment.NewLine}Bump! uwu");
            }
        }
    }
}