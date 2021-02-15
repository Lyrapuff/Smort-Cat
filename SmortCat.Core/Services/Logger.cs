using System;
using SmortCat.Domain.Services;

namespace SmortCat.Core.Services
{
    public class Logger : ILogger
    {
        public void LogInformation(object data)
        {
            Log($"[Info][{DateTime.Now:t}] {data}", ConsoleColor.Gray);
        }

        public void LogWarning(object data)
        {
            Log($"[Warn][{DateTime.Now:t}] {data}", ConsoleColor.DarkYellow);
        }

        public void LogError(object data)
        {
            Log($"[Fail][{DateTime.Now:t}] {data}", ConsoleColor.DarkRed);
        }

        public void LogException(string source, Exception e)
        {
            Log($"[Fail][{DateTime.Now:t}] Exception in {source}{Environment.NewLine}{e}", ConsoleColor.DarkRed);
        }

        public void LogDebug(object data)
        {
            Log($"[Dbug][{DateTime.Now:t}] {data}", ConsoleColor.DarkCyan);
        }

        private void Log(object data, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}