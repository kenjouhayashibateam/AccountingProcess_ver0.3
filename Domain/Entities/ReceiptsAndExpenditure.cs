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
        public int ID { get; set; }
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
        /// 貸方部門
        /// </summary>
        public CreditDept CreditDept { get; set; }
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
        /// 出力日
        /// </summary>
        public DateTime OutputDate { get; set; }
        /// <summary>
        /// 未出力かのチェック
        /// </summary>
        public bool IsUnprinted { get; set; }
        /// <summary>
        /// 軽減税率かのチェック
        /// </summary>
        public bool IsReducedTaxRate { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="registrationDate">登録日</param>
        /// <param name="registrationRep">登録担当者</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="creditDept">貸方部門</param>
        /// <param name="content">内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="price">金額</param>
        /// <param name="isPayment">入出金チェック</param>
        /// <param name="isValidity">有効性</param>
        /// <param name="accountActivityDate">入出金日</param>
        /// <param name="outputDate">出力日</param>
        /// <param name="isRedicedTaxRate">軽減税率チェック</param>
        public ReceiptsAndExpenditure
            (int id, DateTime registrationDate, Rep registrationRep,string location, CreditDept creditDept, 
                Content content, string detail, int price, bool isPayment, bool isValidity,
                DateTime accountActivityDate, DateTime outputDate, bool isRedicedTaxRate)
        {
            ID =id;
            RegistrationDate = registrationDate;
            RegistrationRep = registrationRep;
            Location = location;
            CreditDept = creditDept;
            Content = content;
            Detail = detail;
            Price = price;
            IsPayment = isPayment;
            IsValidity = isValidity;
            AccountActivityDate = accountActivityDate;
            OutputDate = outputDate;
            IsUnprinted = OutputDate == TextHelper.DefaultDate;
            IsReducedTaxRate = isRedicedTaxRate;
        }
    }
}
