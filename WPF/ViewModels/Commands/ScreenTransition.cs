using WPF.Views;

namespace WPF.ViewModels.Commands
{
    /// <summary>
    /// 画面遷移統括クラス
    /// </summary>
    public class ScreenTransition
    {
        /// <summary>
        /// 金庫金額計算ビューを呼び出します
        /// </summary>
        /// <returns></returns>
        public RemainingMoneyCalculationView RemainingMoneyCalculation()
        {
            return new RemainingMoneyCalculationView();
        }
        /// <summary>
        /// データ管理ビューを呼び出します
        /// </summary>
        /// <returns></returns>
        public DataManagementView DataManagement()
        {
            return new DataManagementView();
        }
    }
}
