
namespace Domain.Repositories
{
    public interface ILogger
    {
        enum LogInfomation
        {
            INFOMATION,
            ERROR
        }

        void Log(LogInfomation info, string message);
    }
}
