using System.Collections.ObjectModel;

namespace Domain.Entities
{
    /// <summary>
    /// 受納証クラス
    /// </summary>
    public class Voucher
    {
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
