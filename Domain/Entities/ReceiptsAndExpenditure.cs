using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// 出納クラス
    /// </summary>
    public class ReceiptsAndExpenditure
    {
        /// <summary>
        /// 出納ID
        /// </summary>
        public int ReceiptsAndExpenditureID { get; set; }
        /// <summary>
        /// データ登録日
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// 登録担当者
        /// </summary>
        public Rep RegistrationRep { get; set; }
        /// <summary>
        /// 担当場所
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 貸方勘定
        /// </summary>
        public CreditAccount CreditAccount { get; set; }
        /// <summary>
        /// 伝票内容
        /// </summary>
        public Content Content { get; set; }
        /// <summary>
        /// 内容詳細
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 単位付き金額
        /// </summary>
        public string PriceWithUnit { get => TextHelper.AmountWithUnit(Price); }
        /// <summary>
        /// 出入金　入金はTrue、出金はFalse
        /// </summary>
        public bool IsPayment { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate { get; set; }
        /// <summary>
        /// 出力済みチェック
        /// </summary>
        public bool IsOutput { get; set; }

        public ReceiptsAndExpenditure(int receiptsAndExpenditureID, DateTime registrationDate, Rep registrationRep,string location, CreditAccount creditAccount, Content content, string detail, int price, bool isPayment, bool isValidity, DateTime accountActivityDate,bool isOutput)
        {
            ReceiptsAndExpenditureID = receiptsAndExpenditureID;
            RegistrationDate = registrationDate;
            RegistrationRep = registrationRep;
            Location = location;
            CreditAccount = creditAccount;
            Content = content;
            Detail = detail;
            Price = price;
            IsPayment = isPayment;
            IsValidity = isValidity;
            AccountActivityDate = accountActivityDate;
            IsOutput = isOutput;
        }
    }
}
