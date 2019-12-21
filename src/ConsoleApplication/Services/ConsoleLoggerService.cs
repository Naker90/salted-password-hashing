using System;

namespace ConsoleApplication.Services
{
    public class ConsoleLoggerService : Logger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}