using Domain.Repositories;
using System.IO;

namespace Infrastructure
{
    class LogFile : ILogger
    {
        public void Log(ILogger.LogInfomation info, string message)
        {
            StreamWriter streamWriter = new StreamWriter(Properties.Resources.LogFilePath, true);

            streamWriter.WriteLine($"{info}{"\t"}{message}");
            streamWriter.Flush();
        }
    }
}
