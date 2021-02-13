namespace SmortCat.Domain.Services
{
    public interface ILogger
    {
        void LogInformation(object data);
        void LogWarning(object data);
        void LogError(object data);
        void LogDebug(object data);
    }
}