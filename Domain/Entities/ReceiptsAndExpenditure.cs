using Domain.Entities.Datas;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// 出納クラス
    /// </summary>
    public class ReceiptsAndExpenditure : ReceiptsAndExpenditureBase
    {
        /// <summary>
        /// 伝票内容
        /// </summary>
        public Content Content { get; set; }
        /// <summary>
        /// 出入金　入金はTrue、出金はFalse
        /// </summary>
        public bool IsPayment { get; set; }
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
            (int id, DateTime registrationDate, Rep registrationRep, string location, CreditDept creditDept,
                Content content, string detail, int price, bool isPayment, bool isValidity,
                DateTime accountActivityDate, DateTime outputDate, bool isRedicedTaxRate)
        {
            ID = id;
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
            IsUnprinted = OutputDate == DataHelper.DefaultDate;
            IsReducedTaxRate = isRedicedTaxRate;
        }
    }
}
