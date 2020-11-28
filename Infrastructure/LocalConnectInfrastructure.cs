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
                (0, DateTime.Parse("1900/01/01"), new Rep("rep1", "林飛 顕誠", "aaa", true, true),"春秋苑", new CreditAccount("creditaccount1", "春秋苑", true),
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

        public ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure(string registrationDateStart, string registrationDateEnd, string location, string creditAccount, string accountingSubject, string accountingSubjectCode, bool whichDepositAndWithdrawalOnly, bool isPayment, bool isValidityOnly, string accountActivityDateStart, string accountActivityDateEnd)
        {
            Rep repHayashiba=new Rep("rep1","林飛 顕誠","aaa",true,true);
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            ObservableCollection<ReceiptsAndExpenditure> list = new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(0,DateTime.Today.AddDays(-1),repHayashiba,"管理事務所",new CreditAccount("credit_account","春秋苑",true),
                    new Content("content1",new AccountingSubject("account_subject1","000","納骨冥加",true),40000,"納骨手数料",true),"春秋家",40000,true,true,DateTime.Today.AddDays(-1),
                    repAkima,DateTime.Today.AddDays(-1)),
                new ReceiptsAndExpenditure(1,DateTime.Today.AddDays(-1),repAkima,"青蓮堂",new CreditAccount("credit_account","春秋苑",true),
                    new Content("content2",new AccountingSubject("account_subject2","100","葬儀冥加",true),-1,"青蓮堂使用費",true),"信行家",250000,true,true,DateTime.Today.AddDays(-1),
                    repHayashiba,DateTime.Today.AddDays(-1)),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",new CreditAccount("credit_account","信行寺",true),
                    new Content("content3",new AccountingSubject("account_subject3","202","雑費",true),-1,"文房具代",true),"消しゴム",150,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today)
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
