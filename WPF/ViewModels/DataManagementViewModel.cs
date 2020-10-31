using Domain.Entities.ValueObjects;
using WPF.ViewModels.Commands;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;
using System;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel : BaseViewModel
    {
        #region Properties
        #region RepProperties
        private string repIDField;
        private string repName;
        private string repCurrentPassword;
        private string repNewPassword;
        private bool isRepValidity;
        private bool isRepNameDataEnabled;
        private bool isRepPasswordEnabled;
        private bool isRepNewPasswordEnabled;
        private string repDataOperationButtonContent;
        private bool isCheckedRegistration;
        private bool isCheckedUpdate;
        private string referenceRep;
        private bool repValidity;
        private bool newPasswordCharCheck;
        private bool currentPasswordCharCheck;
        private string currentRepPasswordBorderBrush;
        private string currentRepPasswordBackground;
        private string newRepPasswordBorderBrush;
        private string newRepPasswordBackground;
        private readonly string TrueControlBorderBrush = "#000000";
        private readonly string FalseControlBorderBrush = "#FFABADB3";
        private readonly string TrueControlBackground = "#FFFFFF";
        private readonly string FalseControlBackground = "#A9A9A9";
        private bool isRepReferenceMenuEnabled;
        private Rep currentRep=new Rep(string.Empty,string.Empty,string.Empty,false);
        private ObservableCollection<Rep> repList;
        private bool isRepOperationButtonEnabled;
        #endregion
        private IDataBaseConnect DataBaseConnect;
        private DataOperation CurrentOperation;
        #endregion

        /// <summary>
        /// データ操作の判別
        /// </summary>
        private enum DataOperation
        {
            登録,
            更新
        }

        public DataManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()){}

        public DataManagementViewModel(IDataBaseConnect connecter)
        {
            DataBaseConnect = connecter;
            RepList = new ObservableCollection<Rep>();
            SetDataRegistrationCommand = new DelegateCommand(() => SetDataOperation(DataOperation.登録), () => true);
            SetDataUpdateCommand = new DelegateCommand(() => SetDataOperation(DataOperation.更新), () => true);
            RepNewPasswordCharCheckedReversCommand = new DelegateCommand(() => RepNewPasswordCharCheckedRevers(), () => true);
            RepCurrentPasswordCharCheckedReversCommand = new DelegateCommand(() => RepCurrentPasswordCharCheckedRevers(), () => true);
            RepRegistrationCommand = new DelegateCommand(() => RepRegistration(), () => IsRepRegistrable());
            SetDataOperation(DataOperation.登録);
            SetRepOperationButtonEnabled();
        }

        /// <summary>
        /// データ操作のジャンルを切り替え、操作ボタンの表示文字列を入力し、コントロールの値をクリアして、Enableを設定します
        /// </summary>
        /// <param name="Operation"></param>
        private void SetDataOperation(DataOperation Operation)
        {
            CurrentOperation = Operation;
            IsCheckedRegistration = (Operation == DataOperation.登録);
            IsCheckedUpdate = (Operation == DataOperation.更新);
            RepDataOperationButtonContent = Operation.ToString();
            SetDetailLocked();
        }

        /// <summary>
        /// データ操作のジャンルによって、各詳細のコントロールの値をクリア、Enableの設定をします
        /// </summary>
        private void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    SetFieldRegisterRep();
                    break;
                case DataOperation.更新:
                    SetFieldUpdaterRep();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// データ操作の登録Checked
        /// </summary>
        public bool IsCheckedRegistration
        {
            get => isCheckedRegistration;
            set
            {
                isCheckedRegistration = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作の更新Checked
        /// </summary>
        public bool IsCheckedUpdate
        {
            get => isCheckedUpdate;
            set
            {
                isCheckedUpdate = value;
                CallPropertyChanged();
            }
        }

        #region RepOperation
        /// <summary>
        /// 新しいパスワード入力欄の文字を隠すかの可否を反転させるコマンド
        /// </summary>
        public DelegateCommand RepNewPasswordCharCheckedReversCommand { get; }
        /// <summary>
        /// 現在のパスワード入力欄の文字を隠すかの可否を反転させるコマンド
        /// </summary>
        public DelegateCommand RepCurrentPasswordCharCheckedReversCommand { get; }
        /// <summary>
        /// データ操作を「登録」にするコマンド
        /// </summary>
        public DelegateCommand SetDataRegistrationCommand { get; }
        /// <summary>
        /// データ操作を「更新」にするコマンド
        /// </summary>
        public DelegateCommand SetDataUpdateCommand { get; }
        /// <summary>
        /// 新規担当者登録コマンド
        /// </summary>
        public DelegateCommand RepRegistrationCommand { get; }

        /// <summary>
        /// 担当者登録コマンドのCanExecuteを切り替えます
        /// </summary>
        /// <returns></returns>
        private bool IsRepRegistrable()
        {
            var repError = GetErrors(nameof(RepName));
            if(repError==null)repError = GetErrors(nameof(RepNewPassword));
            return repError == null;
        }

        /// <summary>
        /// 担当者登録
        /// </summary>
        private void RepRegistration()
        {
            RepList.Add(new Rep("aaa", "a a", "aaa", false));
            RepList.Add(new Rep(null, "c c", "ccc", true));
        }
        /// <summary>
        /// 新しいパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepNewPasswordCharCheckedRevers() => NewPasswordCharCheck = !NewPasswordCharCheck;
        /// <summary>
        /// 現在のパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepCurrentPasswordCharCheckedRevers() => CurrentPasswordCharCheck = !CurrentPasswordCharCheck;
        /// <summary>
        /// 担当者管理の更新のためのコントロールのEnableを設定し、フィールドをクリアします
        /// </summary>
        private void SetFieldUpdaterRep()
        {
            IsRepNameDataEnabled = false;
            IsRepPasswordEnabled = true;
            IsRepNewPasswordEnabled = false;
            IsRepReferenceMenuEnabled = true;
            RepDetailClear();
        }
        /// <summary>
        /// 担当者管理の登録のためのコントロールのEnableを設定し、フィールドをクリアします
        /// </summary>
        private void SetFieldRegisterRep()
        {
            IsRepReferenceMenuEnabled = false;
            IsRepNameDataEnabled = true;
            IsRepPasswordEnabled = false;
            IsRepNewPasswordEnabled = true;
            RepDetailClear();
        }
        /// <summary>
        /// 担当者管理のフィールドをクリアします
        /// </summary>
        private void RepDetailClear()
        {
            IsRepValidity = true;
            RepName = string.Empty;
            RepCurrentPassword = string.Empty;
            RepNewPassword = string.Empty;
            CurrentRep = new Rep(null, null, null, true);
        }
        /// <summary>
        /// 担当者ID
        /// </summary>
        public string RepIDField
        {
            get => repIDField;
            set
            {
                repIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者名
        /// </summary>
        public string RepName
        {
            get => repName;
            set
            {
                repName = value;
                ValidationProperty(nameof(RepName), value);
                SetRepOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 現在の担当者パスワード
        /// </summary>
        public string RepCurrentPassword
        {
            get => repCurrentPassword;
            set
            {
                repCurrentPassword = value;
                ValidationProperty(nameof(RepCurrentPassword), value);
                if (CurrentOperation == DataOperation.更新) IsRepNewPasswordEnabled = value == CurrentRep.Password;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しい担当者パスワード
        /// </summary>
        public string RepNewPassword
        {
            get => repNewPassword;
            set
            {
                repNewPassword = value;
                ValidationProperty(nameof(RepNewPassword), value);
                SetRepOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データの有効性
        /// </summary>
        public bool IsRepValidity
        {
            get => isRepValidity;
            set
            {
                isRepValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者名欄のEnable設定
        /// </summary>
        public bool IsRepNameDataEnabled
        {
            get => isRepNameDataEnabled;
            set
            {
                isRepNameDataEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者の現在のパスワード欄のEnable設定
        /// </summary>
        public bool IsRepPasswordEnabled
        {
            get => isRepPasswordEnabled;
            set
            {
                isRepPasswordEnabled = value;
                if (value)
                {
                    CurrentRepPasswordBorderBrush = TrueControlBorderBrush;
                    CurrentRepPasswordBackground = TrueControlBackground;
                }
                else
                {
                    CurrentRepPasswordBorderBrush = FalseControlBorderBrush;
                    CurrentRepPasswordBackground = FalseControlBackground;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データ操作ボタンのContent
        /// </summary>
        public string RepDataOperationButtonContent
        {
            get => repDataOperationButtonContent;
            set
            {
                repDataOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者検索文字列
        /// </summary>
        public string ReferenceRep
        {
            get => referenceRep;
            set
            {
                referenceRep = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データの有効性
        /// </summary>
        public bool RepValidity
        {
            get => repValidity;
            set
            {
                repValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄の文字を隠すかのチェック
        /// </summary>
        public bool NewPasswordCharCheck
        {
            get => newPasswordCharCheck;
            set
            {
                newPasswordCharCheck = value;
                Invoke(nameof(NewPasswordCharCheck));
            }
        }
        /// <summary>
        /// 現在のパスワードの文字を隠すかのチェック
        /// </summary>
        public bool CurrentPasswordCharCheck
        {
            get => currentPasswordCharCheck;
            set
            {
                currentPasswordCharCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄のEnabled
        /// </summary>
        public bool IsRepNewPasswordEnabled
        {
            get => isRepNewPasswordEnabled;
            set
            {
                isRepNewPasswordEnabled = value;
                if (value)
                {
                    NewRepPasswordBorderBrush = TrueControlBorderBrush;
                    NewRepPasswordBackground = TrueControlBackground;
                }
                else
                {
                    NewRepPasswordBorderBrush = FalseControlBorderBrush;
                    NewRepPasswordBackground = FalseControlBackground;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 現在のパスワード欄のボーダーの色
        /// </summary>
        public string CurrentRepPasswordBorderBrush
        {
            get => currentRepPasswordBorderBrush;
            set
            {
                currentRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 現在のパスワード欄の背景の色
        /// </summary>
        public string CurrentRepPasswordBackground
        {
            get => currentRepPasswordBackground;
            set
            {
                currentRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄のボーダーの色
        /// </summary>
        public string NewRepPasswordBorderBrush
        {
            get => newRepPasswordBorderBrush;
            set
            {
                newRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄の背景の色
        /// </summary>
        public string NewRepPasswordBackground
        {
            get => newRepPasswordBackground;
            set
            {
                newRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者検索メニューEnabled
        /// </summary>
        public bool IsRepReferenceMenuEnabled
        {
            get => isRepReferenceMenuEnabled;
            set
            {
                isRepReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者詳細に表示されている担当者
        /// </summary>
        public Rep CurrentRep
        {
            get => currentRep;
            set
            {
                currentRep = value;
                if (currentRep != null) SetRepDetailProperty();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者詳細に担当者クラスのプロパティを代入します
        /// </summary>
        private void SetRepDetailProperty()
        {
            RepIDField = CurrentRep.RepID;
            RepName = CurrentRep.Name;
            IsRepValidity = CurrentRep.IsValidity;
            RepCurrentPassword = string.Empty;
        }
        /// <summary>
        /// 担当者リスト
        /// </summary>
        public ObservableCollection<Rep> RepList
        {
            get => repList;
            set
            {
                repList = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データ操作ボタンEnabled
        /// </summary>
        public bool IsRepOperationButtonEnabled
        {
            get => isRepOperationButtonEnabled;
            set
            {
                isRepOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データ操作ボタンのEnabledを設定します
        /// </summary>
        private void SetRepOperationButtonEnabled()
        {
            if (!HasErrors)
            {
                IsRepOperationButtonEnabled = true;
                return;
            }

            switch(CurrentOperation)
            {
                case DataOperation.登録:
                    IsRepOperationButtonEnabled = !(string.IsNullOrEmpty(RepName) | string.IsNullOrEmpty(RepNewPassword));
                    break;

                default:
                    break;
            }
        }

        #endregion

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(RepName):
                    ErrorsListOperation(string.IsNullOrEmpty(RepName), propertyName, Properties.Resources.NullErrorInfo);
                    break;
                case nameof(RepCurrentPassword):
                    if (IsRepPasswordEnabled) ErrorsListOperation(RepCurrentPassword != CurrentRep.Password, propertyName, Properties.Resources.PasswordErrorInfo);
                    break;
                case nameof(RepNewPassword):
                    if(IsRepNewPasswordEnabled) ErrorsListOperation(string.IsNullOrEmpty(RepNewPassword), propertyName, Properties.Resources.NullErrorInfo);
                    break;

                default:
                    break;
            }
        }       
    }
}