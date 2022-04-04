using Domain.Entities.Datas;
using Domain.Entities.ValueObjects;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// 振替出納クラス
    /// </summary>
    public class TransferReceiptsAndExpenditure : ReceiptsAndExpenditureBase
    {
        /// <summary>
        /// 借方勘定科目
        /// </summary>
        public AccountingSubject DebitAccount { get; set; }
        /// 貸方勘定
        /// </summary>
        public AccountingSubject CreditAccount { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ContentText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="registrationDate">登録日</param>
        /// <param name="registrationRep">登録担当者</param>
        /// <param name="location">経理担当場所</param>
        /// <param name="creditDept">貸方部門</param>
        /// <param name="debitAccount">借方勘定科目</param>
        /// <param name="creditAccount">貸方勘定科目</param>
        /// <param name="contentText">伝票内容</param>
        /// <param name="detail">詳細</param>
        /// <param name="price">金額</param>
        /// <param name="isValidity">有効性</param>
        /// <param name="accountActivityDate">振替日</param>
        /// <param name="outputDate">出力日</param>
        /// <param name="isRedicedTaxRate">軽減税率チェック</param>
        public TransferReceiptsAndExpenditure(int id, DateTime registrationDate, Rep registrationRep,
            string location, CreditDept creditDept, AccountingSubject debitAccount, 
            AccountingSubject creditAccount, string contentText, string detail, int price, bool isValidity, 
            DateTime accountActivityDate, DateTime outputDate, bool isRedicedTaxRate)
        {
            ID = id;
            RegistrationDate = registrationDate;
            RegistrationRep = registrationRep;
            Location = location;
            CreditDept = creditDept;
            DebitAccount = debitAccount;
            CreditAccount = creditAccount;
            ContentText = contentText;
            Detail = detail;
            Price = price;
            IsValidity = isValidity;
            AccountActivityDate = accountActivityDate;
            OutputDate = outputDate;
            IsReducedTaxRate = isRedicedTaxRate;
        }
    }
}
