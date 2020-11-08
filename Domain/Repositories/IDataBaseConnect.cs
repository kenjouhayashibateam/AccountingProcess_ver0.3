using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Repositories
{
    /// <summary>
    /// データベース接続
    /// </summary>
    public interface IDataBaseConnect
    {
        /// <summary>
        /// 担当者登録
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(Rep rep, Rep operationRep);
        /// <summary>
        /// 担当者更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <param name="opelationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep, Rep opelationRep);
        /// <summary>
        /// 担当者検索
        /// </summary>
        /// <param name="repName">担当者名</param>
        /// <param name="isValidity">有効性がTrueのデータのみ検索チェック</param>
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidity);
        /// <summary>
        /// 勘定科目登録
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(AccountingSubject accountingSubject, Rep operationRep);
    }
}
