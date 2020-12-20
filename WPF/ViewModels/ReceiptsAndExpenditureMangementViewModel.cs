using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納管理ウィンドウViewModel
    /// </summary>
    public class ReceiptsAndExpenditureMangementViewModel : DataOperationViewModel
    {
        #region Properties
        #region int
        private int peymentSum;
        private int withdrawalSum;
        private int transferSum;
        private int previousDayFinalAccount;
        private int todayTotalAmount;
        #endregion
        #region string
        private string peymentSumDisplayValue;
        private string withdrawalSumDisplayValue;
        private string transferSumDisplayValue;
        private string previousDayFinalAccountDisplayValue;
        private string comboAccountingSubjectText;
        private string comboAccountingSubjectCode;
        private string detailText;
        private string cashBoxTotalAmount;
        private string depositAndWithdrawalContetnt;
        private string comboContentText;
        private string price;
        private string receiptsAndExpenditureIDField;
        private string comboCreditAccountText;
        private string dataOperationButtonContent;
        private string balanceFinalAccount;
        private string listTitle;
        private string todaysFinalAccount;
        private string balanceFinalAccountOutputButtonContent;
        private string yokohamaBankAmount;
        private string ceresaAmount;
        private string wizeCoreAmount;
        private string receiptsAndExpenditureOutputButtonContent;
        private string paymentSlipsOutputButtonContent;
        #endregion
        #region bool
        private bool isValidity;
        private bool isPaymentCheck;
        private bool isDepositAndWithdrawalContetntEnabled;
        private bool isComboBoxEnabled;
        private bool isDetailTextEnabled;
        private bool isPriceEnabled;
        private bool isAccountActivityEnabled;
        private bool isPeriodSearch;
        private bool isSeachInfoVisibility;
        private bool isAllShowItem;
        private bool isPaymentOnly;
        private bool isWithdrawalOnly;
        private bool isReferenceMenuEnabled;
        private bool isDataOperationButtonEnabled;
        private bool isOutputGroupEnabled;
        private bool isBalanceFinalAccountOutputEnabled;
        private bool isReceiptsAndExpenditureOutputButtonEnabled;
        private bool isPaymentSlipsOutputEnabled;
        #endregion
        #region DateTime
        private DateTime accountActivityDate;
        private DateTime searchStartDate;
        private DateTime searchEndDate;
        private DateTime registrationDate;
        #endregion
        #region ObservableCollection
        private ObservableCollection<AccountingSubject> comboAccountingSubjects;
        private ObservableCollection<AccountingSubject> comboAccountingSubjectCodes;
        private ObservableCollection<Content> comboContents;
        private ObservableCollection<CreditAccount> comboCreditAccounts;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        #endregion
        private SolidColorBrush detailBackGroundColor;
        private Cashbox Cashbox = Cashbox.GetInstance();
        private Rep registrationRep;
        private Content selectedContent;
        private AccountingSubject selectedAccountingSubject;
        private AccountingSubject selectedAccountingSubjectCode;
        private CreditAccount selectedCreditAccount = new CreditAccount(string.Empty, string.Empty, false);
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        #endregion

        public ReceiptsAndExpenditureMangementViewModel(IDataOutput dataOutput)
        {
            DataOutput = dataOutput;
            SetProperty();
            BalanceFinalAccountOutputCommand = new DelegateCommand(() => BalanceFinalAccountOutput(), () => IsBalanceFinalAccountOutputEnabled);
            ReceiptsAndExpenditureOutputCommand = new DelegateCommand(() => ReceiptsAndExpenditureOutput(), () => IsReceiptsAndExpenditureOutputButtonEnabled);
            ShowRemainingCalculationViewCommand = new DelegateCommand(() => ShowRemainingCalculationView(), () => true);
            SetCashboxTotalAmountCommand = new DelegateCommand(() => SetCashboxTotalAmount(), () => true);
            PaymentSlipsOutputCommand = new DelegateCommand(() => PaymentSlipsOutput(), () => IsPaymentSlipsOutputEnabled);
        }
        public ReceiptsAndExpenditureMangementViewModel() : this(DefaultInfrastructure.GetDefaultDataOutput()) { }
        /// <summary>
        /// 入金伝票出力コマンド
        /// </summary>
        public DelegateCommand PaymentSlipsOutputCommand { get; }
        private async void PaymentSlipsOutput()
        {
            PaymentSlipsOutputButtonContent = "出力中";
            IsPaymentSlipsOutputEnabled = false;
            await Task.Run(() => DataOutput.PaymentSlips(ReceiptsAndExpenditures,LoginRep.Rep));
            IsPaymentSlipsOutputEnabled = true;
            PaymentSlipsOutputButtonContent = "入金伝票";
        }
        /// <summary>
        /// 金庫金額計算ウィンドウ表示コマンド
        /// </summary>
        public DelegateCommand ShowRemainingCalculationViewCommand { get; }
        private void ShowRemainingCalculationView()
        {
            CreateShowWindowCommand(ScreenTransition.RemainingMoneyCalculation());
            CallPropertyChanged();
        }
        /// <summary>
        /// Viewにプロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            AccountActivityDate = DateTime.Today;
            SearchStartDate = DateTime.Today;
            RegistrationDate = DateTime.Today;
            IsPaymentCheck = true;
            TodaysFinalAccount = TextHelper.AmountWithUnit(ReturnTodaysFinalAccount());
            IsOutputGroupEnabled = Cashbox.GetTotalAmount() == TextHelper.IntAmount(TodaysFinalAccount);
            IsReceiptsAndExpenditureOutputButtonEnabled = true;
            IsBalanceFinalAccountOutputEnabled = true;
            IsPaymentSlipsOutputEnabled = true;
            BalanceFinalAccountOutputButtonContent = "収支日報";
            ReceiptsAndExpenditureOutputButtonContent = "出納帳";
            PaymentSlipsOutputButtonContent = "入金伝票";
            DateTime PreviousDay;
            if (IsPeriodSearch) PreviousDay = SearchEndDate;
            else PreviousDay = SearchStartDate;
            PreviousDayFinalAccount = DataBaseConnect.FinalAccountPerMonth() - DataBaseConnect.PreviousDayDisbursement(PreviousDay) + DataBaseConnect.PreviousDayIncome(PreviousDay);
            ListTitle = $"一覧 : 前日決算 {TextHelper.AmountWithUnit( PreviousDayFinalAccount)}";
            RegistrationRep = LoginRep.Rep;
            ReceiptsAndExpenditures = DataBaseConnect.ReferenceReceiptsAndExpenditure(string.Empty, string.Empty, string.Empty, string.Empty, 
                string.Empty, string.Empty, false, true, false, string.Empty, string.Empty);
            SetPeymentSum();
            SetWithdrawalSumAndTransferSum();
            SetCashboxTotalAmount();
        }
        /// <summary>
        /// 金庫データをViewにセットする
        /// </summary>
        public DelegateCommand SetCashboxTotalAmountCommand { get; }
        private void SetCashboxTotalAmount()
        {
            Cashbox = Cashbox.GetInstance();
            todayTotalAmount = Cashbox.GetTotalAmount();
            CashBoxTotalAmount = todayTotalAmount == 0 ? "金庫の金額を計上して下さい" : $"金庫の金額 : {TextHelper.AmountWithUnit(todayTotalAmount)}";
            if (todayTotalAmount == PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum) TodaysFinalAccount = TextHelper.AmountWithUnit(todayTotalAmount);
            if (todayTotalAmount == 0) TodaysFinalAccount = "データを照合して下さい。";
            SetBalanceFinalAccount();
        }
        /// <summary>
        /// 出納データ出力コマンド
        /// </summary>
        public DelegateCommand ReceiptsAndExpenditureOutputCommand { get; }
        private async void ReceiptsAndExpenditureOutput()
        {
            ReceiptsAndExpenditureOutputButtonContent = "出力中";
            IsOutputGroupEnabled = false;
            await Task.Run(() => DataOutput.ReceiptsAndExpenditureData(ReceiptsAndExpenditures, PreviousDayFinalAccount));
            ReceiptsAndExpenditureOutputButtonContent = "出納データ";
            IsOutputGroupEnabled = true;
        }
        /// <summary>
        /// 収支日報出力コマンド
        /// </summary>
        public DelegateCommand BalanceFinalAccountOutputCommand { get; }
        /// <summary>
        /// 収支日報を出力します
        /// </summary>
        private async void BalanceFinalAccountOutput()
        {
            BalanceFinalAccountOutputButtonContent = "出力中";
            IsOutputGroupEnabled = false;
            await Task.Run(() => DataOutput.BalanceFinalAccount(TextHelper.AmountWithUnit(PreviousDayFinalAccount), PeymentSumDisplayValue, WithdrawalSumDisplayValue, TransferSumDisplayValue, TodaysFinalAccount,
                YokohamaBankAmount, CeresaAmount, WizeCoreAmount));
            BalanceFinalAccountOutputButtonContent = "収支日報";
            IsOutputGroupEnabled = true;
        }

        protected override void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    IsValidity = true;
                    IsDepositAndWithdrawalContetntEnabled = true;
                    IsComboBoxEnabled = true;
                    IsDetailTextEnabled = true;
                    IsAccountActivityEnabled = true;
                    IsPriceEnabled = true;
                    ComboContentText = string.Empty;
                    ComboAccountingSubjectText = string.Empty;
                    ComboAccountingSubjectCode = string.Empty;
                    ComboCreditAccountText = ComboCreditAccounts[0].Account;
                    DetailText = string.Empty;
                    price = string.Empty;
                    IsReferenceMenuEnabled = false;
                    break;
                case DataOperation.更新:
                    ComboCreditAccountText = string.Empty;
                    SetDataList();
                    IsReferenceMenuEnabled = true;
                    break;
            }
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) => DataOperationButtonContent = operation.ToString();

        protected override void SetDataList()
        {
            ComboContents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty, string.Empty, true);
            ComboAccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboAccountingSubjectCodes = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboCreditAccounts = DataBaseConnect.ReferenceCreditAccount(string.Empty, true);
        }

        protected override void SetDelegateCommand()
        {
            ReceiptsAndExpenditureDataOperationCommand =
                new DelegateCommand(() => ReceiptsAndExpenditureDataOperation(), () => IsDataOperationButtonEnabled);
        }
        /// <summary>
        /// 本日の決算額を返します
        /// </summary>
        /// <returns></returns>
        private int ReturnTodaysFinalAccount()
        {
            if (DataBaseConnect.PreviousDayTotalBalance(DateTime.Today.AddDays(-2))- DataBaseConnect.PreviousDayDisbursement(DateTime.Today.AddDays(-1))+DataBaseConnect.PreviousDayIncome(DateTime.Today.AddDays(-1)) == PreviousDayFinalAccount)
                return PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum;
            else
                return 0;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand ReceiptsAndExpenditureDataOperationCommand { get; set; }
        /// <summary>
        /// 出納データを登録、更新します
        /// </summary>
        private void ReceiptsAndExpenditureDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    DataRegistration();
                    break;
            }
        }
        /// <summary>
        /// 出納データ登録
        /// </summary>
        private void DataRegistration()
        {
            DataBaseConnect.Registration
                (new ReceiptsAndExpenditure(0, DateTime.Now, LoginRep.Rep, AccountingProcessLocation.Location, SelectedCreditAccount, SelectedContent, DetailText, TextHelper.IntAmount(price),
                 IsPaymentCheck, IsValidity, TextHelper.DefaultDate, null, AccountActivityDate));
            SetPeymentSum();
            SetWithdrawalSumAndTransferSum();
        }
        /// <summary>
        /// 出納データから出金データを取り出し、出金、振替に振り分けて合計を算出します
        /// </summary>
        private void SetWithdrawalSumAndTransferSum()
        {
            WithdrawalSum = 0;
            TransferSum = 0;
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures) { if (!rae.IsPayment) WithdrawalAllocation(rae); }
        }
        /// <summary>
        /// 出金データを出金、振替に振り分けます
        /// </summary>
        /// <param name="receiptsAndExpenditure"></param>
        private void WithdrawalAllocation(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            if (receiptsAndExpenditure.Content.Text == "口座入金")
                TransferSum += receiptsAndExpenditure.Price;
            else
                WithdrawalSum += receiptsAndExpenditure.Price;
        }
        /// <summary>
        /// 入金合計を算出します
        /// </summary>
        private void SetPeymentSum()
        {
            int i = 0;

            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures) { if (rae.IsPayment) i += rae.Price; }
            PaymentSum =i;
        }
        /// <summary>
        /// 金庫の総計金額
        /// </summary>
        public string CashBoxTotalAmount
        {
            get => cashBoxTotalAmount;
            set
            {
                cashBoxTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金チェック　入金がTrue
        /// </summary>
        public bool IsPaymentCheck
        {
            get => isPaymentCheck;
            set
            {
                isPaymentCheck = value;
                if (value)
                {
                    DepositAndWithdrawalContetnt = "入金";
                    DetailBackGroundColor = new SolidColorBrush(Colors.MistyRose);
                }
                else
                {
                    DepositAndWithdrawalContetnt = "出金";
                    DetailBackGroundColor = new SolidColorBrush(Colors.LightCyan);
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金のトグルコンテント
        /// </summary>
        public string DepositAndWithdrawalContetnt
        {
            get => depositAndWithdrawalContetnt;
            set
            {
                depositAndWithdrawalContetnt = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金に応じて、詳細の色を決める
        /// </summary>
        public SolidColorBrush DetailBackGroundColor
        {
            get => detailBackGroundColor;
            set
            {
                detailBackGroundColor = value;
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
                if (selectedContent != null && selectedContent.Equals(value)) return;
                selectedContent = value;
                if (selectedContent != null && selectedContent.FlatRate > 0) Price = selectedContent.FlatRate.ToString();
                else Price = string.Empty;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスリスト
        /// </summary>
        public ObservableCollection<Content> ComboContents
        {
            get => comboContents;
            set
            {
                comboContents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスのText
        /// </summary>
        public string ComboContentText
        {
            get => comboContentText;
            set
            {
                if (comboContentText == value) return;
                comboContentText = value;
                CallPropertyChanged();
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboContentText), comboContentText);
            }
        }
        /// <summary>
        /// コンボボックスで選択された勘定科目
        /// </summary>
        public AccountingSubject SelectedAccountingSubject
        {
            get => selectedAccountingSubject;
            set
            {
                if (selectedAccountingSubject != null && selectedAccountingSubject.Equals(value)) return;
                selectedAccountingSubject = value;
                if (selectedAccountingSubject != null && CurrentOperation == DataOperation.登録) ComboContents = DataBaseConnect.ReferenceContent(string.Empty, selectedAccountingSubject.SubjectCode, selectedAccountingSubject.Subject, true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コンボボックスの勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> ComboAccountingSubjects
        {
            get => comboAccountingSubjects;
            set
            {
                comboAccountingSubjects = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectText
        {
            get => comboAccountingSubjectText;
            set
            {
                if (comboAccountingSubjectText == value) return;
                comboAccountingSubjectText = value;
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboAccountingSubjectText), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードコンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectCode
        {
            get => comboAccountingSubjectCode;
            set
            {
                if (comboAccountingSubjectCode == value) return;
                comboAccountingSubjectCode = value;
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboAccountingSubjectCode), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 詳細欄のText
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
        /// 出納金額
        /// </summary>
        public string Price
        {
            get => price;
            set
            {
                price = TextHelper.CommaDelimitedAmount(value);
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(Price), price);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納IDフィールド
        /// </summary>
        public string ReceiptsAndExpenditureIDField
        {
            get => receiptsAndExpenditureIDField;
            set
            {
                receiptsAndExpenditureIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定コンボボックスのリスト
        /// </summary>
        public ObservableCollection<CreditAccount> ComboCreditAccounts
        {
            get => comboCreditAccounts;
            set
            {
                comboCreditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方勘定
        /// </summary>
        public CreditAccount SelectedCreditAccount
        {
            get => selectedCreditAccount;
            set
            {
                if (selectedCreditAccount != null && selectedCreditAccount.Equals(value)) return;
                selectedCreditAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定コンボボックスのText
        /// </summary>
        public string ComboCreditAccountText
        {
            get => comboCreditAccountText;
            set
            {
                if (comboCreditAccountText == value) return;
                SelectedCreditAccount = ComboCreditAccounts.FirstOrDefault(c => c.Account == value);
                if (SelectedCreditAccount == null) comboCreditAccountText = string.Empty;
                else comboCreditAccountText = SelectedCreditAccount.Account;

                SetDataOperationButtonEnabled();
                ValidationProperty(ComboCreditAccountText, value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データの有効性
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
        /// 入出金チェックのEnabled
        /// </summary>
        public bool IsDepositAndWithdrawalContetntEnabled
        {
            get => isDepositAndWithdrawalContetntEnabled;
            set
            {
                isDepositAndWithdrawalContetntEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 詳細のコンボボックスのEnabled
        /// </summary>
        public bool IsComboBoxEnabled
        {
            get => isComboBoxEnabled;
            set
            {
                isComboBoxEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納詳細のEnabled
        /// </summary>
        public bool IsDetailTextEnabled
        {
            get => isDetailTextEnabled;
            set
            {
                isDetailTextEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額Enabled
        /// </summary>
        public bool IsPriceEnabled
        {
            get => isPriceEnabled;
            set
            {
                isPriceEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金日Enabled
        /// </summary>
        public bool IsAccountActivityEnabled
        {
            get => isAccountActivityEnabled;
            set
            {
                isAccountActivityEnabled = value;
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
        /// データ操作ボタンのEnable
        /// </summary>
        public bool IsDataOperationButtonEnabled
        {
            get => isDataOperationButtonEnabled;
            set
            {
                isDataOperationButtonEnabled = value;
                CallPropertyChanged();
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
                IsSearchInfoVisibility = value;
                if (!value) SearchEndDate = SearchStartDate;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索の最古の年月日
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                if (SearchEndDate < value) SearchEndDate = value;
                searchStartDate = value;                
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索の最新の年月日
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                if (SearchStartDate > value) searchEndDate = SearchStartDate;
                else searchEndDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索する場合のVisiblity
        /// </summary>
        public bool IsSearchInfoVisibility
        {
            get => isSeachInfoVisibility;
            set
            {
                isSeachInfoVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 該当する日付のすべての出納データを表示するかのCheck
        /// </summary>
        public bool IsAllShowItem
        {
            get => isAllShowItem;
            set
            {
                isAllShowItem = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 該当する日付の入金の出納データのみ表示するかのCheck
        /// </summary>
        public bool IsPaymentOnly
        {
            get => isPaymentOnly;
            set
            {
                isPaymentOnly = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 該当する日付の出金の出納データのみ表示するかのCheck
        /// </summary>
        public bool IsWithdrawalOnly
        {
            get => isWithdrawalOnly;
            set
            {
                isWithdrawalOnly = value;
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
                SetBalanceFinalAccount();
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
                SetReceiptsAndExpenditureProperty();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データ選択時に詳細フィールドにプロパティをセットします
        /// </summary>
        private void SetReceiptsAndExpenditureProperty()
        {
            IsValidity = SelectedReceiptsAndExpenditure.IsValidity;
            IsPaymentCheck = SelectedReceiptsAndExpenditure.IsPayment;
            ComboCreditAccountText = SelectedReceiptsAndExpenditure.CreditAccount.Account;
            ComboAccountingSubjectCode = SelectedReceiptsAndExpenditure.Content.AccountingSubject.SubjectCode;
            ComboAccountingSubjectText = SelectedReceiptsAndExpenditure.Content.AccountingSubject.Subject;
            ComboContentText = SelectedReceiptsAndExpenditure.Content.Text;
            DetailText = SelectedReceiptsAndExpenditure.Detail;
            Price = SelectedReceiptsAndExpenditure.Price.ToString();
            AccountActivityDate = SelectedReceiptsAndExpenditure.AccountActivityDate;
            RegistrationDate = SelectedReceiptsAndExpenditure.RegistrationDate;
            RegistrationRep = SelectedReceiptsAndExpenditure.RegistrationRep;
        }
        /// <summary>
        /// 出納データ登録日
        /// </summary>
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set
            {
                registrationDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録担当者
        /// </summary>
        public Rep RegistrationRep
        {
            get => registrationRep;
            set
            {
                registrationRep = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索メニューのEnabled
        /// </summary>
        public bool IsReferenceMenuEnabled
        {
            get => isReferenceMenuEnabled;
            set
            {
                isReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データリストの収支決算
        /// </summary>
        public string BalanceFinalAccount
        {
            get => balanceFinalAccount;
            set
            {
                balanceFinalAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードのリスト
        /// </summary>
        public ObservableCollection<AccountingSubject> ComboAccountingSubjectCodes
        {
            get => comboAccountingSubjectCodes;
            set
            {
                comboAccountingSubjectCodes = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目コード
        /// </summary>
        public AccountingSubject SelectedAccountingSubjectCode
        {
            get => selectedAccountingSubjectCode;
            set
            {
                selectedAccountingSubjectCode = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前日決算
        /// </summary>
        public int PreviousDayFinalAccount
        {
            get => previousDayFinalAccount;
            set
            {
                previousDayFinalAccount = value;
                PreviousDayFinalAccountDisplayValue = TextHelper.AmountWithUnit(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納一覧のタイトル
        /// </summary>
        public string ListTitle
        {
            get => listTitle;
            set
            {
                listTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金合計
        /// </summary>
        public int PaymentSum
        {
            get => peymentSum;
            set
            {
                peymentSum = value;
                PeymentSumDisplayValue = TextHelper.AmountWithUnit(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出金合計
        /// </summary>
        public int  WithdrawalSum
        {
            get => withdrawalSum;
            set
            {
                withdrawalSum = value;
                WithdrawalSumDisplayValue = TextHelper.AmountWithUnit(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 振替合計
        /// </summary>
        public int TransferSum
        {
            get => transferSum;
            set
            {
                transferSum = value;
                TransferSumDisplayValue = TextHelper.AmountWithUnit(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 今日の決算
        /// </summary>
        public string TodaysFinalAccount
        {
            get => todaysFinalAccount;
            set
            {
                todaysFinalAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 収支日報出力ボタンのContent
        /// </summary>
        public string BalanceFinalAccountOutputButtonContent
        {
            get => balanceFinalAccountOutputButtonContent;
            set
            {
                balanceFinalAccountOutputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 横浜銀行残高
        /// </summary>
        public string YokohamaBankAmount
        {
            get => yokohamaBankAmount;
            set
            {
                yokohamaBankAmount = TextHelper.CommaDelimitedAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// セレサ川崎残高
        /// </summary>
        public string CeresaAmount
        {
            get => ceresaAmount;
            set
            {
                ceresaAmount = TextHelper.CommaDelimitedAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ワイズコア仮受金
        /// </summary>
        public string WizeCoreAmount
        {
            get => wizeCoreAmount;
            set
            {
                wizeCoreAmount = TextHelper.CommaDelimitedAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 各種出力メニューのEnabled
        /// </summary>
        public bool IsOutputGroupEnabled
        {
            get => isOutputGroupEnabled;
            set
            {
                isOutputGroupEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 収支日報出力ボタンのEnabled
        /// </summary>
        public bool IsBalanceFinalAccountOutputEnabled
        {
            get => isBalanceFinalAccountOutputEnabled;
            set
            {
                isBalanceFinalAccountOutputEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用入金額
        /// </summary>
        public string PeymentSumDisplayValue
        {
            get => peymentSumDisplayValue;
            set
            {
                peymentSumDisplayValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用出金額
        /// </summary>
        public string WithdrawalSumDisplayValue
        {
            get => withdrawalSumDisplayValue;
            set
            {
                withdrawalSumDisplayValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用振替額
        /// </summary>
        public string TransferSumDisplayValue
        {
            get => transferSumDisplayValue;
            set
            {
                transferSumDisplayValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データ出力ボタンのContent
        /// </summary>
        public string ReceiptsAndExpenditureOutputButtonContent
        {
            get => receiptsAndExpenditureOutputButtonContent;
            set
            {
                receiptsAndExpenditureOutputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データ出力ボタンのEnabled
        /// </summary>
        public bool IsReceiptsAndExpenditureOutputButtonEnabled
        {
            get => isReceiptsAndExpenditureOutputButtonEnabled;
            set
            {
                isReceiptsAndExpenditureOutputButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用前日決算
        /// </summary>
        public string PreviousDayFinalAccountDisplayValue
        {
            get => previousDayFinalAccountDisplayValue;
            set
            {
                previousDayFinalAccountDisplayValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金伝票出力ボタンのContent
        /// </summary>
        public string PaymentSlipsOutputButtonContent
        {
            get => paymentSlipsOutputButtonContent;
            set
            {
                paymentSlipsOutputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金伝票出力ボタンのEnabled
        /// </summary>
        public bool IsPaymentSlipsOutputEnabled
        {
            get => isPaymentSlipsOutputEnabled;
            set
            {
                isPaymentSlipsOutputEnabled = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// リストの収支決算を表示します
        /// </summary>
        private void SetBalanceFinalAccount()
        {
            int amount = DataBaseConnect.FinalAccountPerMonth();

            foreach(ReceiptsAndExpenditure receiptsAndExpenditure in ReceiptsAndExpenditures)
            {
                if (receiptsAndExpenditure.IsPayment) amount += receiptsAndExpenditure.Price;
                else amount -= receiptsAndExpenditure.Price;
            }
            BalanceFinalAccount = $"出納リストの収支決算 : {TextHelper.AmountWithUnit(amount)}";
            SetOutputButtonEnabled(amount);
        }
        private void SetOutputButtonEnabled(int amount)
        {        
            IsBalanceFinalAccountOutputEnabled = Cashbox.GetTotalAmount() == amount;
            IsReceiptsAndExpenditureOutputButtonEnabled = Cashbox.GetTotalAmount() == amount;
            IsPaymentSlipsOutputEnabled = Cashbox.GetTotalAmount() == amount;
        }
        /// <summary>
        /// データ操作ボタンのEnabledを設定します
        /// </summary>
        public void SetDataOperationButtonEnabled()
        {
            switch(CurrentOperation)
            {
                case DataOperation.登録:
                    IsDataOperationButtonEnabled = CanRegistration();
                    break;
            }
        }
        /// <summary>
        /// 出納データ登録時の必須のフィールドにデータが入力されているかを確認し、判定結果を返します
        /// </summary>
        /// <returns>判定結果</returns>
        private bool CanRegistration() =>
            !string.IsNullOrEmpty(ComboCreditAccountText) & !string.IsNullOrEmpty(ComboContentText) & !string.IsNullOrEmpty(ComboAccountingSubjectText) &
            !string.IsNullOrEmpty(ComboAccountingSubjectCode) & !string.IsNullOrEmpty(Price) && 0 < TextHelper.IntAmount(price);

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(ComboContentText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboAccountingSubjectText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboAccountingSubjectCode):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(Price):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboCreditAccountText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
            }
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"出納管理 : {AccountingProcessLocation.Location}";
            return DefaultWindowTitle;
        }
    }
}