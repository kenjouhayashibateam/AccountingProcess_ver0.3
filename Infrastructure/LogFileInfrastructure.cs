using Domain.Repositories;
using System;
using System.IO;

namespace Infrastructure
{
    /// <summary>
    /// ログファイル出力クラス
    /// </summary>
    public class LogFileInfrastructure : ILogger
    {
        /// <summary>
        /// ログを出力します
        /// </summary>
        /// <param name="info">ログの種類</param>
        /// <param name="message">ログ内容</param>
        public void Log(ILogger.LogInfomation info, string message)
        {
            string[] textArray;
            using (StreamReader streamReader = new StreamReader(Properties.Resources.LogFilePath))
            {
                textArray = streamReader.ReadToEnd().Split('\n');
            }

            string newText = $"{info}\t{ DateTime.Now}\t{message}\n";

            for (int i = 0; i < textArray.Length; i++)
            {
                newText += $"{textArray[i]}\n";
            }

            using (StreamWriter streamWriter = new StreamWriter(Properties.Resources.LogFilePath))
            {
                streamWriter.WriteLine(newText);
                streamWriter.Flush();
            }
            _ = System.Diagnostics.Process.Start(Properties.Resources.LogFilePath);
        }
    }
}
