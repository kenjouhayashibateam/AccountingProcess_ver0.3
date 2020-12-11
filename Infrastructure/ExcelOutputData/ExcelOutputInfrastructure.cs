using Domain.Repositories;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// エクセル出力クラス
    /// </summary>
    public class ExcelOutputInfrastructure : IDataOutput
    {
        public void BalanceFinalAccount(string previousDayFinalAccountWithUnit, string paymentWithUnit, string withdrawalWithUnit, string tranceferAmountWithUnit, string todayFinalAccountWithUnit, string yokohamaBankAmountWithUnit, string ceresaAmountWithUnit, string wizeCoreAmountWithUnit)
        {
            BalanceFinalAccountOutput bfao = new BalanceFinalAccountOutput(previousDayFinalAccountWithUnit, paymentWithUnit, withdrawalWithUnit, tranceferAmountWithUnit, todayFinalAccountWithUnit, yokohamaBankAmountWithUnit, ceresaAmountWithUnit, wizeCoreAmountWithUnit);
            bfao.DataOutput();
        }

        /// <summary>
        /// 金庫データを出力します
        /// </summary>
        public void CashboxData()
        {
            CashBoxOutput cbo = new CashBoxOutput();
            cbo.DataOutput();
        }
    }
}
