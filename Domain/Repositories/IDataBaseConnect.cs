using Domain.Entities;
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
        public int Registration(Rep rep);
        /// <summary>
        /// 勘定科目データ登録
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(AccountingSubject accountingSubject);
        /// <summary>
        /// 貸方勘定データ登録
        /// </summary>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(CreditAccount creditAccount);
        /// <summary>
        /// 伝票内容データ登録
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(Content content);
        /// <summary>
        /// 担当者データ更新
        /// </summary>
        /// <param name="rep">担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Rep rep);
        /// <summary>
        /// 勘定科目データ更新
        /// </summary>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(AccountingSubject accountingSubject);
        /// <summary>
        /// 貸方勘定データ更新
        /// </summary>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(CreditAccount creditAccount);
        /// <summary>
        /// 伝票内容データ更新
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Content content);
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
        /// <param name="account">貸方勘定</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>貸方勘定リスト</returns>
        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string account, bool isValidityTrueOnly);
        /// <summary>
        /// 伝票内容検索
        /// </summary>
        /// <param name="contentText">伝票内容</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>伝票内容リスト</returns>
        public ObservableCollection<Content> ReferenceContent(string contentText,string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly);
        /// <summary>
        /// IDを基に勘定科目を呼び出します
        /// </summary>
        /// <param name="id">勘定科目ID</param>
        /// <returns>勘定科目</returns>
        public AccountingSubject CallAccountingSubject(string id);
        /// <summary>
        /// 伝票内容の文字列で、所属する勘定科目を検索します
        /// </summary>
        /// <param name="contentText">検索する伝票内容</param>
        /// <returns>勘定科目リスト</returns>
        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject(string contentText);
        /// <summary>
        /// 前日の収入金額を返します
        /// </summary>
        /// <returns>前日収入金額</returns>
        public int PreviousDayIncome();
        /// <summary>
        /// 前日の支出金額を返します
        /// </summary>
        /// <returns>前日支出金額</returns>
        public int PreviousDayDisbursement();
        /// <summary>
        /// 月ごとの決算を返します
        /// </summary>
        /// <returns>決算額</returns>
        public int FinalAccountPerMonth();
        /// <summary>
        /// 出納データを登録します
        /// </summary>
        /// <param name="">出納データ</param>
        /// <returns>データ処理件数</returns>
        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure);
        /// <summary>
        /// 出納データを検索します
        /// </summary>
        /// <param name="registrationDateStart">登録日検索開始日時</param>
        /// <param name="registrationDateEnd">登録日検索最終日時</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="creditAccount">貸方勘定</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="whichDepositAndWithdrawalOnly">入出金のどちらかのみを検索するか</param>
        /// <param name="isPayment">入金チェック</param>
        /// <param name="isValidityOnly">有効性</param>
        /// <param name="accountActivityDateStart">入出金日検索開始日時</param>
        /// <param name="accountActivityDateEnd">入出金日検索最終日時</param>
        /// <returns></returns>
        public ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure(string registrationDateStart, string registrationDateEnd, string location, string creditAccount, string accountingSubject, string accountingSubjectCode, bool whichDepositAndWithdrawalOnly, bool isPayment, bool isValidityOnly, string accountActivityDateStart, string accountActivityDateEnd);
    }
}
