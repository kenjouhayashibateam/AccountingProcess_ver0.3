using Domain.Entities;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Repositories
{
    public enum WizeCoreDept
    {
        蓮華庵,
        春秋庵,
        香華
    }
    public enum WizeCoreAmountCategory
    {
        繰越,
        入金,
        出金,
        銀行振替
    }
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
        /// 御布施一覧データ登録
        /// </summary>
        /// <param name="condolence">データ</param>
        /// <returns></returns>
        public int Registration(Condolence condolence);
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
        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidityTrueOnly);
        /// <summary>
        /// 勘定科目検索
        /// </summary>
        /// <param name="subjectCode">科目コード</param>
        /// <param name="subject">勘定科目</param>
        /// <param name="isTrueOnly">有効な物のみ表示する</param>
        /// <returns>勘定科目リスト</returns>
        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isShunjuen, bool isValidityTrueOnly);
        /// <summary>
        /// 貸方勘定検索
        /// </summary>
        /// <param name="account">貸方勘定</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <param name="isShunjuenAccount">春秋苑会計か</param>
        /// <returns>貸方勘定リスト</returns>
        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly, bool isShunjuenAccount);
        /// <summary>
        /// 伝票内容検索
        /// </summary>
        /// <param name="contentText">伝票内容</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="isShunjuen">春秋苑会計か</param>
        /// <param name="isValidityTrueOnly">有効な物のみ表示</param>
        /// <returns>伝票内容リスト</returns>
        public ObservableCollection<Content> ReferenceContent
            (string contentText, string accountingSubjectCode, string accountingSubject, bool isShunjuen,
                bool isValidityTrueOnly);
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
        /// <param name="creditDept">貸方部門</param>
        /// <param name="content">内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="isShunjuen">春秋苑会計か</param>
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
                bool isShunjuen, bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted,
                bool isValidityOnly, DateTime accountActivityDateStart, DateTime accountActivityDateEnd,
                DateTime outputDateStart, DateTime outputDateEnd);
        /// <summary>
        /// 出納データを検索します
        /// </summary>
        /// <param name="registrationDateStart">登録日検索開始日時</param>
        /// <param name="registrationDateEnd">登録日検索最終日時</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="creditDept">貸方部門</param>
        /// <param name="content">内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="accountingSubject">勘定科目</param>
        /// <param name="accountingSubjectCode">勘定科目コード</param>
        /// <param name="isShunjuen">春秋苑会計か</param>
        /// <param name="whichDepositAndWithdrawalOnly">入出金のどちらかのみを検索するか</param>
        /// <param name="isPayment">入金チェック</param>
        /// <param name="isContainOutputted">出力済みのものも含むか</param>
        /// <param name="isValidityOnly">有効性がTrueのみ</param>
        /// <param name="accountActivityDateStart">入出金日検索開始日時</param>
        /// <param name="accountActivityDateEnd">入出金日検索最終日時</param>
        /// <param name="outputDateStart">伝票発行日検索開始日時</param>
        /// <param name="outputDateEnd">伝票発行日検索最終日時</param>
        /// <param name="pageCount">ページカウント</param>
        /// <param name="sortColumn">ソートカラム</param>
        /// <param name="sortDirection">ソート方向</param>
        /// <param name="countEachPage">1ページの行数</param>
        /// <returns></returns>
        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List)
            ReferenceReceiptsAndExpenditure
                (DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept,
                    string content, string detail, string accountingSubject, string accountingSubjectCode,
                    bool isShunjuen, bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted,
                    bool isValidityOnly, DateTime accountActivityDateStart, DateTime accountActivityDateEnd,
                    DateTime outputDateStart, DateTime outputDateEnd, int pageCount, string sortColumn,
                    bool sortDirection, int countEachPage);
        /// <summary>
        /// 前日決算額を返します
        /// </summary>
        /// <returns>決算額</returns>
        public int PreviousDayFinalAmount(bool isShunjuen);
        /// <summary>
        /// 貸方部門の前日決算額を返します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <returns></returns>
        public int PreviousDayFinalAmount(CreditDept creditDept);
        /// <summary>
        /// 春秋苑前年度決算を登録します
        /// </summary>
        public int RegistrationPrecedingYearFinalAccount();
        /// <summary>
        /// ワイズコア前年度決算を登録します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <returns></returns>
        public int RegistrationPrecedingYearFinalAccount(CreditDept creditDept);
        /// <summary>
        /// 春秋苑前年度決算を更新します
        /// </summary>
        /// <returns></returns>
        public int UpdatePrecedingYearFinalAccount();
        /// <summary>
        /// ワイズコア前年度決算を更新します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <returns></returns>
        public int UpdatePrecedingYearFinalAccount(CreditDept creditDept);
        /// <summary>
        /// ワイズコアの前年度決算額を返します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <returns></returns>
        public int CallPrecedingYearFinalAccount(DateTime date, CreditDept creditDept);
        /// <summary>
        /// 春秋苑の前年度決算額を返します
        /// </summary>
        /// <param name="date">基準の日付</param>
        /// <returns></returns>
        public int CallPrecedingYearFinalAccount(DateTime date);
        /// <summary>
        /// 前月決算を返します
        /// </summary>
        /// <param name="date">返してほしい月の次の月</param>
        /// <param name="isShunjuen">春秋苑会計か</param>
        /// <param name="creditDept">貸方部門</param>
        /// <returns></returns>
        public int CallFinalMonthFinalAccount(DateTime date, bool isShunjuen, CreditDept creditDept);
        /// <summary>
        /// 伝票の出力日を前日に変更します
        /// </summary>
        public int ReceiptsAndExpenditurePreviousDayChange(ReceiptsAndExpenditure receiptsAndExpenditure);
        /// <summary>
        /// 信行寺の僧侶リストを返します
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSoryoList();
        /// <summary>
        /// 御布施一覧データを更新します
        /// </summary>
        /// <param name="condolence">更新する御布施一覧データ</param>
        /// <returns></returns>
        public int Update(Condolence condolence);
        /// <summary>
        /// 御布施一覧データを検索します
        /// </summary>
        /// <param name="startDate">検索最古日時</param>
        /// <param name="dateTime">検索最新日時</param>
        /// <returns></returns>
        public ObservableCollection<Condolence> ReferenceCondolence
            (DateTime startDate, DateTime endDate, string location);
        /// <summary>
        /// 御布施一覧データを検索します
        /// </summary>
        /// <param name="startDate">検索最古日時</param>
        /// <param name="endDate">検索最新日時</param>
        /// <param name="pageCount">ページカウント</param>
        /// <returns></returns>
        public (int TotalRows, ObservableCollection<Condolence> List) ReferenceCondolence
            (DateTime startDate, DateTime endDate, string location, int pageCount, int countEachPage);
        /// <summary>
        /// 受納証に異なった文字列を表示する伝票内容を登録します
        /// </summary>
        /// <param name="id">伝票内容ID</param>
        /// <param name="contentConvertText">表示する伝票内容文字列</param>
        /// <returns></returns>
        public int Registration(string id, string contentConvertText);
        /// <summary>
        /// 受納証に異なった文字列を表示する伝票内容を更新します
        /// </summary>
        /// <param name="id">伝票内容ID</param>
        /// <param name="contentConvertText">表示する伝票内容文字列</param>
        /// <returns></returns>
        public int Update(string id, string contentConvertText);
        /// <summary>
        /// 受納証に異なった文字列を表示する伝票内容を呼び出します
        /// </summary>
        /// <param name="id">検索する伝票内容ID</param>
        /// <returns></returns>
        public string CallContentConvertText(string id);
        /// <summary>
        /// 受納証に異なった文字列を表示する伝票内容データを削除します
        /// </summary>
        /// <param name="convertContent">削除する伝票内容</param>
        /// <returns></returns>
        public int DeleteConvertContent(Content convertContent);
        /// <summary>
        /// 受納証を登録します
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public int Registration(Voucher voucher);
        /// <summary>
        /// 最新のVoucherデータを呼び出します
        /// </summary>
        /// <returns></returns>
        public Voucher CallLatestVoucher();
        /// <summary>
        /// 受納証GTBLを登録します
        /// </summary>
        /// <param name="voucherID"></param>
        /// <param name="receiptsAndExpenditureID"></param>
        /// <returns></returns>
        public int Registration(int voucherID, int receiptsAndExpenditureID);
        /// <summary>
        /// 受納証データを更新します
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public int Update(Voucher voucher);
        /// <summary>
        /// 受納証データを検索します
        /// </summary>
        /// <param name="outputDateStart"></param>
        /// <param name="outputDateEnd"></param>
        /// <param name="isValidityTrueOnly"></param>
        /// <returns></returns>
        public ObservableCollection<Voucher> ReferenceVoucher
            (DateTime outputDateStart, DateTime outputDateEnd, bool isValidityTrueOnly);
        /// <summary>
        /// 受納証データにグループされた出納データを呼び出します
        /// </summary>
        /// <param name="voucherID"></param>
        /// <returns></returns>
        public ObservableCollection<ReceiptsAndExpenditure>
            CallVoucherGroupingReceiptsAndExpenditure(int voucherID);
        /// <summary>
        /// お布施一覧データを削除します
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(Condolence condolence);
        /// <summary>
        /// 伝票内容に設定された既定の貸方部門を呼び出します
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public CreditDept CallContentDefaultCreditDept(Content content);
        /// <summary>
        /// 伝票内容規定の貸方部門を登録します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int Registration(CreditDept creditDept, Content content);
        /// <summary>
        /// 伝票内容規定の貸方部門を更新します
        /// </summary>
        /// <param name="creditDept"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int Update(CreditDept creditDept, Content content);
        /// <summary>
        /// 伝票内容規定の貸方部門を削除します
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public int DeleteContentDefaultCreditDept(Content content);
        /// <summary>
        /// 勘定科目コードの枝番を取得します
        /// </summary>
        /// <param name="accountingSubject"></param>
        /// <returns></returns>
        public string GetBranchNumber(AccountingSubject accountingSubject);
        /// <summary>
        /// 勘定科目コードの枝番を登録します
        /// </summary>
        /// <param name="accountingSubject"></param>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public int Registration(AccountingSubject accountingSubject, string branchNumber);
        /// <summary>
        /// 勘定科目コードの枝番を更新します
        /// </summary>
        /// <param name="accountingSubject"></param>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public int Update(AccountingSubject accountingSubject, string branchNumber);
        /// <summary>
        /// 勘定科目コードの枝番を削除します
        /// </summary>
        /// <param name="accountingSubject"></param>
        /// <returns></returns>
        public int DeleteBranchNumber(AccountingSubject accountingSubject);
        /// <summary>
        /// 正式サーバーに接続しているか
        /// </summary>
        /// <returns></returns>
        public bool IsConnectiongProductionServer();
        ///// <summary>
        ///// 出納データに関連している振替データを返します
        ///// </summary>
        ///// <returns></returns>
        //public ObservableCollection<TransferReceiptsAndExpenditure> CallGroupingTransferReceiptsAndExpenditure
        //    (ReceiptsAndExpenditure receiptsAndExpenditure);
        /// <summary>
        /// 振替データを検索します
        /// </summary>
        /// <param name="accountActivityDateStart">振替日検索開始日</param>
        /// <param name="acountActivityDateEnd">振替日検索最終日</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="dept">貸方部門</param>
        /// <param name="debitAccountCode">借方勘定科目コード</param>
        /// <param name="creditAccountCode">貸方勘定科目コード</param>
        /// <param name="isValidityTrueOnly">有効なもののみのリストにするか</param>
        /// <param name="containOutputted">既に出力したものも含むか</param>
        /// <param name="outputDateStart">印刷日検索開始日</param>
        /// <param name="outputDateEnd">印刷日検索最終日</param>
        /// <returns></returns>
        public ObservableCollection<TransferReceiptsAndExpenditure> ReferenceTransferReceiptsAndExpenditure
            (DateTime accountActivityDateStart, DateTime acountActivityDateEnd, string location, string dept, 
                string debitAccountCode, string creditAccountCode, bool isValidityTrueOnly, bool containOutputted, 
                DateTime outputDateStart, DateTime outputDateEnd);
    }
}
