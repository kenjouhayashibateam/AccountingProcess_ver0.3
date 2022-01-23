using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    public class TransferSlipOperationViewModel : DataOperationViewModel, IPagenationObserver,
        ITransferReceiptsAndExpenditureOperationObserver
    {
        #region Properties
        #region Strings
        private string searchSubjectCode;
        private string selectedSubjectCode;
        private string selectedSubject;
        private string selectedContentText;
        private string selectedDetail;
        private string selectedPrice;
        private string debitAccountCode;
        private string creditAccountCode;
        private string detailText;
        private string price;
        private string dataOperationButtonContent;
        #endregion
        #region Bools
        private bool isLimitedCreditDept;
        private bool isDoNotUseOriginalSlip;
        private bool isPayment;
        private bool isValidity;
        private bool isReducedTaxRate;
        private bool isValidityEnabled;
        #endregion
        #region Dates
        private DateTime searchStartDate = DateTime.Now;
        private DateTime searchEndDate = DateTime.Now;
        private DateTime accountActivityDate;
        #endregion
        #region ObservableCollections
        private ObservableCollection<CreditDept> searchCreditDepts;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private ObservableCollection<TransferReceiptsAndExpenditure> transferReceiptsAndExpenditures;
        private ObservableCollection<AccountingSubject> debitAccounts;
        private ObservableCollection<AccountingSubject> creditAccounts;
        private ObservableCollection<Content> contents;
        #endregion
        #region ValueObjects
        private CreditDept searchCreditDept;
        private CreditDept selectedCreditDept;
        private AccountingSubject debitAccount;
        private AccountingSubject creditAccount;
        private Content selectedContent;
        #endregion
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private Pagination pagination;
        #endregion

        public TransferSlipOperationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            Pagination.SetProperty();

            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null)
            {
                FieldClear();
                SetDataOperation(DataOperation.登録);
                DataOperationButtonContent = DataOperation.登録.ToString();
            }
            else
            {
                SetProperty();
                SetDataOperation(DataOperation.更新);
                DataOperationButtonContent = DataOperation.更新.ToString();
            }
            SetDelegateCommand();
        }
        public TransferSlipOperationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 振替伝票データプロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null) { return; }

            TransferReceiptsAndExpenditure trae = TransferReceiptsAndExpenditureOperation.GetInstance().GetData();
            SearchStartDate = trae.AccountActivityDate;
            SearchEndDate = trae.AccountActivityDate;
            AccountActivityDate = trae.AccountActivityDate;
            SelectedCreditDept = trae.CreditDept;
            DebitAccountCode = trae.DebitAccount.SubjectCode;
            DebitAccount = trae.DebitAccount;
            CreditAccountCode = trae.CreditAccount.SubjectCode;
            CreditAccount = trae.CreditAccount;
            SelectedContent = DataBaseConnect.ReferenceContent
                (trae.ContentText, string.Empty, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen,
                    false)[0];
            price = trae.Price.ToString();
            IsReducedTaxRate = trae.IsReducedTaxRate;

            ReceiptsAndExpenditure rae = DataBaseConnect.CallTransferReceiptsAndExpenditureParentData(trae);
            if (rae == null) { return; }

            IsPayment = rae.IsPayment;
        }
        /// <summary>
        /// フィールドをクリアします
        /// </summary>
        private void FieldClear()
        {
            IsDoNotUseOriginalSlip = false;
            IsPayment = true;
            AccountActivityDate = DateTime.Now;
            SearchStartDate = DateTime.Now;
            SearchEndDate = DateTime.Now;
            SearchSubjectCode = string.Empty;
            IsLimitedCreditDept = false;
            ReceiptsAndExpenditureFieldClear();
        }
        /// <summary>
        /// 選択された元伝票フィールドをクリアします
        /// </summary>
        private void ReceiptsAndExpenditureFieldClear()
        {
            SelectedSubjectCode = string.Empty;
            SelectedSubject = string.Empty;
            SelectedContentText = string.Empty;
            SelectedDetail = string.Empty;
            SelectedPrice = string.Empty;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand DataOperationCommand { get; set; }
        /// <summary>
        /// 出納データ詳細表示コマンド
        /// </summary>
        public DelegateCommand ShowDetailCommand { get; set; }
        private async void ShowDetail()
        {
            if (SelectedReceiptsAndExpenditure == null) { return; }

            await Task.Delay(1);

            ReceiptsAndExpenditureOperation.GetInstance().SetData(SelectedReceiptsAndExpenditure);
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureOperation());
        }
        /// <summary>
        /// 選択された出納データ反映コマンド
        /// </summary>
        public DelegateCommand SetSelectedReceiptsAndExpenditureCommand { get; set; }
        private async void SetSelectedReceiptsAndExpenditure()
        {
            await Task.Delay(1);

            if (SelectedReceiptsAndExpenditure == null) { return; }

            IsPayment = SelectedReceiptsAndExpenditure.IsPayment; ;
            SelectedSubjectCode = SelectedReceiptsAndExpenditure.Content.AccountingSubject.SubjectCode;
            SelectedSubject = SelectedReceiptsAndExpenditure.Content.AccountingSubject.Subject;
            SelectedContentText = SelectedReceiptsAndExpenditure.Content.Text;
            SelectedDetail = SelectedReceiptsAndExpenditure.Detail;
            SelectedPrice = SelectedReceiptsAndExpenditure.PriceWithUnit;

            int i = SelectedReceiptsAndExpenditure.Price;
            TransferReceiptsAndExpenditures =
                DataBaseConnect.ReferenceTransferReceiptsAndExpenditure(SelectedReceiptsAndExpenditure);

            foreach (TransferReceiptsAndExpenditure trae in TransferReceiptsAndExpenditures) { i -= trae.Price; }

            Price = i.ToString();
        }
        /// <summary>
        /// 出納データ検索コマンド
        /// </summary>
        public DelegateCommand SearchReceiptsAndExpenditureCommand { get; set; }
        private void SearchReceiptsAndExpenditure(bool isPageReset)
        {
            Pagination.CountReset(isPageReset);
            string dept = SearchCreditDept == null ? string.Empty : SearchCreditDept.Dept;
            (Pagination.TotalRowCount, ReceiptsAndExpenditures) = DataBaseConnect.ReferenceReceiptsAndExpenditure
                (DefaultDate, DateTime.Now, AccountingProcessLocation.Location.ToString(), dept,
                    string.Empty, string.Empty, string.Empty, SearchSubjectCode,
                    AccountingProcessLocation.IsAccountingGenreShunjuen, false, true, true, true, SearchStartDate,
                    SearchEndDate, DefaultDate, DateTime.Now, Pagination.PageCount, Pagination.SelectedSortColumn,
                    Pagination.SortDirectionIsASC, Pagination.CountEachPage);

            Pagination.SetProperty();
        }
        /// <summary>
        /// 検索メニューの貸方部門
        /// </summary>
        public ObservableCollection<CreditDept> SearchCreditDepts
        {
            get => searchCreditDepts;
            set
            {
                searchCreditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票リスト
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
        /// 既に関連付けられている振替伝票リスト
        /// </summary>
        public ObservableCollection<TransferReceiptsAndExpenditure> TransferReceiptsAndExpenditures
        {
            get => transferReceiptsAndExpenditures;
            set
            {
                transferReceiptsAndExpenditures = value;
                CallPropertyChanged();
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
                searchStartDate = value > SearchEndDate ? SearchEndDate : value;
                CallPropertyChanged();
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
                searchEndDate = value < SearchStartDate ? SearchStartDate : value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する科目コード
        /// </summary>
        public string SearchSubjectCode
        {
            get => searchSubjectCode;
            set
            {
                searchSubjectCode = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方部門
        /// </summary>
        public CreditDept SearchCreditDept
        {
            get => searchCreditDept;
            set
            {
                searchCreditDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方部門を限定チェック
        /// </summary>
        public bool IsLimitedCreditDept
        {
            get => isLimitedCreditDept;
            set
            {
                isLimitedCreditDept = value;
                CallPropertyChanged();
                if (value) { SearchCreditDept = null; }
            }
        }
        /// <summary>
        /// 元の伝票を使用しないチェック
        /// </summary>
        public bool IsDoNotUseOriginalSlip
        {
            get => isDoNotUseOriginalSlip;
            set
            {
                isDoNotUseOriginalSlip = value;
                CallPropertyChanged();
                if (value) { SelectedReceiptsAndExpenditure = null; }
            }
        }
        /// <summary>
        /// 入出金チェック
        /// </summary>
        public bool IsPayment
        {
            get => isPayment;
            set
            {
                isPayment = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された科目コード
        /// </summary>
        public string SelectedSubjectCode
        {
            get => selectedSubjectCode;
            set
            {
                selectedSubjectCode = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目
        /// </summary>
        public string SelectedSubject
        {
            get => selectedSubject;
            set
            {
                selectedSubject = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票内容
        /// </summary>
        public string SelectedContentText
        {
            get => selectedContentText;
            set
            {
                selectedContentText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票詳細
        /// </summary>
        public string SelectedDetail
        {
            get => selectedDetail;
            set
            {
                selectedDetail = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票金額
        /// </summary>
        public string SelectedPrice
        {
            get => selectedPrice;
            set
            {
                selectedPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity
        {
            get => isValidity;
            set
            {
                isValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
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
            }
        }
        /// <summary>
        /// 選択された借方、貸方勘定科目にグルーピングされた伝票内容をリストにします
        /// </summary>
        private void SetContents()
        {
            ObservableCollection<Content> debitList;
            ObservableCollection<Content> creditList;
            ObservableCollection<Content> List = new ObservableCollection<Content>();

            if (DebitAccount != null)
            {
                debitList = DataBaseConnect.ReferenceContent
                    (string.Empty, DebitAccount.SubjectCode, DebitAccount.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                foreach (Content content in debitList) { List.Add(content); }
            }

            if (CreditAccount != null)
            {
                creditList = DataBaseConnect.ReferenceContent
                    (string.Empty, CreditAccount.SubjectCode, CreditAccount.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                foreach (Content content in creditList) { List.Add(content); }
            }

            Contents = List;
        }
        /// <summary>
        /// 借方勘定科目
        /// </summary>
        public AccountingSubject DebitAccount
        {
            get => debitAccount;
            set
            {
                debitAccount = value;
                CallPropertyChanged();
                if (value != null) { SetContents(); }
            }
        }
        /// <summary>
        /// 借方勘定科目コード
        /// </summary>
        public string DebitAccountCode
        {
            get => debitAccountCode;
            set
            {
                debitAccountCode = value;
                CallPropertyChanged();
                if (value.Length == 3)
                {
                    DebitAccounts = DataBaseConnect.ReferenceAccountingSubject
                        (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                }
                else { return; }
                if (DebitAccounts.Count > 0) { DebitAccount = DebitAccounts[0]; }
            }
        }
        /// <summary>
        /// 借方勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> DebitAccounts
        {
            get => debitAccounts;
            set
            {
                debitAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定科目コード
        /// </summary>
        public string CreditAccountCode
        {
            get => creditAccountCode;
            set
            {
                creditAccountCode = value;
                CallPropertyChanged();
                if (value.Length == 3)
                {
                    CreditAccounts = DataBaseConnect.ReferenceAccountingSubject
                        (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                }
                else { return; }
                if (CreditAccounts.Count > 0) { CreditAccount = CreditAccounts[0]; }
            }
        }
        /// <summary>
        /// 貸方勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> CreditAccounts
        {
            get => creditAccounts;
            set
            {
                creditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定科目
        /// </summary>
        public AccountingSubject CreditAccount
        {
            get => creditAccount;
            set
            {
                creditAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容リスト
        /// </summary>
        public ObservableCollection<Content> Contents
        {
            get => contents;
            set
            {
                contents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票内容
        /// </summary>
        public Content SelectedContent
        {
            get => selectedContent;
            set
            {
                selectedContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 詳細
        /// </summary>
        public string DetailText
        {
            get => detailText;
            set
            {
                detailText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額
        /// </summary>
        public string Price
        {
            get => price;
            set
            {
                price = CommaDelimitedAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 軽減税率チェック
        /// </summary>
        public bool IsReducedTaxRate
        {
            get => isReducedTaxRate;
            set
            {
                isReducedTaxRate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作ボタンのContent
        /// </summary>
        public string DataOperationButtonContent
        {
            get => dataOperationButtonContent;
            set
            {
                dataOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された出納データ
        /// </summary>
        public ReceiptsAndExpenditure SelectedReceiptsAndExpenditure
        {
            get => selectedReceiptsAndExpenditure;
            set
            {
                selectedReceiptsAndExpenditure = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 有効性チェックのEnabled
        /// </summary>
        public bool IsValidityEnabled
        {
            get => isValidityEnabled;
            set
            {
                isValidityEnabled = value;
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

        public override void ValidationProperty(string propertyName, object value)
        {

        }

        protected override void SetDataList()
        {
            SearchCreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, true, AccountingProcessLocation.IsAccountingGenreShunjuen);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        { DataOperationButtonContent = operation.ToString(); }

        protected override void SetDelegateCommand()
        {
            SearchReceiptsAndExpenditureCommand = new DelegateCommand
                (() => SearchReceiptsAndExpenditure(true), () => true);
            ShowDetailCommand = new DelegateCommand(() => ShowDetail(), () => true);
            SetSelectedReceiptsAndExpenditureCommand = new DelegateCommand
                (() => SetSelectedReceiptsAndExpenditure(), () => true);
        }

        protected override void SetDetailLocked()
        {
            IsValidityEnabled = CurrentOperation == DataOperation.更新;
        }

        protected override void SetWindowDefaultTitle() { WindowTitle = "複合伝票管理"; }

        public void SortNotify() { SearchReceiptsAndExpenditure(true); }

        public void PageNotify() { SearchReceiptsAndExpenditure(false); }

        public void SetSortColumns() { Pagination.SortColumns = ReceptsAndExpenditureListSortColumns(); }

        public void SetCountEachPage() { Pagination.CountEachPage = 5; }

        public void TransferReceiptsAndExpenditureOperationNotify()
        {
            throw new NotImplementedException();
        }
    }
}
