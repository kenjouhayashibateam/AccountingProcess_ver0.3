using Domain.Entities.ValueObjects;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// 伝票クラス
    /// </summary>
    public class Slip
    {
        /// <summary>
        /// ID
        /// </summary>
        public int SlipID { get; set; }
        public AccountingLocation Location { get; set; }
        /// <summary>
        /// 伝票登録日
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        public CreditAccount CreditAccount { get; set; }
        public Content Content { get; set; }
        /// <summary>
        /// 内容詳細
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }
        public Rep Rep { get; set; }
        /// <summary>
        /// 出入金　入金はTrue、出勤はFalse
        /// </summary>
        public bool IsPayment { get; set; }
    }
}
