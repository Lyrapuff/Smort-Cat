using System.Threading.Tasks;
using Discord.Commands;

namespace AntiStupidStuff.Commands
{
    public class TestCommand : ModuleBase
    {
        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("Meow OwO");
        }
    }
}