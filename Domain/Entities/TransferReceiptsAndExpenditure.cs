using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// 振替出納クラス
    /// </summary>
    public class TransferReceiptsAndExpenditure:ReceiptsAndExpenditureBase
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

        public TransferReceiptsAndExpenditure(int id, DateTime registrationDate, Rep registrationRep, string location, CreditDept creditDept,
                AccountingSubject debitAccount,AccountingSubject creditAccount,string contentText, string detail, int price, bool isValidity,
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
