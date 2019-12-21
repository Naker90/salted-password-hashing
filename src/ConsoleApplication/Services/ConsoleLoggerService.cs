using System;

namespace ConsoleApplication.Services
{
    public sealed class ConsoleLoggerService : Logger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}