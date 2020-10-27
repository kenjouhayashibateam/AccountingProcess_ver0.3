using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Views;

namespace WPF.ViewModels.Commands
{
    /// <summary>
    /// 画面遷移を統括するクラス
    /// </summary>
    public class ScreenTransition
    {
        public RemainingMoneyCalculationView RemainingMoneyCalculation()
        {
            return new RemainingMoneyCalculationView();
        }

        public DataManagementView DataManagement()
        {
            return new DataManagementView();
        }
    }
}
