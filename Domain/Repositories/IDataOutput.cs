using Domain.Entities;

namespace Domain.Repositories
{
    /// <summary>
    /// データ出力
    /// </summary>
    public interface IDataOutput
    {
        /// <summary>
        /// 金庫データを出力します
        /// </summary>
        void CashBoxDataOutput();
    }
}
