using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>gds
    /// メインウィンドウ
    /// </summary>
    public class MainWindowViewModel : BaseViewModel, IClosing, IOriginalTotalAmountObserver
    {
        #region Properties
        #region bools
        private bool callClosingMessage;
        private bool processFeatureEnabled;
        private bool shorendoChecked;
        private bool kanriJimushoChecked;
        private bool isSlipManagementEnabled;
        private bool isRegistrationPrecedingYearFinalAccountVisiblity;
        private bool isLogoutEnabled;
        private bool isCreateVoucherEnabled;
        private bool isPartTransportRegistrationEnabled;
        private bool isShunjuen;
        private bool isShunjuenMenuEnabled;
        private bool isWizeCoreMenuEnabled;
        private bool isShunjuenCommonEnabled;
        private bool isUpdatePrecedingYearFinalAccountVisibility;
        private bool isWizeCoreMenuVisibility;
        private bool isCreditDeptVisibility;
        private bool isCommonEnabled;
        private bool IsPrecedingYearFinalAccountRegisterVerified = false;
        #endregion
        private string depositAmount;
        private string showSlipManagementContent;
        private CreditDept selectedCreditDept;
        #endregion

        /// <summary>
        /// コンストラクタ　DelegateCommand、LoginRepのインスタンスを生成します
        /// </summary>
        public MainWindowViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            LoginRep.GetInstance().SetRep(new Rep(string.Empty, string.Empty, string.Empty, false, false));
            AccountingProcessLocation.GetInstance().Add(this);
            IsShunjuen = true;

            ShowRemainingMoneyCalculationCommand = new DelegateCommand
                (() => CreateShowWindowCommand
                    (ScreenTransition.RemainingMoneyCalculation()), () => true);
            MessageBoxCommand =
                new DelegateCommand(() => ClosingMessage(), () => true);
            SetLocationKanriJimushoCommand =
                new DelegateCommand(() => SetLocationKanriJimusho(), () => true);
            SetLodationShorendoCommand =
                new DelegateCommand(() => SetLocationShorendo(), () => true);
            ShowDataManagementCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.DataManagement()), () => true);
            ShowLoginCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.Login()), () => true);
            ShowReceiptsAndExpenditureManagementCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureMangement()),
                    () => SetOperationButtonEnabled());
            ShowCreateVoucherCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.CreateVoucher()), () => true);
            RegistrationPrecedingYearFinalAccountCommand =
                new DelegateCommand(() => RegistrationPrecedingYearFinalAccount(), () => true);
            LogoutCommand =
                new DelegateCommand(() => Logout(), () => true);
            ShowPartTimerTransPortCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.PartTimerTransportRegistration()),
                    () => true);
            ShowCreateCondolencesCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.CreateCondolences()), () => true);
            ShowSearchReceiptsAndExpenditureCommand = new DelegateCommand
                (() => CreateShowWindowCommand
                    (ScreenTransition.SearchReceiptsAndExpenditure()), () => true);
            CreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
            UpdatePrecedingYearFinalAccountCommand = new DelegateCommand
                (() => UpdatePrecedingYearFinalAccount(), () => true);
            ShowRegistrationFlowerReceiptsAndExpenditureCommand = new DelegateCommand
                (() => CreateShowWindowCommand
                    (ScreenTransition.RegistrationFlowerReceiptsAndExpenditure()), () => true);
            ShowProductSalesRegistrationViewCommand = new DelegateCommand
                (() => CreateShowWindowCommand
                    (ScreenTransition.ProductSalesRegistration()), () => true);
            ShowSearchCondlencesViewCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.SearchCondlences()), () => true);
            ShowTransferReceiptsAndExpenditureManagementCommand = new DelegateCommand
                (() => CreateShowWindowCommand
                    (ScreenTransition.TransferReceiptsAndExpenditureManagement()), () => true);
            ShowMemorialServiceAccountRegister = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.MemorialServiceAccountRegister()), () => true);
        }
        public MainWindowViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 法事計算書登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowMemorialServiceAccountRegister { get; }
        /// <summary>
        /// 振替出納データ管理画面表示コマンド
        /// </summary>
        public DelegateCommand ShowTransferReceiptsAndExpenditureManagementCommand { get; }
        /// <summary>
        /// 御布施一覧データ閲覧画面表示コマンド
        /// </summary>
        public DelegateCommand ShowSearchCondlencesViewCommand { get; }
        /// <summary>
        /// 物販売上登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowProductSalesRegistrationViewCommand { get; }
        /// <summary>
        /// 花売りデータ登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowRegistrationFlowerReceiptsAndExpenditureCommand { get; }
        /// <summary>
        /// 前年度決算更新コマンド
        /// </summary>
        public DelegateCommand UpdatePrecedingYearFinalAccountCommand { get; }
        private void UpdatePrecedingYearFinalAccount()
        {
            if (AccountingProcessLocation.IsAccountingGenreShunjuen) { UpdateShunjuenFinalAccount(); }
            else { UpdateWizeCoreFinalAccount(); }

            void UpdateWizeCoreFinalAccount()
            {
                if (DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today, SelectedCreditDept) ==
                    DataBaseConnect.CallFinalMonthFinalAccount(DateTime.Today, false, SelectedCreditDept))
                {
                    CallNoUPdateMessage(SelectedCreditDept.Dept,
                        DataBaseConnect.CallFinalMonthFinalAccount(DateTime.Today, false, SelectedCreditDept));
                    return;
                }
                _ = DataBaseConnect.UpdatePrecedingYearFinalAccount(SelectedCreditDept);
            }

            void UpdateShunjuenFinalAccount()
            {
                if (DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today, true) ==
                    DataBaseConnect.CallFinalMonthFinalAccount(DateTime.Today, true, null))
                {
                    CallNoUPdateMessage
                        (SHUNJUEN, DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today, true)) ;
                    return;
                }
                _ = DataBaseConnect.UpdatePrecedingYearFinalAccount();
            }

            void CallNoUPdateMessage(string dept, int finalAmount)
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message = $"{dept}{Space}前年度決算額{Space}" +
                                    $"{AmountWithUnit(finalAmount)}は更新の必要がありません。" +
                                    $"\r\n出納データを更新した場合のみクリックしてください",
                    Title = "更新不要通知",
                    Button = MessageBoxButton.OK,
                    Image = MessageBoxImage.Information
                };
                CallShowMessageBox = true;
            }
        }
        /// <summary>
        /// 貸方勘定リスト
        /// </summary>
        public readonly ObservableCollection<CreditDept> CreditDepts;
        /// <summary>
        /// 出納データ閲覧画面表示コマンド
        /// </summary>
        public DelegateCommand ShowSearchReceiptsAndExpenditureCommand { get; }
        /// <summary>
        /// 経理担当場所を管理事務所に設定するコマンド
        /// </summary>
        public DelegateCommand SetLocationKanriJimushoCommand { get; }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定するコマンド
        /// </summary>
        public DelegateCommand SetLodationShorendoCommand { get; }
        /// <summary>
        /// 金庫金額計算画面表示コマンド
        /// </summary>
        public DelegateCommand ShowRemainingMoneyCalculationCommand { get; }
        /// <summary>
        /// データ管理画面表示コマンド
        /// </summary>
        public DelegateCommand ShowDataManagementCommand { get; }
        /// <summary>
        /// ログイン画面表示コマンド
        /// </summary>
        public DelegateCommand ShowLoginCommand { get; }
        /// <summary>
        /// 伝票管理画面表示コマンド
        /// </summary>
        public DelegateCommand ShowReceiptsAndExpenditureManagementCommand { get; }
        /// <summary>
        /// 受納証発行画面表示コマンド
        /// </summary>
        public DelegateCommand ShowCreateVoucherCommand { get; }
        /// <summary>
        /// パート交通費データ登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowPartTimerTransPortCommand { get; }
        /// <summary>
        /// お布施一覧管理画面表示コマンド
        /// </summary>
        public DelegateCommand ShowCreateCondolencesCommand { get; }

        /// <summary>
        /// ログアウトコマンド
        /// </summary>
        public DelegateCommand LogoutCommand { get; set; }
        private void Logout()
        {
            LoginRep.GetInstance().SetRep(new Rep(string.Empty, string.Empty, string.Empty, false, false));
            IsSlipManagementEnabled = false;
            IsCreateVoucherEnabled = false;
            IsPartTransportRegistrationEnabled = false;
            ShowSlipManagementContent = "出納管理";
            IsLogoutEnabled = false;
            IsShunjuenCommonEnabled = false;
            IsCommonEnabled = false;
        }
        /// <summary>
        /// 前月決算額を確認したうえで登録します
        /// </summary>
        private void ConfirmationPrecedingYearFinalAccount()
        {
            //ログインアカウントが管理者権限を所持していて、なおかつ今日の日付が4月1日~4月20日なら登録ボタンを可視化する
            IsRegistrationPrecedingYearFinalAccountVisiblity =
                DateTime.Today >= CurrentFiscalYearFirstDate &&
                DateTime.Today < CurrentFiscalYearFirstDate.AddDays(20)
                && LoginRep.GetInstance().Rep.IsAdminPermisson;

            if (string.IsNullOrEmpty(LoginRep.GetInstance().Rep.Name))
            {
                CallNoLoginMessage();
                IsSlipManagementEnabled = false;
                ShowSlipManagementContent = "出納管理";
                return;
            }
            //前月決算が登録されていれば登録ボタンを隠して更新ボタンを可視化する
            if (IsRegistrationPrecedingYearFinalAccountVisiblity) { SetButtonVsisbility(); }
            //登録ボタンが可視化されていなければ処理を終了する
            if (!IsRegistrationPrecedingYearFinalAccountVisiblity) { return; }

            RegistrationPrecedingYearFinalAccount();

            void SetButtonVsisbility()
            {
                if (IsShunjuen)
                {
                    IsRegistrationPrecedingYearFinalAccountVisiblity =
                        DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today,IsShunjuen) == 0;
                }
                else
                {
                    ObservableCollection<CreditDept> list =
                        DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
                    int amount = default;

                    foreach (CreditDept cd in list)
                    { amount += DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today, cd); }
                    IsRegistrationPrecedingYearFinalAccountVisiblity = amount == 0;
                }
                IsUpdatePrecedingYearFinalAccountVisibility =
                    !IsRegistrationPrecedingYearFinalAccountVisiblity;
            }
        }
        /// <summary>
        /// 前年度決算を登録します
        /// </summary>
        public DelegateCommand RegistrationPrecedingYearFinalAccountCommand { get; set; }
        private void RegistrationPrecedingYearFinalAccount()
        {
            CreditDept creditDept = null;

            if (IsShunjuen) { ShunjuenRegistration(); }
            else { WizeCoreOperation(); }

            void WizeCoreOperation()
            {
                bool IsNotExistsData = false;
                ObservableCollection<CreditDept> list = DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);

                foreach (CreditDept cd in list)
                {
                    if (DataBaseConnect.CallPrecedingYearFinalAccount(DateTime.Today, cd) != 0) { continue; }
                    creditDept = cd;
                    if (CallPreviousPerMonthFinalAccountRegisterInfo(cd.Dept) == MessageBoxResult.Yes)
                    { Registration(cd); }
                    else { IsNotExistsData = true; }
                }
                IsRegistrationPrecedingYearFinalAccountVisiblity = IsNotExistsData;
            }

            void ShunjuenRegistration()
            {
                if (CallPreviousPerMonthFinalAccountRegisterInfo(SHUNJUEN) == MessageBoxResult.Yes)
                {
                    _ = DataBaseConnect.RegistrationPrecedingYearFinalAccount();
                    //前年度決算が登録されたので、登録ボタンを隠す
                    IsRegistrationPrecedingYearFinalAccountVisiblity = false;
                    IsUpdatePrecedingYearFinalAccountVisibility = true;
                }
            }

            void Registration(CreditDept creditDept)
            { _ = DataBaseConnect.RegistrationPrecedingYearFinalAccount(creditDept); }

            MessageBoxResult CallPreviousPerMonthFinalAccountRegisterInfo(string accountingDept)
            {
                string amount = IsShunjuen ? AmountWithUnit(DataBaseConnect.PreviousDayFinalAmount(true)) :
                    AmountWithUnit(DataBaseConnect.PreviousDayFinalAmount(creditDept));

                if (IntAmount(amount) == 0) 
                {
                    amount = AccountingProcessLocation.IsAccountingGenreShunjuen ?
                        AmountWithUnit(DataBaseConnect.RetutnFiscalYearEndFinalAccountCalculation
                        (CurrentFiscalYearFirstDate.AddDays(-1))) :
                        AmountWithUnit(DataBaseConnect.RetutnFiscalYearEndFinalAccountCalculation
                        (CurrentFiscalYearFirstDate.AddDays(-1), creditDept));
                }

                if(IntAmount(amount) == 0) { return MessageBoxResult.No; }

                MessageBox = new MessageBoxInfo()
                {
                    Message =
                    $"{accountingDept}前年度決算額{Space}{amount}を登録します。\r\n\r\nよろしいですか？",
                    Image = MessageBoxImage.Question,
                    Title = "登録確認",
                    Button = MessageBoxButton.YesNo
                };
                return MessageBox.Result;
            }
        }
        /// <summary>
        /// ログインしていないことを案内します
        /// </summary>
        private void CallNoLoginMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "ログインしてください。\r\nログインしていない場合は金種金額計算のみ使用できます。",
                Button = MessageBoxButton.OK,
                Image = MessageBoxImage.Warning,
                Title = "担当者データがありません"
            };
        }
        /// <summary>
        /// ログインしていて、なおかつ経理担当場所が管理事務所か、
        /// 青蓮堂の場合は預り金を設定している場合にTrueを返します
        /// </summary>
        /// <returns></returns>
        public bool SetOperationButtonEnabled()
        {
            ShowSlipManagementContent = "出納管理";
            if (!ReturnIsRepLogin()) { return false; }

            if (AccountingProcessLocation.Location == Locations.管理事務所) { return true; }

            bool b = !string.IsNullOrEmpty(DepositAmount);
            if (!b)
            {
                CallDepositAmountEmptyMessage();
                ShowSlipManagementContent = "預かり金額を入力して下さい";
            }
            return b;
        }
        /// <summary>
        /// 春秋苑会計の経理担当場所が青蓮堂で、預り金のテキストボックスの値が0だった時に警告します
        /// </summary>
        private void CallDepositAmountEmptyMessage()
        {
            if(AccountingProcessLocation.Location== Locations.管理事務所) { return; }
            if (!IsShunjuen) { return; }
            if (IntAmount(DepositAmount) != 0) { return; }

            MessageBox = new MessageBoxInfo()
            {
                Message =
                    "経理担当場所が青蓮堂の場合は、テキストボックスに預かった金庫の金額を入力してください。",
                Image = MessageBoxImage.Warning,
                Title = "金額未入力",
                Button = MessageBoxButton.OK
            };
            CallPropertyChanged(nameof(MessageBox));
        }
        /// <summary>
        /// ログインしているかを判定します
        /// </summary>
        /// <returns>判定結果</returns>
        private bool ReturnIsRepLogin()
        {
            bool b = LoginRep.GetInstance().Rep.ID != string.Empty;
            return b;
        }
        /// <summary>
        /// 経理システムの終了確認のメッセージボックスを設定します
        /// </summary>
        private void ClosingMessage()
        {
            string NoOutputed =
                AccountingProcessLocation.IsCashBoxOutputed ? string.Empty : "金種表";

            if (string.IsNullOrEmpty(NoOutputed))
            { NoOutputed = AccountingProcessLocation.IsBalanceAccountOutputed ? string.Empty : "収支日報"; }
            else
            { NoOutputed += AccountingProcessLocation.IsBalanceAccountOutputed ? string.Empty : "、収支日報"; }

            if (AccountingProcessLocation.Location != Locations.管理事務所)
            { NoOutputed = string.Empty; }

            NoOutputed = string.IsNullOrEmpty(NoOutputed) ? 
                string.Empty : $"\r\n\r\n\t※{NoOutputed}が出力されていません";

            MessageBox = new MessageBoxInfo()
            {
                Message = $"終了します。よろしいですか？{NoOutputed}",
                Button = MessageBoxButton.YesNo,
                Title = "経理システムの終了",
                Image = MessageBoxImage.Question
            };
        }
        /// <summary>
        /// 経理システムの終了確認メッセージボックスを呼び出します
        /// </summary>
        public bool CallClosingMessage
        {
            get => callClosingMessage;
            set
            {
                if (callClosingMessage == value) { return; }
                callClosingMessage = value;
                CallPropertyChanged();
                callClosingMessage = false;
            }
        }
        /// <summary>
        /// 処理機能コントロールのEnabled
        /// </summary>
        public bool ProcessFeatureEnabled
        {
            get => processFeatureEnabled;
            set
            {
                processFeatureEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 管理事務所チェック欄
        /// </summary>
        public bool KanriJimushoChecked
        {
            get => kanriJimushoChecked;
            set
            {
                kanriJimushoChecked = value;
                if (value) { AccountingProcessLocation.SetLocation(Locations.管理事務所); }
                ValidationProperty(nameof(KanriJimushoChecked), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 青蓮堂チェック欄
        /// </summary>
        public bool ShorendoChecked
        {
            get => shorendoChecked;
            set
            {
                shorendoChecked = value;
                if (value) { AccountingProcessLocation.SetLocation(Locations.青蓮堂); }
                ValidationProperty(nameof(ShorendoChecked), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票管理ボタンEnable
        /// </summary>
        public bool IsSlipManagementEnabled
        {
            get => isSlipManagementEnabled;
            set
            {
                isSlipManagementEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 預り金額（管理事務所なら前日残高、青蓮堂なら預り金）
        /// </summary>
        public string DepositAmount
        {
            get => depositAmount;
            set
            {
                AccountingProcessLocation.OriginalTotalAmount = IntAmount(value);
                if (LoginRep.GetInstance().Rep.Name != string.Empty)
                {
                    IsSlipManagementEnabled = IsCreateVoucherEnabled =
                        AccountingProcessLocation.OriginalTotalAmount != 0;
                }

                if (ShorendoChecked)
                {
                    SetSlipContent();

                    IsShunjuenCommonEnabled = AccountingProcessLocation.OriginalTotalAmount != 0;
                }
                SetMenuEnabled();
                depositAmount = CommaDelimitedAmount(value);
                ValidationProperty(nameof(DepositAmount), value);
                CallPropertyChanged();

                void SetSlipContent()
                {
                    if (AccountingProcessLocation.IsAccountingGenreShunjuen)
                    {
                        ShowSlipManagementContent =
                            AccountingProcessLocation.OriginalTotalAmount == 0 ?
                                "預かり金額を設定して下さい" : "出納管理";
                    }
                    else { ShowSlipManagementContent = "出納管理"; }
                }
            }
        }
        /// <summary>
        /// 前月決算登録ボタンのVisiblity
        /// </summary>
        public bool IsRegistrationPrecedingYearFinalAccountVisiblity
        {
            get => isRegistrationPrecedingYearFinalAccountVisiblity;
            set
            {
                isRegistrationPrecedingYearFinalAccountVisiblity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ログアウトボタンのenabled
        /// </summary>
        public bool IsLogoutEnabled
        {
            get => isLogoutEnabled;
            set
            {
                isLogoutEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納間画面表示ボタンのContent
        /// </summary>
        public string ShowSlipManagementContent
        {
            get => showSlipManagementContent;
            set
            {
                showSlipManagementContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証出力画面表示ボタンのEnabled
        /// </summary>
        public bool IsCreateVoucherEnabled
        {
            get => isCreateVoucherEnabled;
            set
            {
                isCreateVoucherEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パート交通費データ登録画面表示ボタンのEnabled
        /// </summary>
        public bool IsPartTransportRegistrationEnabled
        {
            get => isPartTransportRegistrationEnabled;
            set
            {
                isPartTransportRegistrationEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 春秋苑会計かのチェック
        /// </summary>
        public bool IsShunjuen
        {
            get => isShunjuen;
            set
            {
                isShunjuen = value;
                AccountingProcessLocation.IsAccountingGenreShunjuen = value;
                IsWizeCoreMenuVisibility = !value;
                IsCreditDeptVisibility =
                    !value && IsUpdatePrecedingYearFinalAccountVisibility &&
                    LoginRep.GetInstance().Rep.IsAdminPermisson;

                if (KanriJimushoChecked)
                {
                    SetPreviousDayFinalAmount();
  
                    SetLocationKanriJimusho();
                }
                else if (ShorendoChecked)
                { SetLocationShorendo(); }

                CallPropertyChanged();

                SetWindowDefaultTitle();
                string s = string.IsNullOrEmpty(LoginRep.GetInstance().Rep.FirstName) ? string.Empty : 
                    $"（ログイン：{LoginRep.GetInstance().Rep.FirstName}）";
                WindowTitle = $"{DefaultWindowTitle}{s}";

                if (!string.IsNullOrEmpty(LoginRep.GetInstance().Rep.Name)) { CallConfirmationPrecedingYearFinalAccount(); }

                void CallConfirmationPrecedingYearFinalAccount()
                {
                    if (IsPrecedingYearFinalAccountRegisterVerified) { IsPrecedingYearFinalAccountRegisterVerified = false; }
                    else
                    {
                        ConfirmationPrecedingYearFinalAccount();
                        IsPrecedingYearFinalAccountRegisterVerified = true;
                    }
                }

                void SetPreviousDayFinalAmount()
                {
                    if(IsShunjuen)
                    {
                        AccountingProcessLocation.OriginalTotalAmount =
                            DataBaseConnect.PreviousDayFinalAmount(IsShunjuen);
                    }
                    else
                    {
                        ObservableCollection<CreditDept> creditDepts =
                            DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
                        int amount = 0;

                        foreach (CreditDept creditDept in creditDepts)
                        {
                            amount+=DataBaseConnect.PreviousDayFinalAmount(creditDept);
                        }

                        AccountingProcessLocation.OriginalTotalAmount = amount;
                    }
                }
            }
        }
        /// <summary>
        /// 春秋苑独自のメニューのEnabled
        /// </summary>
        public bool IsShunjuenMenuEnabled
        {
            get => isShunjuenMenuEnabled;
            set
            {
                isShunjuenMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ワイズコア独自のメニューのEnabled
        /// </summary>
        public bool IsWizeCoreMenuEnabled
        {
            get => isWizeCoreMenuEnabled;
            set
            {
                isWizeCoreMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御布施一覧のEnabled
        /// </summary>
        public bool IsShunjuenCommonEnabled
        {
            get => isShunjuenCommonEnabled;
            set
            {
                isShunjuenCommonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前年度決算更新ボタンのvisibility
        /// </summary>
        public bool IsUpdatePrecedingYearFinalAccountVisibility
        {
            get => isUpdatePrecedingYearFinalAccountVisibility;
            set
            {
                isUpdatePrecedingYearFinalAccountVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ワイズコアの貸方部門選択メニューのvisibility
        /// </summary>
        public bool IsWizeCoreMenuVisibility
        {
            get => isWizeCoreMenuVisibility;
            set
            {
                isWizeCoreMenuVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門項目のVisibility
        /// </summary>
        public bool IsCreditDeptVisibility
        {
            get => isCreditDeptVisibility;
            set
            {
                isCreditDeptVisibility = value;
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
        /// 共通処理メニューのEnabled
        /// </summary>
        public bool IsCommonEnabled
        {
            get => isCommonEnabled;
            set
            {
                isCommonEnabled = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 経理担当場所を管理事務所に設定します
        /// </summary>
        private void SetLocationKanriJimusho()
        {
            KanriJimushoChecked = true;
            ProcessFeatureEnabled = true;
            if (IsShunjuen)
            {
                IsShunjuenMenuEnabled = true;
                IsWizeCoreMenuEnabled = false;
                IsShunjuenCommonEnabled = true;
            }
            else
            {
                IsShunjuenMenuEnabled = false;
                IsWizeCoreMenuEnabled = true;
            }

            if (IsPrecedingYearFinalAccountRegisterVerified) { IsPrecedingYearFinalAccountRegisterVerified = false; }
            else 
            {
                ConfirmationPrecedingYearFinalAccount();
                IsPrecedingYearFinalAccountRegisterVerified = true;
            }

            AccountingProcessLocation.OriginalTotalAmount =
                DataBaseConnect.PreviousDayFinalAmount(IsShunjuen);

            DepositAmount =
                CommaDelimitedAmount(AccountingProcessLocation.OriginalTotalAmount);
            IsSlipManagementEnabled = IsPartTransportRegistrationEnabled =
                IsCreateVoucherEnabled = IsShunjuenCommonEnabled =
                LoginRep.GetInstance().Rep.Name != string.Empty;
            ShowSlipManagementContent = "出納管理";
        }

        private void SetMenuEnabled()
        {
            IsShunjuenMenuEnabled = IsShunjuen;
            IsWizeCoreMenuEnabled = !IsShunjuen;

            if (string.IsNullOrEmpty(LoginRep.GetInstance().Rep.Name))
            {
                IsShunjuenMenuEnabled = false;
                IsWizeCoreMenuEnabled = false;
                IsCommonEnabled = false;
            }
            else
            { SetIsCommonEnabled(); }

            void SetIsCommonEnabled()
            {
                if (AccountingProcessLocation.IsAccountingGenreShunjuen)
                {
                    IsCommonEnabled =
                        (ShorendoChecked && IntAmount(DepositAmount) != 0) || KanriJimushoChecked;
                }
                else
                {
                    IsCommonEnabled = true;
                    IsSlipManagementEnabled = true;
                    IsCreateVoucherEnabled = true;
                }
            }
        }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定します
        /// </summary>
        private void SetLocationShorendo()
        {
            ShorendoChecked = true;
            ProcessFeatureEnabled = true;
            IsPartTransportRegistrationEnabled = false;
            SetMenuEnabled();
            AccountingProcessLocation.OriginalTotalAmount = 0;
            DepositAmount = AccountingProcessLocation.OriginalTotalAmount.ToString();
            if (AccountingProcessLocation.OriginalTotalAmount == 0) { SetControleProperty(); }
            else
            {
                IsShunjuenMenuEnabled = IsShunjuen;
                IsWizeCoreMenuEnabled = !IsShunjuen;
            }

            void SetControleProperty()
            {
                if (AccountingProcessLocation.IsAccountingGenreShunjuen)
                {
                    ShowSlipManagementContent = "預かり金額を設定して下さい";
                    IsShunjuenMenuEnabled = false;
                    IsWizeCoreMenuEnabled = false;
                }
            }
        }
        /// <summary>
        /// 画面を閉じるメソッドを使用するかのチェック
        /// </summary>
        /// <returns>YesNo</returns>
        public bool CancelClose()
        {
            CallClosingMessage = true;
            bool b = MessageBox.Result != MessageBoxResult.Yes;
            if (!b)
            {
                WavSoundPlayCommand.Play("Shutdown.wav");
                //wav再生時間の取得方法を模索すること。21.9.19時点でいいのがなかった
                Thread.Sleep(2700);
            }
            return b;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(KanriJimushoChecked):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                case nameof(ShorendoChecked):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                case nameof(DepositAmount):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 経理担当場所のエラーを判定します
        /// </summary>
        private void SetLocationErrorsListOperation(string propertyName)
        {
            ErrorsListOperation
                (KanriJimushoChecked == false & ShorendoChecked == false, propertyName,
                    "経理担当場所を設定して下さい");
            if (GetErrors(propertyName) == null)
            {
                ErrorsListOperation
                    (shorendoChecked &
                    (string.IsNullOrEmpty(DepositAmount) || IntAmount(DepositAmount) == 0),
                    nameof(DepositAmount), "金額を入力してください"); 
            }
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = $"春秋苑経理システム：{(IsShunjuen ? "春秋苑会計" : "ワイズコア会計")}"; }

        public override void SetRep(Rep rep)
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name))
                {
                    WindowTitle = DefaultWindowTitle;
                    IsAdminPermisson = false;
                    _ = SetOperationButtonEnabled();
                }
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle = $"{DefaultWindowTitle}（ログイン : {rep.FirstName}）";
                    IsLogoutEnabled = true;
                }
            }
        }

        public void OriginalTotalAmoutNotify()
        { DepositAmount = AmountWithUnit(AccountingProcessLocation.OriginalTotalAmount); }
    }
}