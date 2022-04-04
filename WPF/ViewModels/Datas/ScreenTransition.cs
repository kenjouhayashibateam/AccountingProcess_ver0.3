using Domain.Entities.ValueObjects;
using System.Windows;
using WPF.Views;

namespace WPF.ViewModels.Datas
{
    /// <summary>
    /// 画面遷移統括クラス
    /// </summary>
    public static class ScreenTransition
    {
        /// <summary>
        /// 金庫金額計算ウィンドウを呼び出します
        /// </summary>
        /// <returns>金庫金額計算ウィンドウインスタンス</returns>
        public static Window RemainingMoneyCalculation()
        {
            return AccountingProcessLocation.Location == Locations.管理事務所 ?
                new RemainingMoneyCalculationView() :
                (Window)new ShorendoCashBoxCalculationView();
        }
        /// <summary>
        /// データ管理ウィンドウを呼び出します
        /// </summary>
        /// <returns>データ管理ウィンドウインスタンス</returns>
        public static Window DataManagement() { return new DataManagementView(); }
        /// <summary>
        /// ログインウィンドウを呼び出します
        /// </summary>
        /// <returns>ログインウィンドウインスタンス</returns>
        public static Window Login() { return new LoginView(); }
        /// <summary>
        /// 出納管理画面を呼び出します
        /// </summary>
        /// <returns>出納管理ウィンドウインスタンス</returns>
        public static Window ReceiptsAndExpenditureMangement()
        { return new ReceiptsAndExpenditureMangementView(); }
        /// <summary>
        /// 受納証発行画面を呼び出します
        /// </summary>
        /// <returns>受納証発行ウィンドウインスタンス</returns>
        public static Window CreateVoucher() { return new CreateVoucherView(); }
        /// <summary>
        /// 出納データ操作画面を呼び出します
        /// </summary>
        /// <returns>出納データ操作ウィンドウインスタンス</returns>
        public static Window ReceiptsAndExpenditureOperation()
        { return new ReceiptsAndExpenditureOperationView(); }
        /// <summary>
        /// パート交通費データ登録画面を呼び出します
        /// </summary>
        /// <returns>パート交通費データ登録ウィンドウインスタンス</returns>
        public static Window PartTimerTransportRegistration()
        { return new PartTimerTransportationExpensesRegistrationView(); }
        /// <summary>
        /// 御布施一覧データ登録画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window CondolenceOperation() { return new CondolenceOperationView(); }
        /// <summary>
        /// 御布施一覧出力画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window CreateCondolences() { return new CreateCondolencesView(); }
        /// <summary>
        /// 出納データ登録ヘルパー画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window ReceiptsAndExpenditureRegistrationHelper()
        { return new ReceiptsAndExpenditureRegistrationInputHelperView(); }
        /// <summary>
        /// 出納帳管理画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window CashJournalManagement() { return new CashJournalManagementView(); }
        /// <summary>
        /// 受納証管理画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window VoucherManagement() { return new VoucherManagementView(); }
        /// <summary>
        /// 出納データ検索画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window SearchReceiptsAndExpenditure()
        { return new SearchReceiptsAndExpenditureView(); }
        /// <summary>
        /// 花売りデータ登録画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window RegistrationFlowerReceiptsAndExpenditure()
        { return new RegistrationFlowerSellReceiptsAndExpenditureView(); }
        /// <summary>
        /// 物販売上登録画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window ProductSalesRegistration() { return new ProductSalesRegistrationView(); }
        /// <summary>
        /// 御布施一覧データ閲覧画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window SearchCondlences() { return new SeachCondlencesView(); }
        /// <summary>
        /// 振替出納データ管理画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window TransferReceiptsAndExpenditureManagement()
        { return new TransferReceiptsAndExpenditureManagementView(); }
        /// <summary>
        /// 振替出納データ操作画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window TransferReceiptsAndExpenditureOperationView()
        { return new TransferReceiptsAndExpenditureOperationView(); }
        /// <summary>
        /// 法事計算書登録画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window MemorialServiceAccountRegister() 
        { return new MemorialServiceAccountRegisterView(); }
    }
}
