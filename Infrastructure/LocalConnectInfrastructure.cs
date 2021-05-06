using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure
{
    public class LocalConnectInfrastructure : IDataBaseConnect
    {
        public AccountingSubject CallAccountingSubject(string id) => 
            new AccountingSubject("accounitng_subject1", "000", "法事冥加", true);

        public Content CallContent(string id) => 
            new Content("content0", CallAccountingSubject("accounting_subject0"), 1000, "煙草", true);

        public CreditDept CallCreditDept(string id) => new CreditDept("credit_dept0", "春秋苑", true,true);

        public int CallFinalAccountPerMonth() => 0;

        public Rep CallRep(string id) => new Rep("rep0", "林飛 顕誠", "aaa", true, true);

        public Dictionary<int, string> GetSoryoList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>()
            {
                { 0, "小林 謙" },
                {1, "林飛 顕誠"},
                {2,"西村 守弘" },
                {3,"細江 洸" },
                {4,"安静 至邦" },
                {5,"石丸 恒平" },
                {6,"髙橋 証規" }
            };

            return list;
        }

        public int PreviousDayFinalAmount() => 0;

        public int ReceiptsAndExpenditurePreviousDayChange
            (ReceiptsAndExpenditure receiptsAndExpenditure) => 0;

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true),
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true),
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true),
                new AccountingSubject("accounting_subject3", "416", "仮受金", true),
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true),
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true),
                new AccountingSubject("accounting_subject6", "621", "車両費", true),
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true),
                new AccountingSubject("accounting_subject8", "168", "仮払金", true),
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true)
            };
            return list;
        }

        public ObservableCollection<AccountingSubject> 
            ReferenceAffiliationAccountingSubject(string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true),
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true),
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true),
                new AccountingSubject("accounting_subject3", "416", "仮受金", true),
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true),
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true),
                new AccountingSubject("accounting_subject6", "621", "車両費", true),
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true),
                new AccountingSubject("accounting_subject8", "168", "仮払金", true),
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true)
            };
            return list;
        }

        public ObservableCollection<Content> ReferenceContent
            (string contentText, string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly)
        {
            AccountingSubject OtherMiscellaneousIncome = 
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true);
            AccountingSubject OtherContribution = 
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true);
            AccountingSubject OtherTyadokoroIncome = 
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true);
            AccountingSubject SuspenseReceiptMoney =
                new AccountingSubject("accounting_subject3", "416", "仮受金", true);
            AccountingSubject TravelExpense = 
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true);
            AccountingSubject SuppliesExpense =
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true);
            AccountingSubject VehicleFee = new AccountingSubject("accounting_subject6", "621", "車両費", true);
            AccountingSubject InternalMaintenanceExpenses = 
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true);
            AccountingSubject SuspensePayment = 
                new AccountingSubject("accounting_subject8", "168", "仮払金", true);
            AccountingSubject Seresa =
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true);

            ObservableCollection<Content> list = new ObservableCollection<Content>
            {
                new Content("content3",OtherMiscellaneousIncome,-1,"管理料",true),
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

        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly, bool isShunjuenAccountOnly)
        {
            ObservableCollection<CreditDept> list = new ObservableCollection<CreditDept>
            {
                new CreditDept("credit_dept1","春秋苑",true,true),
                new CreditDept("credit_dept2","法務部",true,true),
                new CreditDept("credit_dept3","ホテル",false,false)
            };
            return list;
        }

        public ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept, 
                string content, string detail, string accountingSubject, string accountingSubjectCode, 
                bool whichDepositAndWithdrawalOnly, bool isPayment,bool isContainOutputted, 
                bool isValidityOnly, DateTime accountActivityDateStart, DateTime accountActivityDateEnd,
                DateTime outputDateStart,DateTime OutputDateEnd)
        {
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            CreditDept shunjuen = new CreditDept("credit_dept1", "春秋苑", true,true);
            CreditDept singyouji = new CreditDept("credit_dept2", "法務部", true, true);
            AccountingSubject Ofuse = new AccountingSubject("accounting_subject11", "815", "冥加読経料", true);
            //AccountingSubject OtherMiscellaneousIncome =
            //new AccountingSubject("accounting_subject0", "882", "その他雑収入", true);
            AccountingSubject OtherContribution =
                new AccountingSubject("accounting_subject2", "822", "その他冥加金", true);
            AccountingSubject OtherTyadokoroIncome = 
                new AccountingSubject("accounting_subject3", "874", "その他茶所収入", true);
            AccountingSubject SuspenseReceiptMoney = 
                new AccountingSubject("accounting_subject4", "416", "仮受金", true);
            AccountingSubject TravelExpense =
                new AccountingSubject("accounting_subject5", "611", "旅費交通費", true);
            AccountingSubject SuppliesExpense =
                new AccountingSubject("accounting_subject6", "616", "消耗品費", true);
            AccountingSubject VehicleFee = new AccountingSubject("accounting_subject7", "621", "車両費", true);
            AccountingSubject InternalMaintenanceExpenses = 
                new AccountingSubject("accounting_subject8", "735", "苑内整備費", true);
            AccountingSubject SuspensePayment =
                new AccountingSubject("accounting_subject9", "168", "仮払金", true);
            AccountingSubject Seresa =
                new AccountingSubject("accounting_subject10", "133","セレサ川崎普通貯金", true);

            ObservableCollection<ReceiptsAndExpenditure> returnList = 
                new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家 2021年度分",30000,true,true,
                    DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content4",OtherContribution,-1,"骨壺",true),"坂村家",2000,true,true,DateTime.Today,
                        DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today.AddDays(2),repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),string.Empty,900,true,true,
                        DateTime.Today.AddDays(1),DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"青蓮堂",shunjuen,
                    new Content
                    ("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),string.Empty,1010000,true,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content7",TravelExpense,-1,"3ヶ月交通費",true),"坂本邦夫",12860,false,true,DateTime.Today,
                        DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content8",SuppliesExpense,-1,"コード、マウスパッド",true),"ヤマダ電機",2247,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content9",VehicleFee,-1,"ヴィッツ　ガソリン代",true),"アセント",4600,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content10",InternalMaintenanceExpenses,-1,"花苗",true),"ビバホーム",5827,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content11",SuspensePayment,-1,"法事両替用",true),"藤井泰子",120000,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content12",Seresa,-1,"口座入金",true),DateTime.Today.AddDays(-1).ToShortDateString(),
                        3130000,false,true, DateTime.Today,DefaultDate,false)
            };
            return returnList;
        }

        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List) ReferenceReceiptsAndExpenditure
            (DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept, 
                string content, string detail, string accountingSubject, string accountingSubjectCode, 
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted, bool isValidityOnly, 
                DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart, 
                DateTime outputDateEnd, int pageCount)
        {
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            CreditDept shunjuen = new CreditDept("credit_dept1", "春秋苑", true, true);
            CreditDept singyouji = new CreditDept("credit_dept2", "法務部", true, true);
            AccountingSubject Ofuse = new AccountingSubject("accounting_subject11", "815", "冥加読経料", true);
            //AccountingSubject OtherMiscellaneousIncome =
            //new AccountingSubject("accounting_subject0", "882", "その他雑収入", true);
            AccountingSubject OtherContribution =
                new AccountingSubject("accounting_subject2", "822", "その他冥加金", true);
            AccountingSubject OtherTyadokoroIncome =
                new AccountingSubject("accounting_subject3", "874", "その他茶所収入", true);
            AccountingSubject SuspenseReceiptMoney =
                new AccountingSubject("accounting_subject4", "416", "仮受金", true);
            AccountingSubject TravelExpense =
                new AccountingSubject("accounting_subject5", "611", "旅費交通費", true);
            AccountingSubject SuppliesExpense =
                new AccountingSubject("accounting_subject6", "616", "消耗品費", true);
            AccountingSubject VehicleFee = new AccountingSubject("accounting_subject7", "621", "車両費", true);
            AccountingSubject InternalMaintenanceExpenses =
                new AccountingSubject("accounting_subject8", "735", "苑内整備費", true);
            AccountingSubject SuspensePayment =
                new AccountingSubject("accounting_subject9", "168", "仮払金", true);
            AccountingSubject Seresa =
                new AccountingSubject("accounting_subject10", "133", "セレサ川崎普通貯金", true);

            ObservableCollection<ReceiptsAndExpenditure> returnList =
                new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家 2021年度分",30000,true,true,
                    DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content4",OtherContribution,-1,"骨壺",true),"坂村家",2000,true,true,DateTime.Today,
                        DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today.AddDays(2),repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),string.Empty,900,true,true,
                        DateTime.Today.AddDays(1),DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"青蓮堂",shunjuen,
                    new Content
                    ("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),string.Empty,1010000,true,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content7",TravelExpense,-1,"3ヶ月交通費",true),"坂本邦夫",12860,false,true,DateTime.Today,
                        DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content8",SuppliesExpense,-1,"コード、マウスパッド",true),"ヤマダ電機",2247,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content9",VehicleFee,-1,"ヴィッツ　ガソリン代",true),"アセント",4600,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content10",InternalMaintenanceExpenses,-1,"花苗",true),"ビバホーム",5827,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content11",SuspensePayment,-1,"法事両替用",true),"藤井泰子",120000,false,true,
                        DateTime.Today,DefaultDate,false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content12",Seresa,-1,"口座入金",true),DateTime.Today.AddDays(-1).ToShortDateString(),
                        3130000,false,true, DateTime.Today,DefaultDate,false)
            };
            return (1, returnList);
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

        public int Registration(Rep rep) => 1;

        public int Registration(AccountingSubject accountingSubject) => 1;

        public int Registration(CreditDept creditDept) => 1;

        public int Registration(Content content) => 1;

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure) => 1;

        public int RegistrationPerMonthFinalAccount() => 1;
        
        public int Update(Rep rep) => 1;

        public int Update(AccountingSubject accountingSubject) => 1;

        public int Update(CreditDept creditDept) => 1;

        public int Update(Content content) => 1;

        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure) => 1;
    }
}
