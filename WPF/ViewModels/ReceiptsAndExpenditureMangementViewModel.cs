using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納管理ウィンドウViewModel
    /// </summary>
    public class ReceiptsAndExpenditureMangementViewModel : BaseViewModel,
        IReceiptsAndExpenditureOperationObserver
    {
        #region Properties
        #region int
        private int peymentSum;
        private int withdrawalSum;
        private int transferSum;
        private int previousDayFinalAccount;
        private int todayTotalAmount;
        /// <summary>
        /// リストデータの総数
        /// </summary>
        private int RowCount;
        /// <summary>
        /// リストのページ
        /// </summary>
        private int PageCount;
        /// <summary>
        /// リストのページの総数
        /// </summary>
        private int TotalPageCount;
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
        private string peymentSumDisplayValue;
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
        private string listPageInfo;
        /// <summary>
        /// 当日決算の基準になる金額の種類。管理事務所なら前日決算、青蓮堂なら預り金額
        /// </summary>
        private string FinalAccountCategory;
        #endregion
        #region bool
        private bool isPeriodSearch;
        private bool isSeachInfoVisibility;
        private bool isAllShowItem;
        private bool isPaymentOnly;
        private bool isWithdrawalOnly;
        private bool isOutputGroupEnabled;
        private bool isBalanceFinalAccountOutputEnabled;
        private bool isReceiptsAndExpenditureOutputButtonEnabled;
        private bool isPaymentSlipsOutputEnabled;
        private bool isWithdrawalSlipsOutputEnabled;
        private bool isOutput;
        private bool isContainOutputted;
        private bool isLocationSearch;
        private bool isValidityTrueOnly;
        private bool isPreviousDayOutput;
        private bool isPreviousDayOutputEnabled;
        private bool isPasswordEnabled;
        private bool passwordCharCheck;
        private bool isPrevPageEnabled;
        private bool isNextPageEnabled;
        #endregion
        #region DateTime
        private DateTime searchEndDate = new DateTime(9999, 1, 1);
        private DateTime searchStartDate = DefaultDate;
        private DateTime searchOutputDateEnd = new DateTime(9999, 1, 1);
        private DateTime searchOutputDateStart = DefaultDate;
        #endregion
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        /// <summary>
        /// 本日付で出力済みの伝票データのリスト
        /// </summary>
        private ObservableCollection<ReceiptsAndExpenditure> TodayWroteList =
            new ObservableCollection<ReceiptsAndExpenditure>();
        private SolidColorBrush detailBackGroundColor;
        private Cashbox Cashbox = Cashbox.GetInstance();
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        private readonly ReceiptsAndExpenditureOperation ReceiptsAndExpenditureOperation =
            ReceiptsAndExpenditureOperation.GetInstance();
        private readonly LoginRep LoginRep = LoginRep.GetInstance();
        #endregion

        public ReceiptsAndExpenditureMangementViewModel
            (IDataOutput dataOutput, IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            ReceiptsAndExpenditureOperation.Add(this);
            ReceiptsAndExpenditureOperation.SetOperationType
                (ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure);
            IsContainOutputted = false;
            SearchStartDate = DateTime.Today.AddDays(-1);
            SearchEndDate = DateTime.Today;
            DataOutput = dataOutput;
            SetProperty();
            DefaultListExpress();
            IsPreviousDayOutputEnabled = false;
            SetDelegateCommand();
        }
        public ReceiptsAndExpenditureMangementViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataOutput(),
            DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; set; }
        private void NextPageListExpress()
        {
            if (PageCount == 0) return;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            ReferenceReceiptsAndExpenditures();
        }
        /// <summary>
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress()
        {
            if (PageCount == 0) return;
            if (PageCount > 1) PageCount--;
            ReferenceReceiptsAndExpenditures();
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
            PageCount = 1;
            ReferenceReceiptsAndExpenditures();
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
            SearchStartDate = DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1));
            SearchEndDate = DateTime.Today;
            switch(AccountingProcessLocation.Location)
            {
                case "管理事務所":
                    IsLocationSearch = DateTime.Now.Hour >= ClosingCashboxHour;
                    break;
                case "青蓮堂":
                    IsLocationSearch = true;
                    break;
            }
            IsValidityTrueOnly = true;
            IsContainOutputted = false;
        }
        /// <summary>
        /// 出金伝票出力コマンド
        /// </summary>
        public DelegateCommand WithdrawalSlipsOutputCommand { get; set; }
        private async void WithdrawalSlipsOutput()
        {
            WithdrawalSlipsOutputButtonContent = "出力中";
            IsWithdrawalSlipsOutputEnabled = false;
            await Task.Run(() => SlipsOutputProcess(false));
            IsWithdrawalSlipsOutputEnabled = true;
            WithdrawalSlipsOutputButtonContent = "出金伝票";
        }

        /// <summary>
        /// 入金伝票出力コマンド
        /// </summary>
        public DelegateCommand PaymentSlipsOutputCommand { get; set; }
        private async void PaymentSlipsOutput()
        {
            PaymentSlipsOutputButtonContent = "出力中";
            IsPaymentSlipsOutputEnabled = false;
            await Task.Run(() => SlipsOutputProcess(true));
            IsPaymentSlipsOutputEnabled = true;
            PaymentSlipsOutputButtonContent = "入金伝票";
        }
        /// <summary>
        /// 伝票を出力します
        /// </summary>
        /// <param name="isPayment">入出金チェック</param>
        private void SlipsOutputProcess(bool isPayment)
        {
            DataOutput.PaymentAndWithdrawalSlips
                (ReceiptsAndExpenditures, LoginRep.Rep, isPayment, IsPreviousDayOutput);
            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures)
            {
                if (rae.IsPayment != isPayment) continue;
                rae.IsUnprinted = false;
                DataBaseConnect.Update(rae);
                if (IsPreviousDayOutput)
                    DataBaseConnect.ReceiptsAndExpenditurePreviousDayChange(rae);
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
            ObservableCollection<ReceiptsAndExpenditure> UnPrintedList;

            SearchStartDate = DateTime.Today;
            TodaysFinalAccount = TextHelper.AmountWithUnit(ReturnTodaysFinalAccount());
            SetOutputGroupEnabled();
            IsReceiptsAndExpenditureOutputButtonEnabled = true;
            IsBalanceFinalAccountOutputEnabled = true;
            IsPaymentSlipsOutputEnabled = true;
            IsWithdrawalSlipsOutputEnabled = true;
            IsAllShowItem = true;
            BalanceFinalAccountOutputButtonContent = "収支日報";
            ReceiptsAndExpenditureOutputButtonContent = "出納帳";
            PaymentSlipsOutputButtonContent = "入金伝票";
            WithdrawalSlipsOutputButtonContent = "出金伝票";
            PageCount = ReceiptsAndExpenditures.Count > 0 ? 1 : 0;
            if (ClosingCashboxHour < DateTime.Now.Hour)
            {
                ReceiptsAndExpenditures =
                    DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                    AccountingProcessLocation.Location, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, false, true, false, true, new DateTime(1900, 1, 1), new DateTime(9999, 1, 1),
                    new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), PageCount).List;
                UnPrintedList =
                    DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                    AccountingProcessLocation.Location, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, false, true, false, true, new DateTime(1900, 1, 1), new DateTime(9999, 1, 1),
                    new DateTime(1900, 1, 1), new DateTime(1900, 1, 1));
            }
            else
            {
                ReceiptsAndExpenditures =
                    DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, true,
                    false, true, new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), new DateTime(1900, 1, 1),
                    new DateTime(1900, 1, 1), PageCount).List;
                UnPrintedList =
                    DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, true,
                    false, true, new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), new DateTime(1900, 1, 1),
                    new DateTime(1900, 1, 1), PageCount).List;
            }
            IsPreviousDayOutput = false;
            ReferenceLocationCheckBoxContent = $"{AccountingProcessLocation.Location}の伝票のみを表示";
            IsPreviousDayOutputEnabled = LoginRep.Rep.IsAdminPermisson;
            SetListTitle();
            SetPeymentSum(UnPrintedList);
            SetWithdrawalSumAndTransferSum(UnPrintedList);
            SetCashboxTotalAmount();
        }
        /// <summary>
        /// 一覧のタイトルをセットします
        /// </summary>
        private void SetListTitle()
        {
            if (AccountingProcessLocation.Location == "管理事務所")
            {
                TodayWroteList=DataBaseConnect.ReferenceReceiptsAndExpenditure
                   (new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), string.Empty, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, false, true, true, true, 
                        new DateTime(1900, 1, 1), new DateTime(9999, 1, 1), DateTime.Today, DateTime.Today);
                PreviousDayFinalAccount = DataBaseConnect.PreviousDayFinalAmount();
                FinalAccountCategory = "前日決算";
            }
            else
            {
                previousDayFinalAccount = AccountingProcessLocation.OriginalTotalAmount;
                FinalAccountCategory = "預かり金額";
            }
            ListTitle = $"一覧 : {FinalAccountCategory} {AmountWithUnit(PreviousDayFinalAccount)}";
        }
        /// <summary>
        /// Cashboxのトータル金額と決算額を比較して、OutputButtonのEnabledを設定します
        /// </summary>
        private void SetOutputGroupEnabled() =>
            IsOutputGroupEnabled = Cashbox.GetTotalAmount() == ListAmount;
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
                TodaysFinalAccount = AmountWithUnit(todayTotalAmount);
            else TodaysFinalAccount = "数字が合っていません。確認して下さい。";
            SetOutputGroupEnabled();
        }
        /// <summary>
        /// 出納データ出力コマンド
        /// </summary>
        public DelegateCommand ReceiptsAndExpenditureOutputCommand { get; set; }
        private async void ReceiptsAndExpenditureOutput()
        {
            ReceiptsAndExpenditureOutputButtonContent = "出力中";
            IsOutputGroupEnabled = false;
            await Task.Run(() =>
            DataOutput.ReceiptsAndExpenditureData(ReceiptsAndExpenditures, PreviousDayFinalAccount));
            ReceiptsAndExpenditureOutputButtonContent = "出納データ";
            IsOutputGroupEnabled = true;
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
            await Task.Run(() =>
            DataOutput.BalanceFinalAccount(AmountWithUnit(PreviousDayFinalAccount),
                PeymentSumDisplayValue,WithdrawalSumDisplayValue, TransferSumDisplayValue, 
                TodaysFinalAccount, YokohamaBankAmount, 
                CeresaAmount, WizeCoreAmount));
            BalanceFinalAccountOutputButtonContent = "収支日報";
            IsOutputGroupEnabled = true;
        }

        protected void SetDelegateCommand()
        {
            BalanceFinalAccountOutputCommand =
                new DelegateCommand(() => BalanceFinalAccountOutput(),
                () => IsBalanceFinalAccountOutputEnabled);
            ReceiptsAndExpenditureOutputCommand =
                new DelegateCommand(() => ReceiptsAndExpenditureOutput(),
                () => IsReceiptsAndExpenditureOutputButtonEnabled);
            ShowRemainingCalculationViewCommand = 
                new DelegateCommand(() => ShowRemainingCalculationView(), () => true);
            SetCashboxTotalAmountCommand = 
                new DelegateCommand(() => SetCashboxTotalAmount(), () => true);
            PaymentSlipsOutputCommand = 
                new DelegateCommand(() => PaymentSlipsOutput(), () => IsPaymentSlipsOutputEnabled);
            WithdrawalSlipsOutputCommand =
                new DelegateCommand(() => WithdrawalSlipsOutput(), 
                () => IsWithdrawalSlipsOutputEnabled);
            DefaultListExpressCommand = new DelegateCommand(() => DefaultListExpress(), () => true);
            RefreshListCommand = new DelegateCommand(() => RefreshList(), () => true);
            ShowRegistrationCommand = new DelegateCommand(() => ShowRegistration(), () => true);
            ShowUpdateCommand = new DelegateCommand(() => ShowUpdate(), () => true);
            PrevPageListExpressCommand = new DelegateCommand
                (() => PrevPageListExpress(), () => true);
            NextPageListExpressCommand = new DelegateCommand
                (() => NextPageListExpress(), () => true);
        }
        /// <summary>
        /// 本日の決算額を返します
        /// </summary>
        /// <returns></returns>
        private int ReturnTodaysFinalAccount() =>
            PreviousDayFinalAccount - WithdrawalSum - TransferSum + PaymentSum;
        /// <summary>
        /// 出納データから出金データを取り出し、出金、振替に振り分けて合計を算出します
        /// </summary>
        private void SetWithdrawalSumAndTransferSum(ObservableCollection<ReceiptsAndExpenditure>allDataList)
        {
            WithdrawalSum = 0;
            TransferSum = 0;
            foreach (ReceiptsAndExpenditure rae in TodayWroteList)
                if (!rae.IsPayment) WithdrawalAllocation(rae);
            foreach (ReceiptsAndExpenditure rae in allDataList)
                if (!rae.IsPayment) WithdrawalAllocation(rae);
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
        /// 本日の入金合計を算出します
        /// </summary>
        private void SetPeymentSum(ObservableCollection<ReceiptsAndExpenditure>allDataList)
        {
            int i = 0;
            foreach (ReceiptsAndExpenditure rae in TodayWroteList)
                if (rae.IsPayment) i += rae.Price;
            foreach (ReceiptsAndExpenditure rae in allDataList)
                if (rae.IsPayment) i += rae.Price;
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
                if (!IsPeriodSearch) SearchEndDate = value;
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount =1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                SetListPageInfo();
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
            get => peymentSum;
            set
            {
                peymentSum = value;
                PeymentSumDisplayValue = AmountWithUnit(value);
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
                if (value) SearchOutputDateEnd = DateTime.Today;
                else
                {
                    SearchOutputDateEnd = DefaultDate;
                    SearchOutputDateStart = DefaultDate;
                }
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                if (SearchOutputDateEnd < value) searchStartDate = SearchOutputDateEnd;
                else searchOutputDateStart = value;
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                if (SearchOutputDateStart > value) searchOutputDateEnd = SearchOutputDateStart;
                else searchOutputDateEnd = value;
                PageCount = 1;
                ReferenceReceiptsAndExpenditures();
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
                if (value == isPreviousDayOutput) return;

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

                if (result == MessageBoxResult.Yes) isPreviousDayOutput = true;
                else isPreviousDayOutput = false;

                CallPropertyChanged();
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
        /// 表示しているリストのページの案内
        /// </summary>
        public string ListPageInfo
        {
            get => listPageInfo;
            set
            {
                listPageInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前の10件ボタンのEnabled
        /// </summary>
        public bool IsPrevPageEnabled
        {
            get => isPrevPageEnabled;
            set
            {
                isPrevPageEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 次の10件ボタンのEnabled
        /// </summary>
        public bool IsNextPageEnabled
        {
            get => isNextPageEnabled;
            set
            {
                isNextPageEnabled = value;
                CallPropertyChanged();
            }
        }

        private void SetListPageInfo()
        {
            int i = RowCount / 10;
            i += RowCount % 10 == 0 ? 0 : 1;
            TotalPageCount = i;
            ListPageInfo = $"{PageCount}/{i}";
            IsPrevPageEnabled = PageCount > 1;
            IsNextPageEnabled = PageCount != i;
        }
        /// <summary>
        /// パスワードの文字を隠すかのチェックを反転させます
        /// </summary>
        public void CheckRevers() => PasswordCharCheck = !PasswordCharCheck;
        /// <summary>
        /// リストの収支決算を表示します
        /// </summary>
        private void SetBalanceFinalAccount(ObservableCollection<ReceiptsAndExpenditure> allDataList)
        {
           ListAmount = PreviousDayFinalAccount;
            
            foreach(ReceiptsAndExpenditure receiptsAndExpenditure in allDataList)
            {
                if (receiptsAndExpenditure.IsPayment) ListAmount += receiptsAndExpenditure.Price;
                else ListAmount -= receiptsAndExpenditure.Price;
            }

            int todayAmount = default;

            if (TodayWroteList.Count != 0)
            {
                foreach (ReceiptsAndExpenditure receiptsAndExpenditure in TodayWroteList)
                {
                    if (receiptsAndExpenditure.IsPayment) todayAmount += receiptsAndExpenditure.Price;
                    else todayAmount -= receiptsAndExpenditure.Price;
                }
                ListAmount += todayAmount;
                BalanceFinalAccount =
                    $"{FinalAccountCategory} + 入金伝票 - 出金伝票 : {AmountWithUnit(ListAmount)}\r\n" +
                    $"（本日出力済み伝票 {AmountWithUnit(todayAmount)} 分を含む）";
            }
            else BalanceFinalAccount = 
                    $"{FinalAccountCategory} + 入金伝票 - 出金伝票 : {AmountWithUnit(ListAmount)}";

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
                IsPreviousDayOutputEnabled = IsOutputGroupEnabled= Cashbox.GetTotalAmount() == amount;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(Password):
                    ErrorsListOperation
                        (string.IsNullOrEmpty(Password), propertyName, Properties.Resources.NullErrorInfo);
                    ErrorsListOperation
                        (password != LoginRep.Rep.Password, propertyName, 
                        Properties.Resources.PasswordErrorInfo);
                    break;

            }
        }
        /// <summary>
        /// 出納データを検索して、リストに格納します
        /// </summary>
        private void ReferenceReceiptsAndExpenditures()
        {
            DateTime AccountActivityDateStart;
            DateTime AccountActivityDateEnd;
            DateTime OutputDateStart;
            DateTime OutputDateEnd;

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
            if (IsLocationSearch) Location = AccountingProcessLocation.Location;
            else Location = string.Empty;

            if (IsLocationSearch) Location = AccountingProcessLocation.Location;

            ListTitle = "描画中です。お待ちください。";
            CreateReceiptsAndExpenditures
                (AccountActivityDateStart, AccountActivityDateEnd, OutputDateStart, OutputDateEnd, Location);
            SetListPageInfo();
            ListTitle = $"一覧 : {FinalAccountCategory} {AmountWithUnit(PreviousDayFinalAccount)}";

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
            DateTime outputDateEnd, string location)
        {
            ReceiptsAndExpenditures = 
                DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                location, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, !IsAllShowItem,
                IsPaymentOnly, IsContainOutputted, IsValidityTrueOnly, accountActivityDateStart, 
                accountActivityDateEnd, outputDateStart, outputDateEnd,PageCount).List;
            RowCount=
                DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                location, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, !IsAllShowItem,
                IsPaymentOnly, IsContainOutputted, IsValidityTrueOnly, accountActivityDateStart,
                accountActivityDateEnd, outputDateStart, outputDateEnd, PageCount).TotalRows;
            if (ReceiptsAndExpenditures.Count == 0) PageCount = 0;
            ObservableCollection<ReceiptsAndExpenditure> allDataList =
                DataBaseConnect.ReferenceReceiptsAndExpenditure(DefaultDate, new DateTime(9999, 1, 1),
                location, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, !IsAllShowItem,
                IsPaymentOnly, IsContainOutputted, IsValidityTrueOnly, accountActivityDateStart,
                accountActivityDateEnd, outputDateStart, outputDateEnd);
            SetBalanceFinalAccount(allDataList);
            SetPeymentSum(allDataList);
            SetWithdrawalSumAndTransferSum(allDataList);
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = 
            $"出納管理 : {AccountingProcessLocation.Location}";

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle = $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
            }
        }

        public void Notify() => RefreshList();        
    }
}