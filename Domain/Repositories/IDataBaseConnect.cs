using Domain.Entities.ValueObjects;

namespace Domain.Repositories
{
    /// <summary>
    /// データベース接続
    /// </summary>
    public interface IDataBaseConnect
    {
        /// <summary>
        /// データ登録
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns></returns>
        public int Registration(Rep rep);
    }
}
