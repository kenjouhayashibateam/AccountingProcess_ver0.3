using System.Collections.ObjectModel;

namespace Domain.Entities
{
    /// <summary>
    /// 受納証クラス
    /// </summary>
    public class Voucher
    {
        /// <param name="addressee">宛名</param>
        /// <param name="receiptsAndExpenditures">出納データリスト</param>
        /// <param name="totalAmount">総額</param>
        public Voucher
            (string addressee, ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures,
                int totalAmount)
        {
            ReceiptsAndExpenditures = receiptsAndExpenditures;
            TotalAmount = totalAmount;
            Addressee = addressee;
        }

        /// <summary>
        /// 受納証に記載する出納データリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures { get; set; }
        /// <summary>
        /// 冥加金欄に記載する金額
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 受納証宛名
        /// </summary>
        public string Addressee { get; set; }
    }
}
