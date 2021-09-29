using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納管理ウィンドウViewModel
    /// </summary>
    public class ReceiptsAndExpenditureMangementViewModel : BaseViewModel,
        IReceiptsAndExpenditureOperationObserver, IPagenationObserver, IClosing
    {
        #region Properties
        #region int
        private int paymentSum;
        private int withdrawalSum;
        private int transferSum;
        private int previousDayFinalAccount;
        private int todayTotalAmount;
        /// <summary>
        /// 金庫の締め時間
        /// </summary>
        private readonly int ClosingCashboxHour = 15;
        /// <summary>
        /// リストの収支金額
        /// </summary>
        private int ListAmount;
        #endregion
        #region string
        private string paymentSumDisplayValue;
        private string withdrawalSumDisplayValue;
        private string transferSumDisplayValue;
        private string previousDayFinalAccountDisplayValue;
        private string cashBoxTotalAmount;
        private string balanceFinalAccount;
        private string listTitle;
        private string todaysFinalAccount;
        private string balanceFinalAccountOutputButtonContent;
        private string yokohamaBankAmount;
        private string ceresaAmount;
        private string wizeCoreAmount;
        private string receiptsAndExpenditureOutputButtonContent;
        private string paymentSlipsOutputButtonContent;
        private string withdrawalSlipsOutputButtonContent;
        private string referenceLocationCheckBoxContent;
        private string password;
        private string outputMenuHeader;
        /// <summary>
        /// 当日決算の基準になる金額の種類。管理事務所なら前日決算、青蓮堂なら預り金額
        /// </summary>
        private string FinalAccountCategory;
        #endregion
        #region bool
        private bool isPeriodSearch = true;
        private bool isSeachInfoVisibility = true;
        private bool isAllShowItem = true;
        private bool isPaymentOnly = false;
        private bool isWithdrawalOnly = false;
        private bool isOutputGroupEnabled = true;
        private bool isBalanceFinalAccountOutputEnabled;
        private bool isReceiptsAndExpenditureOutputButtonEnabled;
        private bool isPaymentSlipsOutputEnabled;
        private bool isWithdrawalSlipsOutputEnabled;
        private bool isOutput = false;
        private bool isContainOutputted = false;
        private bool isLocationSearch = false;
        private bool isValidityTrueOnly = false;
        private bool isPreviousDayOutput = false;
        private bool isPreviousDayOutputEnabled = false;
        private bool isPasswordEnabled = false;
        private bool passwordCharCheck = false;
        private bool isYokohamaBankCheck = false;
        private bool isCeresaCheck = false;
        private bool isClose = true;
        #endregion
        #region DateTime
        private DateTime searchEndDate = DateTime.Today;
        private DateTime searchStartDate = DefaultDate;
        private DateTime searchOutputDateEnd = DateTime.Today;
        private DateTime searchOutputDateStart = DefaultDate;
        #endregion
        #region ObservableCollection
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures =
            new ObservableCollection<ReceiptsAndExpenditure>();
        private ObservableCollection<ReceiptsAndExpenditure> AllDataList =
            new ObservableCollection<ReceiptsAndExpenditure>();
        /// <summary>
        /// 本日付で出力済みの伝票データのリスト
        /// </summary>
        private ObservableCollection<ReceiptsAndExpenditure> TodayWroteList =
            new ObservableCollection<ReceiptsAndExpenditure>();
        #endregion
        private SolidColorBrush detailBackGroundColor;
        private Cashbox Cashbox = Cashbox.GetInstance();
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        private readonly ReceiptsAndExpenditureOperation ReceiptsAndExpenditureOperation =
            ReceiptsAndExpenditureOperation.GetInstance();
        private Pagination pagination;
        #endregion

        public ReceiptsAndExpenditureMangementViewModel
            (IDataOutput dataOutput, IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            ReceiptsAndExpenditureOperation.Add(this);
            ReferenceReceiptsAndExpenditures(true);
            ReceiptsAndExpenditureOperation.SetOperationType
                (ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure);
            DataOutput = dataOutput;
            IsPreviousDayOutputEnabled = false;
            PreviousDayFinalAccount = AccountingProcessLocation.OriginalTotalAmount;
            SetProperty();
            SetDelegateCommand();
            DefaultListExpress();
            SetBalanceFinalAccount();
            RefreshList();
        }
        public ReceiptsAndExpenditureMangementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataOutput(),
                DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        public DelegateCommand PasswordCheckReversCommand { get; set; }
        /// <summary>
        /// パスワードの文字を隠すかのチェックを反転させます
        /// </summary>
        private void CheckRevers()
        {
            PasswordCharCheck = !PasswordCharCheck;
        }

        public void SetSortColumns()
        {
            Pagination.SortColumns = new Dictionary<int, string>()
            {
                {0, "ID"},
                {1,"科目コード" },
                {2,"入出金日" },
                {3,"伝票出力日" }
            };
        }

        /// <summary>
        /// データ更新を行う画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowUpdateCommand { get; set; }
        private async void ShowUpdate()
        {
            await Task.Delay(1);
            ReceiptsAndExpenditureOperation.SetData(SelectedReceiptsAndExpenditure);
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureOperation());
        }
        /// <summary>
        /// データ登録を行う画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowRegistrationCommand { get; set; }
        private void ShowRegistration()
        {
            ReceiptsAndExpenditureOperation.SetOperationType
                (ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure);
            ReceiptsAndExpenditureOperation.SetData(null);
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureOperation());
        }

        /// <summary>
        /// 出納データリストを再検索するコマンド
        /// </summary>
        public DelegateCommand RefreshListCommand { get; set; }
        private void RefreshList()
        {
            ReferenceReceiptsAndExpenditures(true);
            SetTodayWroteList();
            SetPeymentSum();
            SetWithdrawalSumAndTransferSum();
            SetBalanceFinalAccount();
        }
        /// <summary>
        /// 伝票出力で使用するリストを表示するコマンド
        /// </summary>
        public DelegateCommand DefaultListExpressCommand { get; set; }
        private void DefaultListExpress()
        {
            IsPeriodSearch = true;
            SearchOutputDateStart = DefaultDate;
            SearchOutputDateEnd = DefaultDate;
            SearchStartDate =
                DateTime.Today.Day == 1 ?
                    DateTime.Today.AddDays(-10) :
                    DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1));
            SearchEndDate = DateTime.Today;
            switch (AccountingProcessLocation.Location)
            {
                case "管理事務所":
                    IsLocationSearch = DateTime.Now.Hour >= ClosingCashboxHour;
                    break;
                case "青蓮堂":
                    IsLocationSearch = true;
                    break;
                default:
                    break;
            }
            IsValidityTrueOnly = true;
            IsContainOutputted = false;
        }
        /// <summary>
        /// 出金伝票出力コマンド
        /// </summary>
        public DelegateCommand WithdrawalSlipsOutputCommand { get; set; }
        private void WithdrawalSlipsOutput() { SlipsOutput(false); }
        /// <summary>
        /// 入金伝票出力コマンド
        /// </summary>
        public DelegateCommand PaymentSlipsOutputCommand { get; set; }
        private void PaymentSlipsOutput() { SlipsOutput(true); }

        /// <summary>
        /// 伝票を出力します
        /// </summary>
        /// <param name="isPayment">入出金チェック</param>
        private async void SlipsOutput(bool isPayment)
        {
            string s = PaymentSlipsOutputButtonContent;
            PaymentSlipsOutputButtonContent = "出力中";
            IsOutputGroupEnabled = false;
            IsClose = false;
            await Task.Run(() => SlipsOutputProcess());
            SetOutputGroupEnabled();
            IsClose = true;
            PaymentSlipsOutputButtonContent = s;

            void SlipsOutputProcess()
            {
                DataOutput.PaymentAndWithdrawalSlips
                    (AllDataList, isPayment, IsPreviousDayOutput);
                foreach (ReceiptsAndExpenditure rae in AllDataList)
                {
                    if (rae.IsPayment != isPayment) { continue; }
                    rae.IsUnprinted = false;
                    _ = DataBaseConnect.Update(rae);
                    if (IsPreviousDayOutput)
                    {
                        _ = DataBaseConnect.ReceiptsAndExpenditurePreviousDayChange(rae);
                    }
                }
            }
        }
        /// <summary>
        /// 金庫金額計算ウィンドウ表示コマンド
        /// </summary>
        public DelegateCommand ShowRemainingCalculationViewCommand { get; set; }
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
            Pagination.SelectedSortColumn = Pagination.SortColumns[0];
            SearchStartDate = DateTime.Today;
            TodaysFinalAccount = AmountWithUnit(ReturnTodaysFinalAccount());
            SetOutputGroupEnabled();
            IsReceiptsAndExpenditureOutputButtonEnabled = true;
            IsBalanceFinalAccountOutputEnabled = true;
            IsPaymentSlipsOutputEnabled = true;
            IsWithdrawalSlipsOutputEnabled = true;
            IsAllShowItem = true;
            Pagination.SortDirectionIsASC = false;
            IsContainOutputted = false;
            SearchStartDate = DateTime.Today.AddDays(-1);
            SearchEndDate = DateTime.Today;
            BalanceFinalAccountOutputButtonContent = "収支日報";
            ReceiptsAndExpenditureOutputButtonContent = "出納帳";
            PaymentSlipsOutputButtonContent = "入金伝票";
            WithdrawalSlipsOutputButtonContent = "出金伝票";
            IsLocationSearch = ClosingCashboxHour > DateTime.Now.Hour;
            IsPreviousDayOutput = false;
            ReferenceLocationCheckBoxContent = $"経理担当場所{Space}:{Space}" +
                                                                        $"{AccountingProcessLocation.Location}の伝票のみを表示";
            IsPreviousDayOutputEnabled =
                LoginRep.GetInstance().Rep != null && LoginRep.GetInstance().Rep.IsAdminPermisson &&
                GetErrors(nameof(Password)) == null;
            SetListTitle();
        }
        /// <summary>
        /// 一覧のタイトルをセットします
        /// </summary>
        private void SetListTitle()
        {
            SetTodayWroteList();
            
            previousDayFinalAccount = AccountingProcessLocation.OriginalTotalAmount;

            FinalAccountCategory = AccountingProcessLocation.Location == "管理事務所" ? "前日決算" : "預かり金額";

            ListTitle = $"一覧 : {FinalAccountCategory} {AmountWithUnit(PreviousDayFinalAccount)}";
        }
        /// <summary>
        /// 今日付けで伝票出力したデータリストを保持します
        /// </summary>
        private void SetTodayWroteList()
        {
            if (AccountingProcessLocation.Location == "青蓮堂") { return; }
            TodayWroteList = DataBaseConnect.ReferenceReceiptsAndExpenditure
                   (new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), string.Empty, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, false, true, true, true,
                        new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Cashboxのトータル金額と決算額を比較して、OutputButtonのEnabledを設定します
        /// </summary>
        private void SetOutputGroupEnabled()
        {
            bool b = Cashbox.GetTotalAmount() == ListAmount;
            IsOutputGroupEnabled = b;
            if (b) { b = !IsContainOutputted; }
            IsPaymentSlipsOutputEnabled = AccountingProcessLocation.Location == "管理事務所"
                ? (IsPasswordEnabled = IsWithdrawalSlipsOutputEnabled = b) :
                    (IsPasswordEnabled = IsWithdrawalSlipsOutputEnabled = false);
            IsBalanceFinalAccountOutputEnabled = b;
        }
        /// <summary>
        /// 金庫データをViewにセットする
        /// </summary>
        public DelegateCommand SetCashboxTotalAmountCommand { get; set; }
        private void SetCashboxTotalAmount()
        {
            Cashbox = Cashbox.GetInstance();
            todayTotalAmount = Cashbox.GetTotalAmount();

            CashBoxTotalAmount = todayTotalAmount == 0 ?
                "金庫の金額を計上して下さい" : $"金庫の金額 : {AmountWithUnit(todayTotalAmount)}";

            if (todayTotalAmount == PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum)
            {
                TodaysFinalAccount = AmountWithUnit(todayTotalAmount);
            }
            else
            {
                int difference = PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum -
                    Cashbox.GetTotalAmount();
                TodaysFinalAccount =
                    $"リストの収支に対して現金が\r\n{AmountWithUnit(Math.Abs(difference))}" +
                    $"{(difference < 0 ? "超過" : "不足")}しています";
            }
            SetOutputGroupEnabled();
        }
        /// <summary>
        /// 出納帳管理画面呼び出しコマンド
        /// </summary>
        public DelegateCommand ShowCashJournalManagementCommand { get; set; }
        private void ShowCashJournalManagement()
        {
            CreateShowWindowCommand(ScreenTransition.CashJournalManagement());
        }

        /// <summary>
        /// 収支日報出力コマンド
        /// </summary>
        public DelegateCommand BalanceFinalAccountOutputCommand { get; set; }
        /// <summary>
        /// 収支日報を出力します
        /// </summary>
        private async void BalanceFinalAccountOutput()
        {
            BalanceFinalAccountOutputButtonContent = "出力中";
            IsOutputGroupEnabled = false;
            IsClose = false;
            await Task.Run(() =>
            DataOutput.BalanceFinalAccount(AmountWithUnit(PreviousDayFinalAccount),
                PaymentSumDisplayValue, WithdrawalSumDisplayValue, TransferSumDisplayValue,
                TodaysFinalAccount, AmountWithUnit(IntAmount(YokohamaBankAmount)),
                AmountWithUnit(IntAmount(CeresaAmount)), AmountWithUnit(IntAmount(WizeCoreAmount)),
                IsYokohamaBankCheck, IsCeresaCheck));
            BalanceFinalAccountOutputButtonContent = "収支日報";
            IsOutputGroupEnabled = true;
            IsClose = true;
            AccountingProcessLocation.IsBalanceAccountOutputed = true;
        }

        protected void SetDelegateCommand()
        {
            BalanceFinalAccountOutputCommand =
                new DelegateCommand(() => BalanceFinalAccountOutput(),
                () => IsBalanceFinalAccountOutputEnabled);
            ShowCashJournalManagementCommand =
                new DelegateCommand(() => ShowCashJournalManagement(),
                () => IsReceiptsAndExpenditureOutputButtonEnabled);
            ShowRemainingCalculationViewCommand =
                new DelegateCommand(() => ShowRemainingCalculationView(), () => true);
            SetCashboxTotalAmountCommand =
                new DelegateCommand(() => SetCashboxTotalAmount(), () => true);
            PaymentSlipsOutputCommand =
                new DelegateCommand(() => PaymentSlipsOutput(), () => IsPaymentSlipsOutputEnabled);
            WithdrawalSlipsOutputCommand =
                new DelegateCommand(() => WithdrawalSlipsOutput(), () => IsWithdrawalSlipsOutputEnabled);
            DefaultListExpressCommand = new DelegateCommand(() => DefaultListExpress(), () => true);
            RefreshListCommand = new DelegateCommand(() => RefreshList(), () => true);
            ShowRegistrationCommand = new DelegateCommand(() => ShowRegistration(), () => true);
            ShowUpdateCommand = new DelegateCommand(() => ShowUpdate(), () => true);
            ShowCashJournalManagementCommand = new DelegateCommand
                (() => ShowCashJournalManagement(), () => true);
            PasswordCheckReversCommand = new DelegateCommand(() => CheckRevers(), () => true);
        }
        /// <summary>
        /// 本日の決算額を返します
        /// </summary>
        /// <returns></returns>
        private int ReturnTodaysFinalAccount()
        {
            return PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum;
        }

        /// <summary>
        /// 出納データから出金データを取り出し、出金、振替に振り分けて合計を算出します
        /// </summary>
        private void SetWithdrawalSumAndTransferSum()
        {
            WithdrawalSum = 0;
            TransferSum = 0;
            if (AccountingProcessLocation.Location == "管理事務所")
            {
                foreach (ReceiptsAndExpenditure rae in TodayWroteList)
                {
                    ContainTodayWroteWithdrawal(rae);
                }
            }

            foreach (ReceiptsAndExpenditure rae in AllDataList)
            {
                if (!rae.IsPayment) { WithdrawalAllocation(rae); }
            }

            void ContainTodayWroteWithdrawal(ReceiptsAndExpenditure rae)
            {
                if (!rae.IsPayment) { WithdrawalAllocation(rae); }
            }
        }
        /// <summary>
        /// 出金データを出金、振替に振り分けます
        /// </summary>
        /// <param name="receiptsAndExpenditure"></param>
        private void WithdrawalAllocation(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            if (receiptsAndExpenditure.Content.Text == "口座入金")
            {
                TransferSum += receiptsAndExpenditure.Price;
            }
            else
            {
                WithdrawalSum += receiptsAndExpenditure.Price;
            }
        }
        /// <summary>
        /// 本日の入金合計を算出します
        /// </summary>
        private void SetPeymentSum()
        {
            int i = 0;
            if (AccountingProcessLocation.Location == "管理事務所")
            {
                foreach (ReceiptsAndExpenditure rae in TodayWroteList)
                { ContainTodayWrotePayment(rae); }
            }
            foreach (ReceiptsAndExpenditure rae in AllDataList)
            {
                if (rae.IsPayment) { i += rae.Price; }
            }
            PaymentSum = i;

            void ContainTodayWrotePayment(ReceiptsAndExpenditure rae)
            {
                i += rae.IsPayment ? rae.Price : 0;
            }
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
        /// 期間検索チェック
        /// </summary>
        public bool IsPeriodSearch
        {
            get => isPeriodSearch;
            set
            {
                isPeriodSearch = value;
                IsSearchInfoVisibility = value;
                if (!value) { SearchEndDate = SearchStartDate; }
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
                if (SearchEndDate < value) { SearchEndDate = value; }
                searchStartDate = value;
                if (!IsPeriodSearch) { SearchEndDate = value; }
                ReferenceReceiptsAndExpenditures(true);
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
                searchEndDate = SearchStartDate > value ? SearchStartDate : value;
                ReferenceReceiptsAndExpenditures(true);
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
                ReferenceReceiptsAndExpenditures(true);
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
                ReferenceReceiptsAndExpenditures(true);
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
                ReferenceReceiptsAndExpenditures(true);
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
        /// 前日決算
        /// </summary>
        public int PreviousDayFinalAccount
        {
            get => previousDayFinalAccount;
            set
            {
                previousDayFinalAccount = value;
                PreviousDayFinalAccountDisplayValue = AmountWithUnit(value);
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
            get => paymentSum;
            set
            {
                paymentSum = value;
                PaymentSumDisplayValue = AmountWithUnit(value);
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
                WithdrawalSumDisplayValue = AmountWithUnit(value);
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
                TransferSumDisplayValue = AmountWithUnit(value);
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
                yokohamaBankAmount = CommaDelimitedAmount(value);
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
                ceresaAmount = CommaDelimitedAmount(value);
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
                wizeCoreAmount = CommaDelimitedAmount(value);
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
        public string PaymentSumDisplayValue
        {
            get => paymentSumDisplayValue;
            set
            {
                paymentSumDisplayValue = value;
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
        /// 出金伝票出力ボタンのContent
        /// </summary>
        public string WithdrawalSlipsOutputButtonContent
        {
            get => withdrawalSlipsOutputButtonContent;
            set
            {
                withdrawalSlipsOutputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出金伝票出力ボタンのEnabled
        /// </summary>
        public bool IsWithdrawalSlipsOutputEnabled
        {
            get => isWithdrawalSlipsOutputEnabled;
            set
            {
                isWithdrawalSlipsOutputEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力されたかのチェック
        /// </summary>
        public bool IsOutput
        {
            get => isOutput;
            set
            {
                isOutput = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力したデータも含むかのチェック
        /// </summary>
        public bool IsContainOutputted
        {
            get => isContainOutputted;
            set
            {
                isContainOutputted = value;
                if (value)
                {
                    SearchOutputDateEnd = DateTime.Today;
                    OutputMenuHeader = "各種出力（出力済みのデータがリストに表示されているため、操作出来ません。）";
                }
                else
                {
                    SearchOutputDateEnd = DefaultDate;
                    SearchOutputDateStart = DefaultDate;
                    OutputMenuHeader = "各種出力";
                }
                ReferenceReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 経理担当場所のデータのみを表示するかのチェック
        /// </summary>
        public bool IsLocationSearch
        {
            get => isLocationSearch;
            set
            {
                isLocationSearch = value;
                ReferenceReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 有効なデータのみ検索するかのチェック
        /// </summary>
        public bool IsValidityTrueOnly
        {
            get => isValidityTrueOnly;
            set
            {
                isValidityTrueOnly = value;
                ReferenceReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票発行日ベースでの検索の初めの日付
        /// </summary>
        public DateTime SearchOutputDateStart
        {
            get => searchOutputDateStart;
            set
            {
                if (SearchOutputDateEnd < value) { searchStartDate = SearchOutputDateEnd; }
                else { searchOutputDateStart = value; }
                ReferenceReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票発行日ベースでの検索の終わりの日付
        /// </summary>
        public DateTime SearchOutputDateEnd
        {
            get => searchOutputDateEnd;
            set
            {
                searchOutputDateEnd = SearchOutputDateStart > value ? SearchOutputDateStart : value;
                ReferenceReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 経理担当場所検索チェックボックスのContent
        /// </summary>
        public string ReferenceLocationCheckBoxContent
        {
            get => referenceLocationCheckBoxContent;
            set
            {
                referenceLocationCheckBoxContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前日の日付で出力するかのチェック
        /// </summary>
        public bool IsPreviousDayOutput
        {
            get => isPreviousDayOutput;
            set
            {
                if (value == isPreviousDayOutput) { return; }

                isPreviousDayOutput = value;
                CallPropertyChanged();

                MessageBox = new MessageBoxInfo()
                {
                    Button = MessageBoxButton.YesNo,
                    Title = "チェック確認",
                    Image = MessageBoxImage.Information,
                    Message =
                    $"原則は本日の日付で伝票出力して下さい。\r\n" +
                    $" 前日での出力設定に変更してよろしいですか？"
                };
                CallShowMessageBox = true;

                MessageBoxResult result = MessageBox.Result;

                isPreviousDayOutput = result == MessageBoxResult.Yes;

            }
        }
        /// <summary>
        /// 前日の日付で出力するチェックのEnabled
        /// </summary>
        public bool IsPreviousDayOutputEnabled
        {
            get => isPreviousDayOutputEnabled;
            set
            {
                isPreviousDayOutputEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 職員パスワード入力
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidationProperty(nameof(Password), value);
                IsPreviousDayOutputEnabled = GetErrors(nameof(Password)) == null;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パスワードのEnabled
        /// </summary>
        public bool IsPasswordEnabled
        {
            get => isPasswordEnabled;
            set
            {
                isPasswordEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パスワードの文字を隠すかのチェック
        /// </summary>
        public bool PasswordCharCheck
        {
            get => passwordCharCheck;
            set
            {
                passwordCharCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 横浜銀行残高の変化なしチェック
        /// </summary>
        public bool IsYokohamaBankCheck
        {
            get => isYokohamaBankCheck;
            set
            {
                isYokohamaBankCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// セレサ川崎残高の変化なしチェック
        /// </summary>
        public bool IsCeresaCheck
        {
            get => isCeresaCheck;
            set
            {
                isCeresaCheck = value;
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
        /// ウィンドウを閉じる許可を統括
        /// </summary>
        public bool IsClose
        {
            get => isClose;
            set
            {
                isClose = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 管理事務所なら「前日残高」、青蓮堂なら「預り金」
        /// </summary>
        public string PreviousDayBalanceText =>
            AccountingProcessLocation.Location == "管理事務所" ? "前日残高" : "預り金";
        /// <summary>
        /// 出力メニューグループボックスのHeader 
        /// </summary>
        public string OutputMenuHeader
        {
            get => outputMenuHeader;
            set
            {
                outputMenuHeader = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// リストの収支決算を表示します
        /// </summary>
        private void SetBalanceFinalAccount()
        {
            ListAmount = PreviousDayFinalAccount;

            foreach (ReceiptsAndExpenditure receiptsAndExpenditure in AllDataList)
            {
                if (receiptsAndExpenditure.IsPayment) { ListAmount += receiptsAndExpenditure.Price; }
                else { ListAmount -= receiptsAndExpenditure.Price; }
            }

            if (AccountingProcessLocation.Location == "青蓮堂") { TodayWroteList.Clear(); }

            int todayPayment = 0;
            int todayWithdrawal = default;

            if (TodayWroteList.Count != 0)
            {
                foreach (ReceiptsAndExpenditure receiptsAndExpenditure in TodayWroteList)
                {
                    if (receiptsAndExpenditure.IsPayment) { todayPayment += receiptsAndExpenditure.Price; }
                    else { todayWithdrawal += receiptsAndExpenditure.Price; }
                }
                ListAmount += todayPayment - todayWithdrawal;
                BalanceFinalAccount =
                    $"{FinalAccountCategory}{Space}+{Space}入金伝票{Space}-{Space}出金伝票\r\n" +
                    $"{PreviousDayFinalAccountDisplayValue}{Space}+{Space}" +
                    $"{AmountWithUnit(PaymentSum)}-{Space}{AmountWithUnit(WithdrawalSum + TransferSum)}" +
                    $"{Space}={Space}{AmountWithUnit(ListAmount)}\r\n" +
                    $"（本日出力済み伝票{Space}入金{Space}:{Space}{AmountWithUnit(todayPayment)}、" +
                    $"出金{Space}:{Space}{AmountWithUnit(todayWithdrawal)}分を含む）";
            }
            else
            {
                BalanceFinalAccount =
                      $"{FinalAccountCategory} + 入金伝票 - 出金伝票 : {AmountWithUnit(ListAmount)}";
            }
            SetCashboxTotalAmount();
            SetOutputButtonEnabled(ListAmount);
        }
        /// <summary>
        /// 金庫の総額と決算額があっていれば、各EnabledをTrueにします
        /// </summary>
        /// <param name="amount">決算額</param>
        private void SetOutputButtonEnabled(int amount)
        {
            IsBalanceFinalAccountOutputEnabled =
                IsReceiptsAndExpenditureOutputButtonEnabled = IsPaymentSlipsOutputEnabled =
                IsWithdrawalSlipsOutputEnabled = isPasswordEnabled =
                IsOutputGroupEnabled = Cashbox.GetTotalAmount() == amount;

            if (IsBalanceFinalAccountOutputEnabled)
            {
                IsReceiptsAndExpenditureOutputButtonEnabled = IsPaymentSlipsOutputEnabled =
                    IsWithdrawalSlipsOutputEnabled = isPasswordEnabled =
                    IsOutputGroupEnabled = !IsContainOutputted;
            }
            IsPreviousDayOutputEnabled = false;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(Password):
                    ErrorsListOperation
                        (string.IsNullOrEmpty(Password), propertyName, Properties.Resources.NullErrorInfo);
                    ErrorsListOperation
                        (GetHashValue(password, LoginRep.GetInstance().Rep.ID) !=
                            LoginRep.GetInstance().Rep.Password, propertyName,
                        Properties.Resources.PasswordErrorInfo);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 出納データを検索して、リストに格納します
        /// </summary>
        private void ReferenceReceiptsAndExpenditures(bool isPageCountReset)
        {
            DateTime AccountActivityDateStart;
            DateTime AccountActivityDateEnd;
            DateTime OutputDateStart;
            DateTime OutputDateEnd;

            PaymentSum = 0;
            WithdrawalSum = 0;
            TransferSum = 0;

            if (IsPeriodSearch)
            {
                AccountActivityDateStart = SearchStartDate;
                OutputDateStart = SearchOutputDateStart;
                AccountActivityDateEnd =
                    SearchEndDate == null ? new DateTime(9999, 1, 1) : SearchEndDate;
                OutputDateEnd =
                    SearchOutputDateEnd == null ? new DateTime(9999, 1, 1) : SearchOutputDateEnd;
            }
            else
            {
                AccountActivityDateStart = SearchStartDate;
                OutputDateStart = SearchOutputDateStart;
                AccountActivityDateEnd = AccountActivityDateStart;
                OutputDateEnd = SearchOutputDateStart;
            }

            string Location;
            Location = IsLocationSearch ? AccountingProcessLocation.Location : string.Empty;

            ListTitle = "描画中です。お待ちください。";

            CreateReceiptsAndExpenditures
                (AccountActivityDateStart, AccountActivityDateEnd, OutputDateStart, OutputDateEnd,
                    Location, isPageCountReset);
            Pagination.SetProperty();
            ListTitle = $"一覧 : {FinalAccountCategory} {AmountWithUnit(PreviousDayFinalAccount)}";
            SetCashboxTotalAmount();
        }
        /// <summary>
        /// データベースに接続して出納データリストを生成します
        /// </summary>
        /// <param name="accountActivityDateStart"></param>
        /// <param name="accountActivityDateEnd"></param>
        /// <param name="outputDateStart"></param>
        /// <param name="outputDateEnd"></param>
        /// <param name="location"></param>
        private void CreateReceiptsAndExpenditures
            (DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart,
                DateTime outputDateEnd, string location, bool isPageCountReset)
        {
            Pagination.CountReset(isPageCountReset);

            (int count, ObservableCollection<ReceiptsAndExpenditure> list) =
                DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                location, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, !IsAllShowItem,
                IsPaymentOnly, IsContainOutputted, IsValidityTrueOnly, accountActivityDateStart,
                accountActivityDateEnd, outputDateStart, outputDateEnd, Pagination.PageCount,
                Pagination.SelectedSortColumn, Pagination.SortDirectionIsASC);
            Pagination.TotalRowCount = count;
            ReceiptsAndExpenditures = list;

            AllDataList =
                DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                location, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, !IsAllShowItem,
                IsPaymentOnly, IsContainOutputted, IsValidityTrueOnly, accountActivityDateStart,
                accountActivityDateEnd, outputDateStart, outputDateEnd);

            SetPeymentSum();
            SetWithdrawalSumAndTransferSum();
            SetBalanceFinalAccount();
        }

        protected override void SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"出納管理 : {AccountingProcessLocation.Location}";
        }

        public void ReceiptsAndExpenditureOperationNotify() { RefreshList(); }

        public void SortNotify() { ReferenceReceiptsAndExpenditures(true); }

        public void PageNotify() { ReferenceReceiptsAndExpenditures(false); }

        public bool OnClosing() { return !IsClose; }
    }
}