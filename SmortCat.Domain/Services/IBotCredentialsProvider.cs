using SmortCat.Domain.Models;

namespace SmortCat.Domain.Services
{
    public interface IBotCredentialsProvider
    {
        BotCredentials GetCredentials();
    }
}