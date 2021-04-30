using Domain.Entities;
using Domain.Entities.ValueObjects;
using System;
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
        /// <param name="creditDept">貸方勘定</param>
        /// <param name="operationRep">登録担当者</param>
        /// <returns>データ処理件数</returns>
        public int Registration(CreditDept creditDept);
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
        /// <param name="creditDept">貸方勘定</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(CreditDept creditDept);
        /// <summary>
        /// 伝票内容データ更新
        /// </summary>
        /// <param name="content">伝票内容</param>
        /// <param name="operationRep">更新担当者</param>
        /// <returns>データ処理件数</returns>
        public int Update(Content content);
        /// <summary>
        /// 出納データを更新します
        /// </summary>
        /// <param name="receiptsAndExpenditure">更新出納データ</param>
        /// <returns>データ処理件数</returns>
        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure);
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
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isValidityTrueOnly);
        /// <summary>
        /// 貸方勘定検索
        /// </summary>
        /// <param name="account">貸方勘定</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <param name="isShunjuenAccountOnly">春秋苑会計に掲載されるデータのみ表示</param>
        /// <returns>貸方勘定リスト</returns>
        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly,bool isShunjuenAccountOnly);
        /// <summary>
        /// 伝票内容検索
        /// </summary>
        /// <param name="contentText">伝票内容</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>伝票内容リスト</returns>
        public ObservableCollection<Content> ReferenceContent
            (string contentText,string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly);
        /// <summary>
        /// IDを基に勘定科目を呼び出します
        /// </summary>
        /// <param name="id">勘定科目ID</param>
        /// <returns>勘定科目</returns>
        public AccountingSubject CallAccountingSubject(string id);
        /// <summary>
        /// IDを基に会計内容を呼び出します
        /// </summary>
        /// <param name="id">会計内容ID</param>
        /// <returns>会計内容</returns>
        public Content CallContent(string id);
        /// <summary>
        /// IDを基に貸方勘定を呼び出します
        /// </summary>
        /// <param name="id">貸方勘定ID</param>
        /// <returns>貸方勘定</returns>
        public CreditDept CallCreditDept(string id);
        /// <summary>
        /// IDを基に担当者を呼び出します
        /// </summary>
        /// <param name="id">担当者ID</param>
        /// <returns>担当者</returns>
        public Rep CallRep(string id);
        /// <summary>
        /// 伝票内容の文字列で、所属する勘定科目を検索します
        /// </summary>
        /// <param name="contentText">検索する伝票内容</param>
        /// <returns>勘定科目リスト</returns>
        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject
            (string contentText);
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
        /// <param name="creditDept">貸方勘定</param>
        /// <param name="content">内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="whichDepositAndWithdrawalOnly">入出金のどちらかのみを検索するか</param>
        /// <param name="isPayment">入金チェック</param>
        /// <param name="isContainOutputted">出力済みのものも含むか</param>
        /// <param name="isValidityOnly">有効性がTrueのみ</param>
        /// <param name="accountActivityDateStart">入出金日検索開始日時</param>
        /// <param name="accountActivityDateEnd">入出金日検索最終日時</param>
        /// <param name="outputDateStart">伝票発行日検索開始日時</param>
        /// <param name="outputDateEnd">伝票発行日検索最終日時</param>
        /// <returns></returns>
        public ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept,
                string content, string detail, string accountingSubject, string accountingSubjectCode,
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted, bool isValidityOnly,
                DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart,
                DateTime outputDateEnd);
        /// <summary>
        /// 出納データを検索します
        /// </summary>
        /// <param name="registrationDateStart">登録日検索開始日時</param>
        /// <param name="registrationDateEnd">登録日検索最終日時</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="creditDept">貸方勘定</param>
        /// <param name="content">内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="whichDepositAndWithdrawalOnly">入出金のどちらかのみを検索するか</param>
        /// <param name="isPayment">入金チェック</param>
        /// <param name="isContainOutputted">出力済みのものも含むか</param>
        /// <param name="isValidityOnly">有効性がTrueのみ</param>
        /// <param name="accountActivityDateStart">入出金日検索開始日時</param>
        /// <param name="accountActivityDateEnd">入出金日検索最終日時</param>
        /// <param name="outputDateStart">伝票発行日検索開始日時</param>
        /// <param name="outputDateEnd">伝票発行日検索最終日時</param>
        /// <param name="pageCount">ページカウント</param>
        /// <returns></returns>
        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List) ReferenceReceiptsAndExpenditure
            (DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept,
                string content, string detail, string accountingSubject, string accountingSubjectCode,
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted, bool isValidityOnly,
                DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart,
                DateTime outputDateEnd,int pageCount);
        /// <summary>
        /// 前日決算額を返します
        /// </summary>
        /// <returns>決算額</returns>
        public int PreviousDayFinalAmount();
        /// <summary>
        /// 前月決算を登録します
        /// </summary>
        public int RegistrationPerMonthFinalAccount();
        /// <summary>
        /// 前月決算額を返します
        /// </summary>
        /// <returns></returns>
        public int CallFinalAccountPerMonth();
        /// <summary>
        /// 伝票の出力日を前日に変更します
        /// </summary>
        public int ReceiptsAndExpenditurePreviousDayChange(ReceiptsAndExpenditure receiptsAndExpenditure);
    }
}
