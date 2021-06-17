using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public interface ILogger
    {
        public void Log(string logMessage);
        public void LogError(string errorMessage);

    }

    class Logger
    {

        private Logger() { }

        private static ILogger source = null;

        public static ILogger Source
        {
            get
            {
                return source;
            }

            set { source = value; }
        } 
    }

    public class ConsoleLogger : ILogger
    {

        public void Log(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        public void LogError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }
    }
}
