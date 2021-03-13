using WPF.Views;

namespace WPF.ViewModels.Commands
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
        public static RemainingMoneyCalculationView RemainingMoneyCalculation() => new RemainingMoneyCalculationView();
        /// <summary>
        /// データ管理ウィンドウを呼び出します
        /// </summary>
        /// <returns>データ管理ウィンドウインスタンス</returns>
        public static DataManagementView DataManagement() => new DataManagementView();
        /// <summary>
        /// ログインウィンドウを呼び出します
        /// </summary>
        /// <returns>ログインウィンドウインスタンス</returns>
        public static LoginView Login() => new LoginView();
        /// <summary>
        /// 出納管理画面を呼び出します
        /// </summary>
        /// <returns>出納管理ウィンドウインスタンス</returns>
        public static ReceiptsAndExpenditureMangementView ReceiptsAndExpenditureMangement() => new ReceiptsAndExpenditureMangementView();
        /// <summary>
        /// 青蓮堂金庫管理画面を呼び出します
        /// </summary>
        /// <returns>青蓮堂金庫管理ウインドウインスタンス</returns>
        public static ShorendoCashBoxCalculationView ShorendoCashBoxCalculation() => new ShorendoCashBoxCalculationView();
    }
}
