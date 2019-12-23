using System;

namespace ConsoleApplication.Services
{
    public sealed class ConsoleLogger : Logger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}