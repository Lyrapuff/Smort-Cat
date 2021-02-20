using System.Threading.Tasks;
using Discord.Commands;

namespace SmortCat.Domain.Services
{
    public interface IExecutionObstacle
    {
        Task<bool> TryPass(CommandContext context);
    }
}