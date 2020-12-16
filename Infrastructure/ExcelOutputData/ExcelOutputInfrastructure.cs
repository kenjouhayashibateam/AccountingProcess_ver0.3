using Domain.Entities;
using Domain.Repositories;
using System.Collections.ObjectModel;

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

        public void CashboxData()
        {
            CashBoxOutput cbo = new CashBoxOutput();
            cbo.DataOutput();
        }

        public void ReceiptsAndExpenditureData(ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures, int previousDayBalance)
        {
            ReceiptsAndExpenditureOutput raeo = new ReceiptsAndExpenditureOutput(receiptsAndExpenditures, previousDayBalance);
            raeo.DataOutput();
        }
    }
}
