using Domain.Entities.ValueObjects;
using System;
using System.Collections.ObjectModel;
using static Domain.Entities.Helpers.TextHelper;

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
            (int id, string addressee, ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures,
                DateTime outputDate,Rep outputRep, bool isValidity)
        {
            ID = id;
            ReceiptsAndExpenditures = receiptsAndExpenditures;
            OutputDate = outputDate;
            Addressee = addressee;
            IsValidity = isValidity;
            OutputRep = outputRep;
        }
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 発行日
        /// </summary>
        public DateTime OutputDate { get; set; }
        /// <summary>
        /// 受納証に記載する出納データリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// 冥加金欄に記載する金額
        /// </summary>
        public int TotalAmount 
        {
            get
            {
                int i = default;
                foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures)
                    i += rae.Price;
                return i;
            }
        }
        /// <summary>
        /// 冥加金襴に記載する金額単位付き
        /// </summary>
        public string TotalAmountWithUnit => AmountWithUnit(TotalAmount);
        /// <summary>
        /// 受納証宛名
        /// </summary>
        public string Addressee { get; set; }
        /// <summary>
        /// 発行担当者
        /// </summary>
        public Rep OutputRep { get; set; }
    }
}
