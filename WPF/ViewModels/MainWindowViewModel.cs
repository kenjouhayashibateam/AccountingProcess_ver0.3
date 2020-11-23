﻿using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
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
        private readonly ScreenTransition screenTransition = new ScreenTransition();
        private bool shorendoChecked;
        private bool kanriJimushoChecked;
        private readonly LoginRep LoginRep = LoginRep.GetInstance();
        private bool isSlipManagementEnabled;
        private readonly IDataBaseConnect DataBaseConnect;
        private string depositAmount;
        private bool isDepositMenuEnabled;
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
        /// コンストラクタ　DelegateCommand、LoginRepのインスタンスを生成します
        /// </summary>
        public MainWindowViewModel(IDataBaseConnect dataBaseConnect)
        {
            LoginRep.SetRep(new Rep(string.Empty, string.Empty, string.Empty, false, false));
            DataBaseConnect = dataBaseConnect;

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
                new DelegateCommand(() => SetShowReceiptsAndExpenditureManagementView(), () =>SetOperationButtonEnabled());
        }
        public MainWindowViewModel():this(DefaultInfrastructure.GetDefaultDataBaseConnect()){}

        /// <summary>
        /// ログインしていないことを案内します
        /// </summary>
        private void CallNoLoginMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "ログインしてください",
                Button = MessageBoxButton.OK,
                Image = MessageBoxImage.Warning,
                Title = "担当者データがありません"
            };
            CallPropertyChanged(nameof(MessageBox));
        }
        /// <summary>
        /// ログインしていて、なおかつ経理担当場所が管理事務所か、青蓮堂の場合は預り金を設定している場合にTrueを返します
        /// </summary>
        /// <returns></returns>
        public bool SetOperationButtonEnabled()
        {
            if (!ReturnIsRepLogin()) return false;
            bool b=!string.IsNullOrEmpty(DepositAmount);
            if (AccountingProcessLocation.Location == Locations.管理事務所.ToString()) return true;
            if (!b) CallDepositAmountEmptyMessage();
            return b;
        }
        /// <summary>
        /// 経理担当場所が青蓮堂で、預り金のテキストボックスが空白だった時に警告します
        /// </summary>
        private void CallDepositAmountEmptyMessage()
        {
                MessageBox = new MessageBoxInfo()
                {
                    Message = "経理担当場所が青蓮堂の場合は、テキストボックスに預かった金庫の金額を入力してください。",
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
            if(!b)CallNoLoginMessage();
            return b;
        }
        /// <summary>
        /// 伝票管理画面を表示します
        /// </summary>
        private void SetShowReceiptsAndExpenditureManagementView()
        {
            CreateShowWindowCommand(screenTransition.SlipManagement());
            CallPropertyChanged();
        }
        /// <summary>
        /// ログイン画面を表示します
        /// </summary>
        private void SetShowLoginView()
        {
            CreateShowWindowCommand(screenTransition.Login());
            CallPropertyChanged();
        }
        /// <summary>
        /// データ管理画面を表示します
        /// </summary>
        private void SetShowDataManagementView()
        {
            CreateShowWindowCommand(screenTransition.DataManagement());
            CallPropertyChanged();
        }
        /// <summary>
        /// 金庫金額計算画面を表示します
        /// </summary>
        private void SetShowRemainingMoneyCalculationView()
        {
            CreateShowWindowCommand(screenTransition.RemainingMoneyCalculation());
            CallPropertyChanged();
        }
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
        /// 預り金額
        /// </summary>
        public string DepositAmount
        {
            get => depositAmount;
            set
            {
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
        /// 経理担当場所を管理事務所に設定します
        /// </summary>
        private void SetLocationKanriJimusho()
        {
            KanriJimushoChecked = true;
            ProcessFeatureEnabled = true;
            IsDepositMenuEnabled = false;
            AccountingProcessLocation.SetOriginalTotalAmount(DataBaseConnect.PreviousDayBalance().Price);
        }
        /// <summary>
        /// 経理担当場所を青蓮堂に設定します
        /// </summary>
        private void SetLocationShorendo()
        {
            ShorendoChecked = true;
            IsDepositMenuEnabled = true;
            ProcessFeatureEnabled = true;
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
            ErrorsListOperation(KanriJimushoChecked == false & ShorendoChecked == false, propertyName, "経理担当場所を設定して下さい");
            if (GetErrors(propertyName) == null) ErrorsListOperation(shorendoChecked == true & string.IsNullOrEmpty(DepositAmount), propertyName, "金額を入力してください");
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = "春秋苑経理システム";
            return DefaultWindowTitle;
        }
    }
}