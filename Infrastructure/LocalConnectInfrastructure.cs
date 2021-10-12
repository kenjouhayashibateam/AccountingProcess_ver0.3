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
        public AccountingSubject CallAccountingSubject(string id)
        { return new AccountingSubject("accounitng_subject1", "000", "法事冥加",true, true); }

        public Content CallContent(string id)
        { return new Content("content0", CallAccountingSubject("accounting_subject0"), 1000, "煙草", true); }

        public CreditDept CallCreditDept(string id)
        { return new CreditDept("credit_dept0", "春秋苑", true, true); }

        public int CallFinalAccountPerMonth() { return 0; }

        public int CallFinalAccountPerMonth(DateTime monthEnd) { return 10000; }

        public Rep CallRep(string id) { return new Rep("rep0", "林飛 顕誠", GetHashValue("aaa", "rep0"), true, true); }

        public int DeleteConvertContent(Content convertContent) { return 1; }

        public Dictionary<int, string> GetSoryoList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>()
            {
                {0, "小林 謙" },
                {1, "林飛 顕誠"},
                {2,"西村 守弘" },
                {3,"細江 洸" },
                {4,"安静 至邦" },
                {5,"石丸 恒平" },
                {6,"髙橋 証規" }
            };

            return list;
        }

        public int PreviousDayFinalAmount() { return 0; }

        public int ReceiptsAndExpenditurePreviousDayChange
            (ReceiptsAndExpenditure receiptsAndExpenditure)
        { return 0; }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isShunjuen, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true, true),
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true, true),
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true, true),
                new AccountingSubject("accounting_subject3", "416", "仮受金", true, true),
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true, true),
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true, true),
                new AccountingSubject("accounting_subject6", "621", "車両費", true, true),
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true, true),
                new AccountingSubject("accounting_subject8", "168", "仮払金", true, true),
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true, true)
            };
            return list;
        }

        public ObservableCollection<AccountingSubject> 
            ReferenceAffiliationAccountingSubject(string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>
            {
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true, true),
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true, true),
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true, true),
                new AccountingSubject("accounting_subject3", "416", "仮受金", true, true),
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true, true),
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true, true),
                new AccountingSubject("accounting_subject6", "621", "車両費", true, true),
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true, true),
                new AccountingSubject("accounting_subject8", "168", "仮払金", true, true),
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true, true)
            };
            return list;
        }

        public (int TotalRows, ObservableCollection<Condolence> List) ReferenceCondolence
            (DateTime startDate, DateTime endDate, string location, int pageCount)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>()
            {
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
            };

            return (list.Count, list);
        }

        public ObservableCollection<Condolence> ReferenceCondolence
            (DateTime startDate, DateTime endDate, string location)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>()
            {
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
                new Condolence
                    (1,"管理事務所","あああ家","林飛","法事",10000,5000,0,0,1000,"佐野商店",DateTime.Today,"深瀬",
                        string.Empty),
                new Condolence
                    (2,"管理事務所","いいい家","安田","葬儀",50000,0,0,5000,5000,string.Empty,DateTime.Today,
                        string.Empty,"藤井"),
                new Condolence
                    (3,"管理事務所","ううう家","林飛 顕誠","法事",300000,10000,10000,0,0,"大成祭典",
                        DateTime.Today.AddDays(-1),"藤井",string.Empty),
                new Condolence
                    (4,"管理事務所","えええ家","安田 一貴","法事",50000,10000,5000,0,0,string.Empty,
                        DateTime.Today.AddDays(-1),string.Empty,"深瀬"),
            };

            return list;
        }

        public ObservableCollection<Content> ReferenceContent
            (string contentText, string accountingSubjectCode, string accountingSubject, bool isShunjuen,
                bool isValidityTrueOnly)
        {
            AccountingSubject OtherMiscellaneousIncome = 
                new AccountingSubject("accounting_subject0", "882", "その他雑収入", true, true);
            AccountingSubject OtherContribution = 
                new AccountingSubject("accounting_subject1", "822", "その他冥加金", true, true);
            AccountingSubject OtherTyadokoroIncome = 
                new AccountingSubject("accounting_subject2", "874", "その他茶所収入", true, true);
            AccountingSubject SuspenseReceiptMoney =
                new AccountingSubject("accounting_subject3", "416", "仮受金", true, true);
            AccountingSubject TravelExpense = 
                new AccountingSubject("accounting_subject4", "611", "旅費交通費", true, true);
            AccountingSubject SuppliesExpense =
                new AccountingSubject("accounting_subject5", "616", "消耗品費", true, true);
            AccountingSubject VehicleFee = new AccountingSubject
                ("accounting_subject6", "621", "車両費", true, true);
            AccountingSubject InternalMaintenanceExpenses = 
                new AccountingSubject("accounting_subject7", "735", "苑内整備費", true, true);
            AccountingSubject SuspensePayment = 
                new AccountingSubject("accounting_subject8", "168", "仮払金", true, true);
            AccountingSubject Seresa =
                new AccountingSubject("accounting_subject9", "133", "セレサ川崎普通貯金", true, true);

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

        public string CallContentConvertText(string id) { return null; }

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
                string content, string detail, string accountingSubject, string accountingSubjectCode, bool isShunjuen,
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted,
                bool isValidityOnly, DateTime accountActivityDateStart, DateTime accountActivityDateEnd,
                DateTime outputDateStart, DateTime OutputDateEnd)
        {
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            CreditDept singyouji = new CreditDept("credit_dept2", "法務部", true, true);
            AccountingSubject Ofuse = new AccountingSubject
                ("accounting_subject2", "822", "冥加読経料", true, true);
            AccountingSubject Kanri = new AccountingSubject
                ("accounting_subject2", "832", "懇志読経料", true, true);

            ObservableCollection<ReceiptsAndExpenditure> returnList =
                new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家1 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家2 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Kanri,-1,"管理料",true),"山口家3 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家4 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家5 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家6 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Kanri,-1,"管理料",true),"山口家7 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家8 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家9 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家10 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家11 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                    new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家12 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家13 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家14 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家15 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家16 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家17 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家18 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家19 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家20 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家21 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家22 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content4",Ofuse,-1,"管理料",true),"山口家23 2021年度分",30000,false,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false)//,
                };
            return returnList;
        }

        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List)
            ReferenceReceiptsAndExpenditure(DateTime registrationDateStart, DateTime registrationDateEnd,
                string location, string creditDept, string content, string detail, string accountingSubject,
                string accountingSubjectCode, bool isShunjuen, bool whichDepositAndWithdrawalOnly,
                bool isPayment, bool isContainOutputted, bool isValidityOnly, DateTime accountActivityDateStart,
                DateTime accountActivityDateEnd, DateTime outputDateStart, DateTime outputDateEnd, int pageCount,
                string sortColumn, bool sortDirection)
        {
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            CreditDept shunjuen = new CreditDept("credit_dept1", "春秋苑", true, true);
            CreditDept singyouji = new CreditDept("credit_dept2", "法務部", true, true);
            AccountingSubject Ofuse = new AccountingSubject
                ("accounting_subject11", "815", "冥加読経料", true, true);
            AccountingSubject OtherContribution =
                new AccountingSubject("accounting_subject2", "822", "その他冥加金", true, true);
            AccountingSubject OtherTyadokoroIncome =
                new AccountingSubject("accounting_subject3", "874", "その他茶所収入", true, true);
            AccountingSubject SuspenseReceiptMoney =
                new AccountingSubject("accounting_subject4", "416", "仮受金", true, true);
            AccountingSubject TravelExpense =
                new AccountingSubject("accounting_subject5", "611", "旅費交通費", true, true);
            AccountingSubject SuppliesExpense =
                new AccountingSubject("accounting_subject6", "616", "消耗品費", true, true);
            AccountingSubject VehicleFee = new AccountingSubject
                ("accounting_subject7", "621", "車両費", true, true);
            AccountingSubject InternalMaintenanceExpenses =
                new AccountingSubject("accounting_subject8", "735", "苑内整備費", true, true);
            AccountingSubject SuspensePayment =
                new AccountingSubject("accounting_subject9", "168", "仮払金", true, true);
            AccountingSubject Seresa =
                new AccountingSubject("accounting_subject10", "133", "セレサ川崎普通貯金", true, true);

            ObservableCollection<ReceiptsAndExpenditure> returnList =
                new ObservableCollection<ReceiptsAndExpenditure>
            {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),true),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content4",OtherContribution,-1,"管理料",true),"坂村家",2000,true,true,DateTime.Today,
                        DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today.AddDays(2),repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),string.Empty,900,true,true,
                        DateTime.Today.AddDays(1),DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"青蓮堂",shunjuen,
                    new Content
                    ("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),string.Empty,1010000,true,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content7",TravelExpense,-1,"3ヶ月交通費",true),"坂本邦夫",12860,false,true,DateTime.Today,
                        DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content8",SuppliesExpense,-1,"コード、マウスパッド",true),"ヤマダ電機",2247,false,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content9",VehicleFee,-1,"ヴィッツ　ガソリン代",true),"アセント",4600,false,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content10",InternalMaintenanceExpenses,-1,"花苗",true),"ビバホーム",5827,false,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content11",SuspensePayment,-1,"法事両替用",true),"藤井泰子",120000,false,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content12",Seresa,-1,"口座入金",true),DateTime.Today.AddDays(-1).ToShortDateString(),
                        3130000,false,true, DateTime.Today,DateTime.Today.AddDays(-1),false)
            };
            return (returnList.Count, returnList);
        }

        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidity)
        {
            ObservableCollection<Rep> list = new ObservableCollection<Rep>
            {
                new Rep("rep1", "林飛 顕誠", GetHashValue("aaa","rep1"), true,true),
                new Rep("rep2", "秋間 大樹", "bbb", false,false)
            };

            return list;
        }

        public int Registration(Rep rep) { return 1; }

        public int Registration(AccountingSubject accountingSubject) { return 1; }

        public int Registration(CreditDept creditDept) { return 1; }

        public int Registration(Content content) { return 1; }

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure) { return 1; }

        public int Registration(Condolence condolence) { return 1; }

        public int Registration(string id, string contentConvertText) { return 1; }

        public int RegistrationPrecedingYearFinalAccount() { return 1; }

        public int Update(Rep rep) { return 1; }

        public int Update(AccountingSubject accountingSubject) { return 1; }

        public int Update(CreditDept creditDept) { return 1; }

        public int Update(Content content) { return 1; }

        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure) { return 1; }

        public int Update(Condolence condolence) { return 1; }

        public int Update(string id, string contentConvertText) { return 1; }

        public int Registration(Voucher voucher) { return 1; }

        public Voucher CallLatestVoucher()
        {
            return new Voucher(0, "ろーかる",
                ReferenceReceiptsAndExpenditure(DateTime.Today, DateTime.Today, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, false, true, true, true, true,
                DateTime.Today, DateTime.Today, DateTime.Today, DateTime.Today),
                DateTime.Today, CallRep(string.Empty), true);
        }

        public int Registration(int voucherID, int receiptsAndExpenditureID) { return 1; }

        public Content CallLatestContent()
        {
            return new Content
                ("content3", CallAccountingSubject(string.Empty), -1, "管理料", true);
        }

        public int Update(Voucher voucher) { return 1; }

        public ObservableCollection<Voucher> ReferenceVoucher
            (DateTime outputDateStart, DateTime outputDateEnd, bool isValidityTrueOnly)
        {
            ObservableCollection<Voucher> list = new ObservableCollection<Voucher>()
            {
                { new Voucher(1, "あああ",CallVoucherGroupingReceiptsAndExpenditure(0), DefaultDate, 
                    CallRep(string.Empty), true) }
            };
            return list;
        }

        public ObservableCollection<ReceiptsAndExpenditure> 
            CallVoucherGroupingReceiptsAndExpenditure(int voucherID)
        {
            Rep repAkima = new Rep("rep2", "秋間 大樹", "bbb", true, false);

            CreditDept shunjuen = new CreditDept("credit_dept1", "春秋苑", true, true);
            CreditDept singyouji = new CreditDept("credit_dept2", "法務部", true, true);
            AccountingSubject Ofuse = new AccountingSubject
                ("accounting_subject11", "815", "冥加読経料", true, true);
            AccountingSubject OtherContribution =
                new AccountingSubject("accounting_subject2", "822", "その他冥加金", true, true);
            AccountingSubject OtherTyadokoroIncome =
                new AccountingSubject("accounting_subject3", "874", "その他茶所収入", true, true);
            AccountingSubject SuspenseReceiptMoney =
                new AccountingSubject("accounting_subject4", "416", "仮受金", true, true);

            ObservableCollection<ReceiptsAndExpenditure> returnList =
                new ObservableCollection<ReceiptsAndExpenditure>
                {
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",singyouji,
                    new Content
                    ("content3",Ofuse,-1,"管理料",true),"山口家 2021年度分",30000,true,true,
                    DateTime.Today,DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content4",OtherContribution,-1,"骨壺",true),"坂村家",2000,true,true,DateTime.Today,
                        DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today.AddDays(2),repAkima,"管理事務所",shunjuen,
                    new Content
                    ("content5",OtherTyadokoroIncome,-1,"ビール、ライター",true),string.Empty,900,true,true,
                        DateTime.Today.AddDays(1),DateTime.Today.AddDays(-1),false),
                new ReceiptsAndExpenditure(2,DateTime.Today,repAkima,"青蓮堂",shunjuen,
                    new Content
                    ("content6",SuspenseReceiptMoney,-1,"ワイズコア",true),string.Empty,1010000,true,true,
                        DateTime.Today,DateTime.Today.AddDays(-1),false),
                };

            return returnList;
        }

        public int Delete(Condolence condolence) { return 1; }

        public CreditDept CallContentDefaultCreditDept(Content content)
        {
            return CallCreditDept(string.Empty);
        }

        public int Registration(CreditDept creditDept, Content content) { return 1; }

        public int Update(CreditDept creditDept, Content content) { return 1; }

        public int DeleteContentDefaultCreditDept(Content content) { return 1; }

        public string GetBranchNumber(AccountingSubject accountingSubject)
        { return "001"; }

        public int Update(AccountingSubject accountingSubject, string branchNumber)
        { return 1; }

        public int DeleteBranchNumber(AccountingSubject accountingSubject)
        { return 1; }
    }
}
