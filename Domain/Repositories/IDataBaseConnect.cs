using Domain.Entities.ValueObjects;

namespace Domain.Repositories
{
    /// <summary>
    /// データベースに接続します。
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
