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
        /// <returns>データ処理件数</returns>
        public int Registration(Rep rep);
        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep);
        /// <summary>
        /// 担当者検索
        /// </summary>
        /// <param name="repName">担当者名</param>
        public void ReferenceRep(string repName);
    }
}
