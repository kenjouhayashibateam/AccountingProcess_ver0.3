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
        /// 担当者データ登録
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(Rep rep, Rep operationRep);
        /// <summary>
        /// 勘定科目データ登録
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(AccountingSubject accountingSubject, Rep operationRep);
        /// <summary>
        /// 貸方勘定データ登録
        /// </summary>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(CreditAccount creditAccount, Rep operationRep);
        /// <summary>
        /// 伝票内容データ登録
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(Content content, Rep operationRep);
        /// <summary>
        /// 担当者データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep, Rep opelationRep);
        /// <summary>
        /// 勘定科目データ更新
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(AccountingSubject accountingSubject, Rep operationRep);
        /// <summary>
        /// 貸方勘定データ更新
        /// </summary>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(CreditAccount creditAccount, Rep operationRep);
        /// <summary>
        /// 伝票内容データ更新
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Content content, Rep operationRep);
        /// <summary>
        /// 担当者検索
        /// </summary>
        /// <param name="repName">担当者名</param>
        /// <param name="isValidityTrueOnly">有効なデータのみ検索</param>
        /// <returns>担当者リスト</returns>
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidityTrueOnly);
        /// <summary>
        /// 勘定科目検索
        /// </summary>
        /// <param name="subjectCode">科目コード</param>
        /// <param name="subject">勘定科目</param>
        /// <param name="isTrueOnly">有効な物のみ表示する</param>
        /// <returns>勘定科目リスト</returns>
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isValidityTrueOnly);
        /// <summary>
        /// 貸方勘定検索
        /// </summary>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>貸方勘定リスト</returns>
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string creditAccount, bool isValidityTrueOnly);
        /// <summary>
        /// 伝票内容検索
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>伝票内容リスト</returns>
        public ObservableCollection<Content> ReferenceContent(string content, string accountingSubject, bool isValidityTrueOnly);
    }
}
