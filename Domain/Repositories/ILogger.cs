
namespace Domain.Repositories
{
    /// <summary>
    /// ログ保存
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// ログの種類
        /// </summary>
        enum LogInfomation
        {
            INFOMATION,
            ERROR
        }
        /// <summary>
        /// ログを出力します
        /// </summary>
        /// <param name="info">ログの種類</param>
        /// <param name="message">ログの内容</param>
        void Log(LogInfomation info, string message);
    }
}
