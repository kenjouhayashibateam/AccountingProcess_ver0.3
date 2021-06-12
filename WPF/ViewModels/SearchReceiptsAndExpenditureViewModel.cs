using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納データ検索画面ViewModel
    /// </summary>
    public class SearchReceiptsAndExpenditureViewModel : BaseViewModel, IPagenationObserver
    {
        #region Properties
        private string searchAccountingSubject = string.Empty;
        private string listTotalCountInfo;
        private string paymentTotalAmountWithUnit;
        private string withdwaralTotalAmountWithUnit;
        private string listTotalAmountWithUnit;
        #region Bools
        private bool isAllData = true;
        private bool isPaymentOnly;
        private bool isWithdrawalOnly;
        private bool isIndiscriminateDept = false;
        private bool isIndiscriminateAccountingSubject = false;
        private bool isCreditDeptsEnabled = true;
        private bool isAccountingSubjectsEnabled = true;
        #endregion
        private DateTime searchStartDate = DateTime.Today;
        private DateTime searchEndDate = DateTime.Today;
        private ObservableCollection<CreditDept> creditDepts;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private CreditDept selectedCreditDept = new CreditDept(string.Empty, string.Empty, true, true);
        private Pagination pagination = Pagination.GetPagination();
        #endregion

        public SearchReceiptsAndExpenditureViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            Pagination.Add(this);
            CreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
            SelectedCreditDept = CreditDepts[0];
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            SearchEndDate = DateTime.Today;
            SearchStartDate = DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1));
            InputAllPeriodCommand = new DelegateCommand(() => InputAllPeriod(), () => true);
            Pagination.SortDirectionIsASC = false;
        }
        public SearchReceiptsAndExpenditureViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 期間検索欄の日付を一番広い期間に設定するコマンド
        /// </summary>
        public DelegateCommand InputAllPeriodCommand { get; }
        private void InputAllPeriod()
        {
            SearchStartDate = DefaultDate.AddDays(1);
            SearchEndDate = DateTime.Today;
            CreateReceiptsAndExpenditures(true);
        }
        /// <summary>
        /// 全てのデータを表示するチェック
        /// </summary>
        public bool IsAllData
        {
            get => isAllData;
            set
            {
                isAllData = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金データのみ表示するチェック
        /// </summary>
        public bool IsPaymentOnly
        {
            get => isPaymentOnly;
            set
            {
                isPaymentOnly = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出金データのみ表示するチェック
        /// </summary>
        public bool IsWithdrawalOnly
        {
            get => isWithdrawalOnly;
            set
            {
                isWithdrawalOnly = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索最古日
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                searchStartDate = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索最新日
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                searchEndDate = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門を無差別に検索するチェック
        /// </summary>
        public bool IsIndiscriminateDept
        {
            get => isIndiscriminateDept;
            set
            {
                isIndiscriminateDept = value;
                IsCreditDeptsEnabled = !value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門リスト
        /// </summary>
        public ObservableCollection<CreditDept> CreditDepts
        {
            get => creditDepts;
            set
            {
                creditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方部門
        /// </summary>
        public CreditDept SelectedCreditDept
        {
            get => selectedCreditDept;
            set
            {
                selectedCreditDept = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目を無差別に検索するチェック
        /// </summary>
        public bool IsIndiscriminateAccountingSubject
        {
            get => isIndiscriminateAccountingSubject;
            set
            {
                isIndiscriminateAccountingSubject = value;
                IsAccountingSubjectsEnabled = !value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> AccountingSubjects
        {
            get => accountingSubjects;
            set
            {
                accountingSubjects = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する勘定科目
        /// </summary>
        public string SearchAccountingSubject
        {
            get => searchAccountingSubject;
            set
            {
                searchAccountingSubject = value;
                CreateReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門リストのEnabled
        /// </summary>
        public bool IsCreditDeptsEnabled
        {
            get => isCreditDeptsEnabled;
            set
            {
                isCreditDeptsEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目リストのEnabled
        /// </summary>
        public bool IsAccountingSubjectsEnabled
        {
            get => isAccountingSubjectsEnabled;
            set
            {
                isAccountingSubjectsEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ページネーション
        /// </summary>
        public Pagination Pagination
        {
            get => pagination;
            set
            {
                pagination = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures
        {
            get => receiptsAndExpenditures;
            set
            {
                receiptsAndExpenditures = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// リストのデータ件数
        /// </summary>
        public string ListTotalCountInfo
        {
            get => listTotalCountInfo;
            set
            {
                listTotalCountInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金総額
        /// </summary>
        public string PaymentTotalAmountWithUnit
        {
            get => paymentTotalAmountWithUnit;
            set
            {
                paymentTotalAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出金総額
        /// </summary>
        public string WithdwaralTotalAmountWithUnit
        {
            get => withdwaralTotalAmountWithUnit;
            set
            {
                withdwaralTotalAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// リスト総額
        /// </summary>
        public string ListTotalAmountWithUnit
        {
            get => listTotalAmountWithUnit;
            set
            {
                listTotalAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        private void CreateReceiptsAndExpenditures(bool isPageReset)
        {
            Pagination.CountReset(isPageReset);
            string creditDept = IsIndiscriminateDept ? string.Empty : SelectedCreditDept.Dept;

            (int count, ObservableCollection<ReceiptsAndExpenditure> list) = DataBaseConnect.ReferenceReceiptsAndExpenditure
                (DefaultDate, DateTime.Today, string.Empty, creditDept, string.Empty, string.Empty, SearchAccountingSubject,
                    string.Empty, !IsAllData, IsPaymentOnly, true, true, SearchStartDate, SearchEndDate, DefaultDate.AddDays(1), 
                    DateTime.Today, Pagination.PageCount, Pagination.SelectedSortColumn, Pagination.SortDirectionIsASC);
            Pagination.TotalRowCount = count;
            ListTotalCountInfo = $"{count}{Space}件";
            ReceiptsAndExpenditures = list;
            Pagination.SetProperty();
            
            int payment = 0;
            int withdrawal = 0;

            foreach(ReceiptsAndExpenditure rae in DataBaseConnect.ReferenceReceiptsAndExpenditure
                (DefaultDate, DateTime.Today, string.Empty, creditDept, string.Empty, string.Empty,
                    SearchAccountingSubject, string.Empty, !IsAllData, IsPaymentOnly, true, true, SearchStartDate,
                    SearchEndDate, DefaultDate.AddDays(1), DateTime.Today))
            {
                if (rae.IsPayment) payment += rae.Price;
                else withdrawal += rae.Price;
            }
            PaymentTotalAmountWithUnit = AmountWithUnit(payment);
            WithdwaralTotalAmountWithUnit = AmountWithUnit(withdrawal);
            ListTotalAmountWithUnit = AmountWithUnit(payment - withdrawal);
        }

        public override void ValidationProperty(string propertyName, object value) {}

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = $"出納データ閲覧";

        public void SortNotify() => CreateReceiptsAndExpenditures(true);

        public void PageNotify() => CreateReceiptsAndExpenditures(false);

        public void SetSortColumns() => Pagination.SortColumns =
            new System.Collections.Generic.Dictionary<int, string>()
            {
                {0,"ID" },
                {1,"入出金日" },
                {2,"科目コード" }
            };
    }
}
