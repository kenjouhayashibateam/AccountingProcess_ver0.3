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
        /// 伝票ID
        /// </summary>
        public int SlipID { get; set; }
        /// <summary>
        /// 伝票作成場所
        /// </summary>
        public AccountingProcessLocation Location { get; set; }
        /// <summary>
        /// 伝票登録日
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// 登録担当者
        /// </summary>
        public Rep RegistrationRep { get; set; }
        /// <summary>
        /// 勘定科目
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
        /// 出入金　入金はTrue、出金はFalse
        /// </summary>
        public bool IsPayment { get; set; }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// 認証日
        /// </summary>
        public DateTime ApprovalDate { get; set; }
        /// <summary>
        /// 認証担当者
        /// </summary>
        public Rep ApprovalRep { get; set; }
        /// <summary>
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate { get; set; }

        public ReceiptsAndExpenditure(int slipID, AccountingProcessLocation location, DateTime registrationDate, Rep registrationRep, CreditAccount creditAccount, Content content, string detail, int price, bool isPayment, bool isValidity, DateTime approvalDate, Rep approvalRep, DateTime accountActivityDate)
        {
            SlipID = slipID;
            Location = location;
            RegistrationDate = registrationDate;
            RegistrationRep = registrationRep;
            CreditAccount = creditAccount;
            Content = content;
            Detail = detail;
            Price = price;
            IsPayment = isPayment;
            IsValidity = isValidity;
            ApprovalDate = approvalDate;
            ApprovalRep = approvalRep;
            AccountActivityDate = accountActivityDate;
        }
    }
}
