﻿using Domain.Entities;
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
                new AccountingSubject("id0", "822", "その他雑収入", true),
                new AccountingSubject("id1", "822", "その他冥加金", true),
                new AccountingSubject("id2", "874", "その他茶所収入", true),
                new AccountingSubject("id3", "416", "仮受金", true),
                new AccountingSubject("id4", "611", "旅費交通費", true),
                new AccountingSubject("id5", "616", "消耗品費", true),
                new AccountingSubject("id6", "621", "車両費", true),
                new AccountingSubject("id7", "735", "苑内整備費", true),
                new AccountingSubject("id8", "168", "仮払金", true),
                new AccountingSubject("id9", "133", "セレサ川崎普通貯金", true)
            };
            return list;
        }

        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject(string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("id0", "822", "その他雑収入", true),
                new AccountingSubject("id1", "822", "その他冥加金", true),
                new AccountingSubject("id2", "874", "その他茶所収入", true),
                new AccountingSubject("id3", "416", "仮受金", true),
                new AccountingSubject("id4", "611", "旅費交通費", true),
                new AccountingSubject("id5", "616", "消耗品費", true),
                new AccountingSubject("id6", "621", "車両費", true),
                new AccountingSubject("id7", "735", "苑内整備費", true),
                new AccountingSubject("id8", "168", "仮払金", true),
                new AccountingSubject("id9", "133", "セレサ川崎普通貯金", true)
            };
            return list;
        }

        public ObservableCollection<Content> ReferenceContent(string contentText, string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly)
        {
            AccountingSubject OtherMiscellaneousIncome = new AccountingSubject("id0", "822", "その他雑収入", true);
            AccountingSubject OtherContribution = new AccountingSubject("id1", "822", "その他冥加金", true);
            AccountingSubject OtherTyadokoroIncome = new AccountingSubject("id2", "874", "その他茶所収入", true);
            AccountingSubject SuspenseReceiptMoney = new AccountingSubject("id3", "416", "仮受金", true);
            AccountingSubject TravelExpense = new AccountingSubject("id4", "611", "旅費交通費", true);
            AccountingSubject SuppliesExpense = new AccountingSubject("id5", "616", "消耗品費", true);
            AccountingSubject VehicleFee = new AccountingSubject("id6", "621", "車両費", true);
            AccountingSubject InternalMaintenanceExpenses = new AccountingSubject("id7", "735", "苑内整備費", true);
            AccountingSubject SuspensePayment = new AccountingSubject("id8", "168", "仮払金", true);
            AccountingSubject Seresa = new AccountingSubject("id9", "133", "セレサ川崎普通貯金", true);

            ObservableCollection<Content> list = new ObservableCollection<Content>
            {
                new Content("content3",OtherMiscellaneousIncome,-1,"銘板彫刻",true),
                new Content("content4",OtherContribution,-1,"骨壺",true),
                new Content("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),
                new Content("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),
                new Content("content7",TravelExpense,-1,"3ヶ月交通費",true),
                new Content("content8",SuppliesExpense,-1,"コード、マウスパッド",true),
                new Content("content9",VehicleFee,-1,"ヴィッツ　ガソリン代",true),
                new Content("content10",InternalMaintenanceExpenses,-1,"花苗",true),
                new Content("content11",SuspensePayment,-1,"法事両替用",true),
                new Content("content12",Seresa,-1,"入金",true)
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

            CreditAccount shunjuen = new CreditAccount("credit_account1", "春秋苑", true);

            AccountingSubject OtherMiscellaneousIncome = new AccountingSubject("id0", "822", "その他雑収入", true);
            AccountingSubject OtherContribution = new AccountingSubject("id2", "822", "その他冥加金", true);
            AccountingSubject OtherTyadokoroIncome = new AccountingSubject("id3", "874", "その他茶所収入", true);
            AccountingSubject SuspenseReceiptMoney = new AccountingSubject("id4", "416", "仮受金", true);
            AccountingSubject TravelExpense = new AccountingSubject("id5", "611", "旅費交通費", true);
            AccountingSubject SuppliesExpense = new AccountingSubject("id6", "616", "消耗品費", true);
            AccountingSubject VehicleFee = new AccountingSubject("id7", "621", "車両費", true);
            AccountingSubject InternalMaintenanceExpenses = new AccountingSubject("id8", "735", "苑内整備費", true);
            AccountingSubject SuspensePayment = new AccountingSubject("id9", "168", "仮払金", true);
            AccountingSubject Seresa = new AccountingSubject("id10", "133","セレサ川崎普通貯金", true);

            ObservableCollection<ReceiptsAndExpenditure> list = new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content3",OtherMiscellaneousIncome,-1,"銘板彫刻",true),"山口家",5400,true,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content4",OtherContribution,-1,"骨壺",true),"坂村家",2000,true,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),string.Empty,900,true,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),string.Empty,1010000,true,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content7",TravelExpense,-1,"3ヶ月交通費",true),"坂本邦夫",12860,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content8",SuppliesExpense,-1,"コード、マウスパッド",true),"ヤマダ電機",2247,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content9",VehicleFee,-1,"ヴィッツ　ガソリン代",true),"アセント",4600,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content10",InternalMaintenanceExpenses,-1,"花苗",true),"ビバホーム",5827,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content11",SuspensePayment,-1,"法事両替用",true),"藤井泰子",120000,false,true,DateTime.Today,
                    repHayashiba,DateTime.Today),
                 new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content("content12",Seresa,-1,"入金",true),DateTime.Today.AddDays(-1).ToShortDateString(),120000,false,true,DateTime.Today,
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
