using System;

namespace SmortCat.Domain.Services
{
    public interface ILogger
    {
        void LogInformation(object data);
        void LogWarning(object data);
        void LogError(object data);
        void LogException(string source, Exception e);
        void LogDebug(object data);
    }
}