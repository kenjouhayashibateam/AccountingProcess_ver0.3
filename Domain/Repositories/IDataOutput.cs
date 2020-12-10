namespace Domain.Repositories
{
    /// <summary>
    /// データ出力
    /// </summary>
    public interface IDataOutput
    {
        /// <summary>
        /// 金庫データを出力します
        /// </summary>
        void CashboxData();
        /// <summary>
        /// 収支日報を出力します
        /// </summary>
        /// <param name="previousDayFinalAccountWithUnit">前日決算</param>
        /// <param name="paymentWithUnit">入金</param>
        /// <param name="withdrawalWithUnit">出金</param>
        /// <param name="tranceferAmountWithUnit">社内振替</param>
        /// <param name="todayFinalAccountWithUnit">残高</param>
        /// <param name="yokohamaBankAmountWithUnit">横浜銀行残高</param>
        /// <param name="ceresaAmountWithUnit">セレサ川崎残高</param>
        /// <param name="wizeCoreAmountWithUnit">ワイズコア仮受金</param>
        void BalanceFinalAccount(string previousDayFinalAccountWithUnit, string paymentWithUnit, string withdrawalWithUnit, string tranceferAmountWithUnit, string todayFinalAccountWithUnit, string yokohamaBankAmountWithUnit, string ceresaAmountWithUnit, string wizeCoreAmountWithUnit);
    }
}
