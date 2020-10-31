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
        public int SlipID;
        public AccountingLocation Location;
        /// <summary>
        /// 伝票登録日
        /// </summary>
        public DateTime RegistrationDate;
        public CreditAccount CreditAccount;
        public Content Content;
        /// <summary>
        /// 内容詳細
        /// </summary>
        public string Detail;
        /// <summary>
        /// 金額
        /// </summary>
        public int Amount;
        public Rep Rep;
        /// <summary>
        /// 出入金　入金はTrue、出勤はFalse
        /// </summary>
        public bool IsPayment;
    }
}
