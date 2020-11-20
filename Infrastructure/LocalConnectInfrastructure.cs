using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections.ObjectModel;

namespace Infrastructure
{
    public class LocalConnectInfrastructure : IDataBaseConnect
    {
        public AccountingSubject CallAccountingSubject(string id)
        {
            return new AccountingSubject("accounitng_subject1", "000", "法事冥加", true);
        }

        public ReceiptsAndExpenditure PreviousDayBalance()
        {
            return new ReceiptsAndExpenditure
                (0, DateTime.Parse("1900/01/01"), new Rep("rep1", "林飛 顕誠", "aaa", true, true), new CreditAccount("creditaccount1", "春秋苑", true),
                new Content("content0", new AccountingSubject("accountint_subject0", "000", "収支", false), -1, "収支日報", false), "収支総計", 1000000, true, false, DateTime.Parse("1900/01/01"), 
                new Rep("rep1", "林飛 顕誠", "aaa", true, true), DateTime.Parse("1900/01/01"));
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject(string subjectCode, string subject, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounitng_subject1","000","法事冥加",true),
                new AccountingSubject("accounting_subject2","000","葬儀冥加",true),
                new AccountingSubject("accounting_subject3","222","その他",false)
            };
            return list;
        }

        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject(string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounitng_subject1","000","法事冥加",true),
                new AccountingSubject("accounting_subject2","000","葬儀冥加",true),
            };
            return list;
        }

        public ObservableCollection<Content> ReferenceContent(string contentText, string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly)
        {
            ObservableCollection<Content> list = new ObservableCollection<Content>
            {
                new Content("content1",new AccountingSubject("accounitng_subject1","000","法事冥加",true),-1,"法事お布施",true),
                new Content("content2",new AccountingSubject("accounting_subject2","000","葬儀冥加",true),-1,"葬儀お布施",true),
                new Content("content3",new AccountingSubject("accounitng_subject1","000","法事冥加",true),13000,"焼香台",true)
            };
            return list;            
        }

        public ObservableCollection<CreditAccount> ReferenceCreditAccount(string account, bool isValidityTrueOnly)
        {
            ObservableCollection<CreditAccount> list = new ObservableCollection<CreditAccount>
            {
                new CreditAccount("credit_account1","春秋苑",true),
                new CreditAccount("credit_account2","法務部",true),
                new CreditAccount("credit_account3","ホテル",false)
            };
            return list;
        }
       
        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidity)
        {
            ObservableCollection<Rep> list = new ObservableCollection<Rep>
            {
                new Rep("rep1", "林飛 顕誠", "aaa", true,true),
                new Rep("rep2", "秋間 大樹", "bbb", false,false)
            };

            return list;
        }
  
        public int Registration(Rep rep)
        {
            return 1;
        }

        public int Registration(AccountingSubject accountingSubject)
        {
            return 1;
        }
 
        public int Registration(CreditAccount creditAccount)
        {
            return 1;
        }
 
        public int Registration(Content content)
        {
            return 1;
        }

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            return 1;
        }

        public int Update(Rep rep)
        {
            return 1;
        }
  
        public int Update(AccountingSubject accountingSubject)
        {
            return 1;
        }
  
        public int Update(CreditAccount creditAccount)
        {
            return 1;
        }

        public int Update(Content content)
        {
            return 1;
        }
    }
}
