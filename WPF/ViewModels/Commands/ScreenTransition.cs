using WPF.Views;

namespace WPF.ViewModels.Commands
{
    /// <summary>
    /// 画面遷移統括クラス
    /// </summary>
    public class ScreenTransition
    {
        /// <summary>
        /// 金庫金額計算ウィンドウを呼び出します
        /// </summary>
        /// <returns>金庫金額計算ウィンドウインスタンス</returns>
        public RemainingMoneyCalculationView RemainingMoneyCalculation()
        {
            return new RemainingMoneyCalculationView();
        }
        /// <summary>
        /// データ管理ウィンドウを呼び出します
        /// </summary>
        /// <returns>データ管理ウィンドウインスタンス</returns>
        public DataManagementView DataManagement()
        {
            return new DataManagementView();
        }
        /// <summary>
        /// ログインウィンドウを呼び出します
        /// </summary>
        /// <returns>ログインウィンドウインスタンス</returns>
        public LoginView Login()
        {
            return new LoginView();
        }
        /// <summary>
        /// 伝票管理画面を呼び出します
        /// </summary>
        /// <returns>伝票管理ウィンドウインスタンス</returns>
        public SlipMangementView SlipManagement()
        {
            return new SlipMangementView();
        }
    }
}
