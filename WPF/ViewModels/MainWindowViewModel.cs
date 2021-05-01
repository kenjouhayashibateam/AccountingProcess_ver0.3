using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// メインウィンドウ
    /// </summary>
    public class MainWindowViewModel : BaseViewModel, IClosing
    {
        #region Properties
        private bool callClosingMessage;
        private bool processFeatureEnabled;
        private bool shorendoChecked;
        private bool kanriJimushoChecked;
        private bool isSlipManagementEnabled;
        private bool isDepositMenuEnabled;
        private bool isRegistrationPerMonthFinalAccountVisiblity;
        private bool isLogoutEnabled;
        private bool isCreateVoucherEnabled;
        private bool isPartTransportRegistrationEnabled;
        private readonly LoginRep LoginRep = LoginRep.GetInstance();
        private string depositAmount;
        private string depositAmountInfo;
        private string showSlipManagementContent;
        #endregion

        public enum Locations
        {
            管理事務所,
            青蓮堂
        }
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
        /// コンストラクタ　DelegateCommand、LoginRepのインスタンスを生成します
        /// </summary>
        public MainWindowViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
        {
            LoginRep.SetRep(new Rep(string.Empty, string.Empty, string.Empty, false, false));

            ShowRemainingMoneyCalculationCommand =
                new DelegateCommand(() => SetShowRemainingMoneyCalculationView(), () => true);
            MessageBoxCommand =
                new DelegateCommand(() => ClosingMessage(), () => true);
            SetLocationKanriJimushoCommand =
                new DelegateCommand(() => SetLocationKanriJimusho(), () => true);
            SetLodationShorendoCommand =
                new DelegateCommand(() => SetLocationShorendo(), () => true);
            ShowDataManagementCommand =
                new DelegateCommand(() => SetShowDataManagementView(), () => true);
            ShowLoginCommand =
                new DelegateCommand(() => SetShowLoginView(), () => true);
            ShowReceiptsAndExpenditureManagementCommand =
                new DelegateCommand(() => SetShowReceiptsAndExpenditureManagementView(), 
                    () =>SetOperationButtonEnabled());
            ShowCreateVoucherCommand =
                new DelegateCommand(() => SetShowCreateVoucherView(), () => true);
            RegistrationPerMonthFinalAccountCommand =
                new DelegateCommand(() => RegistrationPerMonthFinalAccount(), () => true);
            LogoutCommand =
                new DelegateCommand(() => Logout(), () => true);
            ShowPartTimerTransPortCommand =
                new DelegateCommand(() => SetShowPartTimerTransportRegistrationView(), () => true);
        }
        public MainWindowViewModel():this(DefaultInfrastructure.GetDefaultDataBaseConnect()){}
        /// <summary>
        /// ログアウトコマンド
        /// </summary>
        public DelegateCommand LogoutCommand { get; set; }
        private void Logout()
        {
            LoginRep.SetRep(new Rep(string.Empty, string.Empty, string.Empty, false, false));
            IsSlipManagementEnabled = false;
            IsCreateVoucherEnabled = false;
            IsPartTransportRegistrationEnabled = false;
            ShowSlipManagementContent = "出納管理";
            IsLogoutEnabled = false;
        }
        /// <summary>
        /// 前月決算額を確認したうえで登録します
        /// </summary>
        private void ConfirmationPerMonthFinalAccount()
        {
            //今日の日付が1日なら登録ボタンを可視化する
            IsRegistrationPerMonthFinalAccountVisiblity = DateTime.Today.Day == 1;
            if(string.IsNullOrEmpty(LoginRep.Rep.Name))
            {
                CallNoLoginMessage();
                IsSlipManagementEnabled = false;
                ShowSlipManagementContent = "出納管理";
                return;
            }
            //前月決算が登録されていれば登録ボタンを隠す
            if (IsRegistrationPerMonthFinalAccountVisiblity) 
                IsRegistrationPerMonthFinalAccountVisiblity = DataBaseConnect.CallFinalAccountPerMonth() == 0;
            //登録ボタンが可視化されていなければ処理を終了する
            if (!IsRegistrationPerMonthFinalAccountVisiblity) return;
            
            RegistrationPerMonthFinalAccount();
        }
        private MessageBoxResult CallPreviousPerMonthFinalAccountRegisterInfo()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message =
                    $"前月決算額 {TextHelper.AmountWithUnit(DataBaseConnect.PreviousDayFinalAmount())} " +
                    $"を登録します。\r\n\r\nよろしいですか？",
                Image = MessageBoxImage.Question,
                Title = "登録確認",
                Button = MessageBoxButton.YesNo
            };
            CallPropertyChanged(nameof(MessageBox));
            return MessageBox.Result;
        }

        /// <summary>
        /// 前月決算を登録します
        /// </summary>
        public DelegateCommand RegistrationPerMonthFinalAccountCommand { get; set; }
        private void RegistrationPerMonthFinalAccount()
        {
            if (CallPreviousPerMonthFinalAccountRegisterInfo() == MessageBoxResult.No) return;
            DataBaseConnect.RegistrationPerMonthFinalAccount();
            IsRegistrationPerMonthFinalAccountVisiblity = false;//前月決算が登録されたので、登録ボタンを隠す
        }
        /// <summary>
        /// ログインしていないことを案内します
        /// </summary>
        private void CallNoLoginMessage() =>
            MessageBox = new MessageBoxInfo() 
                { 
                    Message = "ログインしてください", 
                    Button = MessageBoxButton.OK, 
                    Image = MessageBoxImage.Warning, 
                    Title = "担当者データがありません" 
                };
        /// <summary>
        /// ログインしていて、なおかつ経理担当場所が管理事務所か、
        /// 青蓮堂の場合は預り金を設定している場合にTrueを返します
        /// </summary>
        /// <returns></returns>
        public bool SetOperationButtonEnabled()
        {
            ShowSlipManagementContent = "出納管理";
            if (!ReturnIsRepLogin()) return false;
            if (AccountingProcessLocation.Location == Locations.管理事務所.ToString()) return true;
            bool b=!string.IsNullOrEmpty(DepositAmount);
            if (!b)
            {
                CallDepositAmountEmptyMessage();
                ShowSlipManagementContent = "預かり金額を入力して下さい";
            }

            return b;
        }
        /// <summary>
        /// 経理担当場所が青蓮堂で、預り金のテキストボックスの値が0だった時に警告します
        /// </summary>
        private void CallDepositAmountEmptyMessage()
        {    
            if (TextHelper.IntAmount(DepositAmount)!=0) return;
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
            bool b = LoginRep.Rep.ID != string.Empty;
            return b;
        }
        private void SetShowPartTimerTransportRegistrationView() =>
            CreateShowWindowCommand(ScreenTransition.PartTimerTransportRegistration());
        /// <summary>
        /// 受納証発行画面を表示します
        /// </summary>
        private void SetShowCreateVoucherView() => 
            CreateShowWindowCommand(ScreenTransition.CreateVoucher());
        /// <summary>
        /// 伝票管理画面を表示します
        /// </summary>
        private void SetShowReceiptsAndExpenditureManagementView() =>
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureMangement());
        /// <summary>
        /// ログイン画面を表示します
        /// </summary>
        private void SetShowLoginView() => CreateShowWindowCommand(ScreenTransition.Login());
        /// <summary>
        /// データ管理画面を表示します
        /// </summary>
        private void SetShowDataManagementView() => 
            CreateShowWindowCommand(ScreenTransition.DataManagement());
        /// <summary>
        /// 金庫金額計算画面を表示します
        /// </summary>
        private void SetShowRemainingMoneyCalculationView() =>
            CreateShowWindowCommand(ScreenTransition.RemainingMoneyCalculation());
        /// <summary>
        /// 経理システムの終了確認のメッセージボックスを設定します
        /// </summary>
        private void ClosingMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "終了します。よろしいですか？",
                Button = System.Windows.MessageBoxButton.YesNo,
                Title = "経理システムの終了",
                Image = System.Windows.MessageBoxImage.Question
            };

            CallPropertyChanged(nameof(MessageBox));
        }
        /// <summary>
        /// 経理システムの終了確認メッセージボックスを呼び出します
        /// </summary>
        public bool CallClosingMessage
        {
            get => callClosingMessage;
            set
            {
                if (callClosingMessage == value) return;
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
                if (value) AccountingProcessLocation.SetLocation(Locations.管理事務所.ToString());
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
                if (value) AccountingProcessLocation.SetLocation(Locations.青蓮堂.ToString());
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
                AccountingProcessLocation.OriginalTotalAmount = TextHelper.IntAmount(value);
                if (LoginRep.Rep.Name != string.Empty) IsSlipManagementEnabled = IsCreateVoucherEnabled =
                        AccountingProcessLocation.OriginalTotalAmount != 0;
                
                if (ShorendoChecked) ShowSlipManagementContent =
                        AccountingProcessLocation.OriginalTotalAmount == 0 ?
                            "預かり金額を設定して下さい" : "出納管理";
                depositAmount = TextHelper.CommaDelimitedAmount(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 預り金メニューのEnabled
        /// </summary>
        public bool IsDepositMenuEnabled
        {
            get => isDepositMenuEnabled;
            set
            {
                isDepositMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 預り金欄の案内文字列
        /// </summary>
        public string DepositAmountInfo
        {
            get => depositAmountInfo;
            set
            {
                depositAmountInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前月決算登録ボタンのVisiblity
        /// </summary>
        public bool IsRegistrationPerMonthFinalAccountVisiblity
        {
            get => isRegistrationPerMonthFinalAccountVisiblity;
            set
            {
                isRegistrationPerMonthFinalAccountVisiblity = value;
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
        /// 経理担当場所を管理事務所に設定します
        /// </summary>
        private void SetLocationKanriJimusho()
        {
            KanriJimushoChecked = true;
            ProcessFeatureEnabled = true;
            IsDepositMenuEnabled = false;
            ConfirmationPerMonthFinalAccount();
            AccountingProcessLocation.OriginalTotalAmount = DataBaseConnect.PreviousDayFinalAmount();
            DepositAmountInfo = "前日決算金額";
            DepositAmount =
                TextHelper.CommaDelimitedAmount(AccountingProcessLocation.OriginalTotalAmount);
            IsSlipManagementEnabled = IsPartTransportRegistrationEnabled = IsCreateVoucherEnabled =
                LoginRep.Rep.Name != string.Empty;
            ShowSlipManagementContent = "出納管理";
        }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定します
        /// </summary>
        private void SetLocationShorendo()
        {
            ShorendoChecked = true;
            IsDepositMenuEnabled = true;
            ProcessFeatureEnabled = true;
            IsPartTransportRegistrationEnabled = false;
            AccountingProcessLocation.OriginalTotalAmount = 0;
            DepositAmountInfo = "預かった金庫の金額を入力してください";
            DepositAmount = AccountingProcessLocation.OriginalTotalAmount.ToString();
            if(AccountingProcessLocation.OriginalTotalAmount==0) ShowSlipManagementContent=
                    "預かり金額を設定して下さい";
        }
        /// <summary>
        /// 画面を閉じるメソッドを使用するかのチェック
        /// </summary>
        /// <returns>YesNo</returns>
        public bool OnClosing()
        {
            CallClosingMessage = true;
            return MessageBox.Result != MessageBoxResult.Yes;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(kanriJimushoChecked):
                    SetLocationErrorsListOperation(propertyName);
                    break;
                case nameof(shorendoChecked):
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
                ErrorsListOperation(shorendoChecked == true & string.IsNullOrEmpty(DepositAmount), 
                    propertyName, "金額を入力してください");
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = "春秋苑経理システム";

        public override void SetRep(Rep rep)
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name))
                {
                    WindowTitle = DefaultWindowTitle;
                    IsAdminPermisson = false;
                    SetOperationButtonEnabled();
                }
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle = $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
                    IsLogoutEnabled = true;
                }
            }
        }
    }
}