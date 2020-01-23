using NLog;
using System;

namespace Asendia.Order.Monitor
{
    public static class MessageHelper
    {
        private static bool lastPercentage = false;
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public static void WriteLine(string message)
        {
            if (lastPercentage)
            {
                Console.WriteLine();
                lastPercentage = false;
            }
            Console.WriteLine("{0} - INFO - {1}", DateTime.Now, message);
            _logger.Info(message);
        }

        public static void WriteLine(LogLevel level, string message)
        {
            if (!string.IsNullOrEmpty(message) && message.Contains("%"))
            {
                Console.CursorLeft = 0;
                Console.Write("{0} - {1} - {2}", DateTime.Now, level, message);
                lastPercentage = true;
            }
            else
            {
                if (lastPercentage)
                {
                    Console.WriteLine();
                    lastPercentage = false;
                }
                Console.WriteLine("{0} - {1} - {2}", DateTime.Now, level, message);
                _logger.Log(level, message);
            }
        }

        public static void DisplayProgress(int proceeded, int totalCount)
        {
            var percentComplete = (int)Math.Round((double)(100 * proceeded) / totalCount, 2);
            WriteLine(LogLevel.Info, $"\r Generating Orders XML files : {proceeded}/{totalCount} [{percentComplete}%] proceeded so far");             
        }
    }
}
