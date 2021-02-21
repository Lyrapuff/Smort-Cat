using System.Threading.Tasks;
using Discord.Commands;

namespace SmortCat.Domain.Services.Obstacles
{
    /// <summary>
    /// Used by the CommandHandler before executing a command.
    /// </summary>
    public interface IObstacle
    {
        Task<bool> TryPass(CommandContext context);
    }
}