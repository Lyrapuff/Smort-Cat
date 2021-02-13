using System.Threading.Tasks;
using SmortCat.Core.Services;
using SmortCat.Domain.Services;

namespace SmortCat.Starter
{
    internal static class Program
    {
        internal static async Task Main()
        {
            IBotStarter botStarter = new BotStarter();
            await botStarter.Start();

            await Task.Delay(-1);
        }
    }
}