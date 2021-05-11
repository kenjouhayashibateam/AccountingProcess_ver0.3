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
            if (AccountingProcessLocation.Location == "管理事務所")
                return new RemainingMoneyCalculationView();
            else return new ShorendoCashBoxCalculationView();
        }
        /// <summary>
        /// データ管理ウィンドウを呼び出します
        /// </summary>
        /// <returns>データ管理ウィンドウインスタンス</returns>
        public static Window DataManagement() => new DataManagementView();
        /// <summary>
        /// ログインウィンドウを呼び出します
        /// </summary>
        /// <returns>ログインウィンドウインスタンス</returns>
        public static Window Login() => new LoginView();
        /// <summary>
        /// 出納管理画面を呼び出します
        /// </summary>
        /// <returns>出納管理ウィンドウインスタンス</returns>
        public static Window ReceiptsAndExpenditureMangement() =>
            new ReceiptsAndExpenditureMangementView();
        /// <summary>
        /// 受納証発行画面を呼び出します
        /// </summary>
        /// <returns>受納証発行ウィンドウインスタンス</returns>
        public static Window CreateVoucher() => new CreateVoucherView();
        /// <summary>
        /// 出納データ操作画面を呼び出します
        /// </summary>
        /// <returns>出納データ操作ウィンドウインスタンス</returns>
        public static Window ReceiptsAndExpenditureOperation() =>
            new ReceiptsAndExpenditureOperationView();
        /// <summary>
        /// パート交通費データ登録画面を呼び出します
        /// </summary>
        /// <returns>パート交通費データ登録ウィンドウインスタンス</returns>
        public static Window PartTimerTransportRegistration() =>
            new PartTimerTransportationExpensesRegistrationView();
        /// <summary>
        /// 御布施一覧データ登録画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window CondolenceOperation() =>
            new CondolenceOperationView();
        /// <summary>
        /// 御布施一覧出力画面を呼び出します
        /// </summary>
        /// <returns></returns>
        public static Window CreateCondolences() =>
            new CreateCondolencesView();
    }
}
