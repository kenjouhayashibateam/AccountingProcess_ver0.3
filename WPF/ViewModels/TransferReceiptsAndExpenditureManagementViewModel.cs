using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.ExcelOutputData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    public class TransferReceiptsAndExpenditureManagementViewModel : BaseViewModel,
        IPagenationObserver, ITransferReceiptsAndExpenditureOperationObserver, IClosing
    {
        #region Properties
        #region Bools
        private bool isLimitCreditDept;
        private bool isPeriodSearch = true;
        private bool isLocationSearch = true;
        private bool isValidityTrueOnly;
        private bool isContainOutputted;
        private bool isLimitDebitAccount;
        private bool isLimitCreditAccount;
        private bool isCloseCancel = false;
        private bool isChangeOutputDate;
        private bool isOutputDataSelect;
        #endregion
        #region Strings
        private string referenceLocationCheckContent;
        private string searchCreditAccountCode = string.Empty;
        private string searchDebitAccountCode = string.Empty;
        private string totalAmountDisplayValue;
        private string outputContent = "出力";
        #endregion
        private DateTime searchStartDate = DateTime.Today.AddDays((-1*DateTime.Today.Day)+1);
        private DateTime searchEndDate = DateTime.Today;
        private DateTime changeOutputDate = DateTime.Today;
        #region ObservableCollections
        private ObservableCollection<CreditDept> creditDepts;
        private ObservableCollection<ListItem> transferReceiptsAndExpenditures;
        private ObservableCollection<AccountingSubject> searchDebitAccounts =
            new ObservableCollection<AccountingSubject>();
        private ObservableCollection<AccountingSubject> searchCreditAccounts =
            new ObservableCollection<AccountingSubject>();
        /// <summary>
        /// 検索結果の全データリスト
        /// </summary>
        private ObservableCollection<ListItem> AllDataList = new ObservableCollection<ListItem>();
        #endregion
        #region ValueObjects
        private CreditDept selectedCreditDept;
        private Pagination pagination;
        private AccountingSubject selectedDebitAccount;
        private AccountingSubject selectedCreditAccount;
        #endregion
        /// <summary>
        /// リストの総金額
        /// </summary>
        private int TotalAmount;
        private readonly IDataOutput DataOutput;
        private ListItem selectedListItem;
        #endregion

        public TransferReceiptsAndExpenditureManagementViewModel
            (IDataBaseConnect dataBaseConnect,IDataOutput dataOutput) : base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            Pagination.SortDirectionIsASC = false;
            TransferReceiptsAndExpenditureOperation.GetInstance().Add(this);
            ShowTransferReceiptsAndExpenditureOperationCommand = new DelegateCommand
                (() => ShowTransferReceiptsAndExpenditureOperation(), () => true); 
            RefreshListCommand = new DelegateCommand(() => RefreshList(), () => true);
            ShowDetailCommand = new DelegateCommand(() => ShowDetail(), () => true);
            OutputCommand = new DelegateCommand(() => Output(), () => true);
            SetTotalAmountCommand = new DelegateCommand(() => SetTotalAmount(), () => true);
            CreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, 
                AccountingProcessLocation.IsAccountingGenreShunjuen);
            SelectedCreditDept = CreditDepts[0];
            ReferenceLocationCheckContent =
                $"経理担当場所：{AccountingProcessLocation.Location}のデータのみを表示する";
            IsLocationSearch = true;
            IsPeriodSearch = true;
        }
        public TransferReceiptsAndExpenditureManagementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect(), new ExcelOutputInfrastructure()) { }
        /// <summary>
        /// 総金額を設定するコマンド
        /// </summary>
        public DelegateCommand SetTotalAmountCommand { get; set; }
        /// <summary>
        /// 出力コマンド
        /// </summary>
        public DelegateCommand OutputCommand { get; set; }
        private async void Output()
        {
            OutputContent = "出力中";
            IsCloseCancel = true;

            await Task.Run(() => SlipsOutputProcess());
            IsCloseCancel = false;
            OutputContent = "出力";

            void SlipsOutputProcess()
            {
                ObservableCollection<TransferReceiptsAndExpenditure> list =
                    new ObservableCollection<TransferReceiptsAndExpenditure>();

                if (IsOutputDataSelect) { SetSelectedList(); }
                else { SetAllList(); }

                DataOutput.TransferSlips(list);

                if (IsChangeOutputDate)
                {
                    foreach (TransferReceiptsAndExpenditure trae in list)
                    {
                        trae.OutputDate = ChangeOutputDate;
                        _ = DataBaseConnect.Update(trae);
                    }
                }
                else
                {
                    DataOutput.TransferSlips(list);
                    foreach (TransferReceiptsAndExpenditure trae in list)
                    {
                        trae.OutputDate = ChangeOutputDate;
                        _ = DataBaseConnect.Update(trae);
                    }
                }

                void SetAllList()
                { foreach (ListItem li in AllDataList) { list.Add(li.Data); } }

                void SetSelectedList()
                {
                    foreach (ListItem li in TransferReceiptsAndExpenditures)
                    {
                        if (!li.IsOutput) { continue; }

                        list.Add(li.Data);
                    }
                }
            }
        }
        /// <summary>
        /// 詳細表示コマンド
        /// </summary>
        public DelegateCommand ShowDetailCommand { get; set; }
        private async void ShowDetail()
        {
            await Task.Delay(1);
            TransferReceiptsAndExpenditureOperation.GetInstance().SetData(SelectedListItem.Data);
            CreateShowWindowCommand(ScreenTransition.TransferReceiptsAndExpenditureOperationView());
        }
        /// <summary>
        /// 検索結果再読み込みコマンド
        /// </summary>
        public DelegateCommand RefreshListCommand { get; set; }
        private void RefreshList()
        { ReferenceTransferReceiptsAndExpenditure(true); }
        /// <summary>
        /// 振替データリストを検索します
        /// </summary>
        /// <param name="isPageReset">ページをリセットするか</param>
        private void ReferenceTransferReceiptsAndExpenditure(bool isPageReset)
        {
            Pagination.CountReset(isPageReset);
            
            string dept = IsLimitCreditDept ? SelectedCreditDept.Dept : string.Empty;
            string location = IsLocationSearch ? AccountingProcessLocation.Location.ToString() : string.Empty;
            string creditCode = SearchCreditAccountCode.Length == 3 ? 
                SearchCreditAccountCode : string.Empty;
            string credit = SelectedCreditAccount == null ? string.Empty : SelectedCreditAccount.Subject;
            string debitCode = SearchDebitAccountCode.Length == 3 ? SearchDebitAccountCode : string.Empty;
            string debit = SelectedDebitAccount == null ? string.Empty : SelectedDebitAccount.Subject;
            DateTime outputEnd = IsContainOutputted ? DateTime.Now : DefaultDate;

            ObservableCollection<TransferReceiptsAndExpenditure> list;

            list =
                DataBaseConnect.ReferenceTransferReceiptsAndExpenditure
                (AccountingProcessLocation.IsAccountingGenreShunjuen, SearchStartDate, SearchEndDate,
                    location, dept, debitCode, debit, creditCode, credit, IsValidity, IsContainOutputted, 
                    DefaultDate, outputEnd, Pagination.PageCount, Pagination.SelectedSortColumn, 
                    Pagination.SortDirectionIsASC, Pagination.CountEachPage);

            TransferReceiptsAndExpenditures = new ObservableCollection<ListItem>();

            foreach (TransferReceiptsAndExpenditure trae in list)
            {
                TransferReceiptsAndExpenditures.Add(new ListItem(!IsOutputDataSelect, trae));
            }

            list =
                DataBaseConnect.ReferenceTransferReceiptsAndExpenditure
                (AccountingProcessLocation.IsAccountingGenreShunjuen, SearchStartDate, SearchEndDate,
                    location, dept, debitCode, debit, creditCode, credit, IsValidity, IsContainOutputted, 
                    DefaultDate, outputEnd);

            AllDataList = new ObservableCollection<ListItem>();
            foreach (TransferReceiptsAndExpenditure trae in list)
            {
                AllDataList.Add(new ListItem(true, trae));
            }

            Pagination.TotalRowCount = AllDataList.Count;
            Pagination.SetProperty();

            TotalAmount = 0;
            if (IsChangeOutputDate)
            {
                foreach (ListItem listItem in TransferReceiptsAndExpenditures)
                { TotalAmount += listItem.Data.Price; }
            }
            else
            { foreach (ListItem trae in AllDataList) { TotalAmount += trae.Data.Price; } }
            SetTotalAmount();
        }
        private void SetTotalAmount()
        {
            ObservableCollection<ListItem> list = IsOutputDataSelect ? TransferReceiptsAndExpenditures : AllDataList;
            TotalAmount = 0;
            foreach (ListItem listItem in list)
            {
                if (!listItem.IsOutput) { continue; }
                TotalAmount += listItem.Data.Price;
            }
            TotalAmountDisplayValue = $"リストの総金額：{AmountWithUnit(TotalAmount)}"; 
        }
        /// <summary>
        /// 振替伝票操作画面呼び出しコマンド
        /// </summary>
        public DelegateCommand ShowTransferReceiptsAndExpenditureOperationCommand { get; set; }
        private void ShowTransferReceiptsAndExpenditureOperation()
        {
            TransferReceiptsAndExpenditureOperation.GetInstance().SetData(null);
            CreateShowWindowCommand(ScreenTransition.TransferReceiptsAndExpenditureOperationView()); 
        }
        /// <summary>
        /// 検索で貸方部門を限定するか
        /// </summary>
        public bool IsLimitCreditDept
        {
            get => isLimitCreditDept;
            set
            {
                isLimitCreditDept = value;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
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
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 期間検索チェック
        /// </summary>
        public bool IsPeriodSearch
        {
            get => isPeriodSearch;
            set
            {
                isPeriodSearch = value;
                CallPropertyChanged();
                if (value) { SearchEndDate = DateTime.Now; }
                else { SearchEndDate = SearchStartDate; }
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 検索開始日
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                searchStartDate = value < SearchEndDate ? value : SearchEndDate;
                CallPropertyChanged();
                if (!IsPeriodSearch) { SearchEndDate = value; }
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 検索最終日
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                searchEndDate = value > SearchStartDate ? value : SearchStartDate;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 検索を経理担当場所に限定するかのContent
        /// </summary>
        public string ReferenceLocationCheckContent
        {
            get => referenceLocationCheckContent;
            set
            {
                referenceLocationCheckContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索を経理担当場所に限定するか
        /// </summary>
        public bool IsLocationSearch
        {
            get => isLocationSearch;
            set
            {
                isLocationSearch = value;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 検索するデータの有効性
        /// </summary>
        public bool IsValidity
        {
            get => isValidityTrueOnly;
            set
            {
                isValidityTrueOnly = value;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 出力したものも表示するか
        /// </summary>
        public bool IsContainOutputted
        {
            get => isContainOutputted;
            set
            {
                isContainOutputted = value;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 検索結果リスト
        /// </summary>
        public ObservableCollection<ListItem> TransferReceiptsAndExpenditures
        {
            get => transferReceiptsAndExpenditures;
            set
            {
                transferReceiptsAndExpenditures = value;
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
        /// 検索する貸方勘定コード
        /// </summary>
        public string SearchCreditAccountCode
        {
            get => searchCreditAccountCode;
            set
            {
                searchCreditAccountCode = value;
                CallPropertyChanged();
                if (value.Length != 3)
                {
                    SearchCreditAccounts.Clear();
                    return;
                }
                ReferenceTransferReceiptsAndExpenditure(true);
                SearchCreditAccounts = DataBaseConnect.ReferenceAccountingSubject
                    (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                SelectedCreditAccount = SearchCreditAccounts.Count > 0 ? SearchCreditAccounts[0] : null;
            }
        }
        /// <summary>
        /// 検索する借方勘定コード
        /// </summary>
        public string SearchDebitAccountCode
        {
            get => searchDebitAccountCode;
            set
            {
                searchDebitAccountCode = value;
                CallPropertyChanged();
                if (value.Length != 3) 
                {
                    SearchDebitAccounts.Clear();
                    return; 
                }
                ReferenceTransferReceiptsAndExpenditure(true);
                SearchDebitAccounts = DataBaseConnect.ReferenceAccountingSubject
                    (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                SelectedDebitAccount = SearchDebitAccounts.Count > 0 ? SearchDebitAccounts[0] : null;
            }
        }
        /// <summary>
        /// 検索する借方勘定リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> SearchDebitAccounts
        {
            get => searchDebitAccounts;
            set
            {
                searchDebitAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された借方勘定
        /// </summary>
        public AccountingSubject SelectedDebitAccount
        {
            get => selectedDebitAccount;
            set
            {
                selectedDebitAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 借方勘定を限定するか
        /// </summary>
        public bool IsLimitDebitAccount
        {
            get => isLimitDebitAccount;
            set
            {
                isLimitDebitAccount = value;
                CallPropertyChanged();
                if (!value) { SelectedDebitAccount = null; }
            }
        }
        /// <summary>
        /// 検索する貸方勘定リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> SearchCreditAccounts
        {
            get => searchCreditAccounts;
            set
            {
                searchCreditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方勘定
        /// </summary>
        public AccountingSubject SelectedCreditAccount
        {
            get => selectedCreditAccount;
            set
            {
                selectedCreditAccount = value;
                CallPropertyChanged();
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 貸方勘定を限定するか
        /// </summary>
        public bool IsLimitCreditAccount
        {
            get => isLimitCreditAccount;
            set
            {
                isLimitCreditAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// リストの総金額表示
        /// </summary>
        public string TotalAmountDisplayValue
        {
            get => totalAmountDisplayValue;
            set
            {
                totalAmountDisplayValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出直機能Content
        /// </summary>
        public string OutputContent
        {
            get => outputContent;
            set
            {
                outputContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウィンドウを閉じるのをキャンセルするか
        /// </summary>
        public bool IsCloseCancel
        {
            get => isCloseCancel;
            set
            {
                isCloseCancel = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力日を指定するか
        /// </summary>
        public bool IsChangeOutputDate
        {
            get => isChangeOutputDate;
            set
            {
                isChangeOutputDate = value;
                CallPropertyChanged();
                if (!value) { ChangeOutputDate=DateTime.Now; }
                ReferenceTransferReceiptsAndExpenditure(true);
            }
        }
        /// <summary>
        /// 伝票出力日
        /// </summary>
        public DateTime ChangeOutputDate
        {
            get => changeOutputDate;
            set
            {
                changeOutputDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された振替データ
        /// </summary>
        public ListItem SelectedListItem
        {
            get => selectedListItem;
            set
            {
                selectedListItem = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 印刷するデータを選択するか
        /// </summary>
        public bool IsOutputDataSelect
        {
            get => isOutputDataSelect;
            set
            {
                isOutputDataSelect = value;
                CallPropertyChanged();
                foreach (ListItem li in AllDataList)
                {
                    li.IsOutput = !value;
                }
                
                ObservableCollection<ListItem> items = new ObservableCollection<ListItem>(); 

                foreach (ListItem li in TransferReceiptsAndExpenditures)
                {
                    li.IsOutput = !value;
                    items.Add(li);
                }

                TransferReceiptsAndExpenditures = items;
                SetTotalAmount();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {  }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"振替データ管理 : {AccountingProcessLocation.Location}"; }

        public void SortNotify() { ReferenceTransferReceiptsAndExpenditure(true); }

        public void PageNotify() { ReferenceTransferReceiptsAndExpenditure(false); }

        public void SetSortColumns()
        { 
            Pagination.SortColumns =
                new Dictionary<int, string>() { { 0, "ID" }, { 1, "振替日" }, { 2, "伝票出力日" } }; 
        }

        public int SetCountEachPage() => 10;

        public void TransferReceiptsAndExpenditureOperationNotify() 
        { ReferenceTransferReceiptsAndExpenditure(true); }

        public bool CancelClose() => IsCloseCancel;

        public class ListItem : NotifyPropertyChanged
        {
            private bool isOutput;

            public bool IsOutput
            { get => isOutput; set { isOutput = value; } }

            public TransferReceiptsAndExpenditure Data { get; set; }

            public ListItem(bool isOutput, TransferReceiptsAndExpenditure data)
            {
                IsOutput = isOutput;
                Data = data;
            }
        }
    }
}