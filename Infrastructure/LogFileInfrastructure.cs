using Domain.Repositories;
using System.IO;

namespace Infrastructure
{
    /// <summary>
    /// ログファイル出力クラス
    /// </summary>
  　public  class LogFileInfrastructure : ILogger
    {
        /// <summary>
        /// ログを出力します
        /// </summary>
        /// <param name="info">ログの種類</param>
        /// <param name="message">ログ内容</param>
        public void Log(ILogger.LogInfomation info, string message)
        {
   
            StreamWriter streamWriter = new StreamWriter(Properties.Resources.LogFilePath, true);

            streamWriter.WriteLine($"{info}{"\t"}{message}");
            streamWriter.Flush();
            _ = System.Diagnostics.Process.Start(Properties.Resources.LogFilePath);
        }
    }
}
