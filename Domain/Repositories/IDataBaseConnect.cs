using Domain.Entities.ValueObjects;

namespace Domain.Repositories
{
    /// <summary>
    /// データベースに接続します。
    /// </summary>
    public interface IDataBaseConnect
    {
        public int Registration(Rep rep);
    }
}
