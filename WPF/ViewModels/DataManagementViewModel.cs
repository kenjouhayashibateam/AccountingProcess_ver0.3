using Domain.Entities.ValueObjects;
using WPF.ViewModels.Commands;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;
using WPF.Views.Datas;
using System.Windows;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel : BaseViewModel
    {
        #region Properties
        #region RepProperties
        private string _repIDField;
        private string _repName;
        private string _repCurrentPassword;
        private string _repNewPassword;
        private string _confirmationPassword;
        private bool _isRepValidity;
        private bool _isRepNameDataEnabled;
        private bool _isRepPasswordEnabled;
        private bool _isRepNewPasswordEnabled;
        private string _repDataOperationButtonContent;
        private bool _isCheckedRegistration;
        private bool _isCheckedUpdate;
        private string _referenceRepName = string.Empty;
        private bool _repValidityTrueOnly;
        private bool _newPasswordCharCheck;
        private bool _currentPasswordCharCheck;
        private bool _confirmationPasswordCharCheck;
        private string _currentRepPasswordBorderBrush;
        private string _currentRepPasswordBackground;
        private string _newRepPasswordBorderBrush;
        private string _newRepPasswordBackground;
        private readonly string TrueControlBorderBrush = "#000000";
        private readonly string FalseControlBorderBrush = "#FFABADB3";
        private readonly string TrueControlBackground = "#FFFFFF";
        private readonly string FalseControlBackground = "#A9A9A9";
        private bool _isRepReferenceMenuEnabled;
        private Rep _currentRep = new Rep(string.Empty, string.Empty, string.Empty, false, false);
        private ObservableCollection<Rep> _repList;
        private bool _isRepOperationButtonEnabled;
        #endregion
        #region AccountingSubjectProperties
        private string accountingSubjectIDField;
        private bool isAccountingSubjectValidity;
        private string accountingSubjectCodeField;
        private string accountingSubjectField;
        private string accountingSubnectOperationButtonContent;
        private bool isAccountingSubjectOperationButtonEnabled;
        private AccountingSubject currentAccountingSubject;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        private bool isAccountingSubjectValidityTrueOnly;
        private string referenceAccountingSubjectCode;
        private string referenceAccountingSubject;
        private bool isAccountingSubjectReferenceMenuEnabled;
        private bool isAccountingSubjectCodeFieldEnabled;
        private bool isAccountingSubjectFieldEnabled;
        #endregion
        #region CreditAccountProperties
        private string creditAccountIDField;
        private bool isCreditAccountValidity;
        private string creditAccountField;
        #endregion
        private readonly IDataBaseConnect DataBaseConnect;
        private DataOperation CurrentOperation;
        readonly LoginRep LoginRep = LoginRep.GetInstance();
        #endregion

        /// <summary>
        /// データ操作の判別
        /// </summary>
        private enum DataOperation
        {
            登録,
            更新
        }
        public DataManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// コンストラクタ　DataBaseConnectを設定、DelegateCommandのインスタンスを生成します
        /// </summary>
        /// <param name="connecter">接続するデータベース</param>
        public DataManagementViewModel(IDataBaseConnect connecter)
        {
            DataBaseConnect = connecter;
            RepList = new ObservableCollection<Rep>();
            SetDataRegistrationCommand = new DelegateCommand(() => SetDataOperation(DataOperation.登録), () => true);
            SetDataUpdateCommand = new DelegateCommand(() => SetDataOperation(DataOperation.更新), () => true);
            SetDataOperation(DataOperation.登録);
            SetDelegateCommand();
        }
        /// <summary>
        /// 各デリゲートコマンドを生成します
        /// </summary>
        private void SetDelegateCommand()
        {
            SetRepDelegateCommand();
            AccountingSubjectDataOperationCommand = new DelegateCommand(() => AccountingSubjectDataOperation(), () => IsAccountingSubjectOperationButtonEnabled);
        }
        /// <summary>
        /// 更新の必要がないことをメッセージボックスで知らせます
        /// </summary>
        private void CallNoRequiredUpdateMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "更新の必要はありません",
                Image = MessageBoxImage.Exclamation,
                Title = "データ操作案内",
                Button = MessageBoxButton.OK
            };
            CallShowMessageBox = true;
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
            SetDataOperationButtonContent(Operation);
            if (CurrentOperation == DataOperation.更新) SetDataList();
            SetDetailLocked();
        }
        /// <summary>
        /// 各データのリストを生成します
        /// </summary>
        private void SetDataList()
        {
            RepList = DataBaseConnect.ReferenceRep(string.Empty, false);
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, false);
        }
        /// <summary>
        /// 各データ操作ボタンのContentを設定します
        /// </summary>
        private void SetDataOperationButtonContent(DataOperation operation)
        {
            RepDataOperationButtonContent = operation.ToString();
            AccountingSubnectOperationButtonContent = operation.ToString();
        }
        /// <summary>
        /// データ操作のジャンルによって、各詳細のコントロールの値をクリア、Enableの設定をします
        /// </summary>
        private void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    SetFieldEnabledRegisterRep();
                    SetFieldEnabledRegisterAccountingSubject();
                    break;
                case DataOperation.更新:
                    SetFieldEnabledUpdaterRep();
                    SetFieldEnabledUpdateAccountingSubject();
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
            get => _isCheckedRegistration;
            set
            {
                _isCheckedRegistration = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作の更新Checked
        /// </summary>
        public bool IsCheckedUpdate
        {
            get => _isCheckedUpdate;
            set
            {
                _isCheckedUpdate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録、更新確認メッセージボックスを生成、呼び出します
        /// </summary>
        /// <param name="confirmationMessage">確認内容（プロパティ等）</param>
        /// <param name="titleRegistrationCategory">タイトルに表示するクラス名</param>
        /// <returns>OKCancelを返します</returns>
        private MessageBoxResult CallConfirmationDataOperation(string confirmationMessage, string titleRegistrationCategory)
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = confirmationMessage,
                Title = $"{titleRegistrationCategory}データ操作確認",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question,
            };
            CallShowMessageBox = true;
            return MessageBox.Result;
        }
        /// <summary>
        /// データ操作を「登録」にするコマンド
        /// </summary>
        public DelegateCommand SetDataRegistrationCommand { get; set; }
        /// <summary>
        /// データ操作を「更新」にするコマンド
        /// </summary>
        public DelegateCommand SetDataUpdateCommand { get; set; }

        #region RepOperation

        /// <summary>
        /// 担当者データを操作します
        /// </summary>
        public void RepDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.更新:
                    RepUpdate();
                    break;
                case DataOperation.登録:
                    RepRegistration();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 担当者DelegateCommandのインスタンスを生成します
        /// </summary>
        private void SetRepDelegateCommand()
        {
            RepNewPasswordCharCheckedReversCommand = new DelegateCommand(() => RepNewPasswordCharCheckedRevers(), () => true);
            RepCurrentPasswordCharCheckedReversCommand = new DelegateCommand(() => RepCurrentPasswordCharCheckedRevers(), () => true);
            ConfirmationPasswordCheckedReversCommand = new DelegateCommand(() => ConfirmationPasswordCharCheckedRevers(), () => true);
            RepDataOperationCommand = new DelegateCommand(() => RepDataOperation(), () => IsRepDataOperationCanExecute());
        }
        /// <summary>
        /// 担当者データ操作コマンドのCanExecuteを設定します
        /// </summary>
        /// <returns></returns>
        private bool IsRepDataOperationCanExecute()
        {
            if (!IsAdminPermisson) return false;
            return CurrentOperation switch
            {
                DataOperation.更新 => IsRepUpdatable(),
                DataOperation.登録 => IsRepRegistrable(),
                _ => false,
            };
        }
        /// <summary>
        /// 新しいパスワード入力欄の文字を隠すかの可否を反転させるコマンド
        /// </summary>
        public DelegateCommand RepNewPasswordCharCheckedReversCommand { get; set; }
        /// <summary>
        /// 再入力パスワード入力欄の文字を隠すかの可否を反転させるコマンド
        /// </summary>
        public DelegateCommand ConfirmationPasswordCheckedReversCommand { get; set; }
        /// <summary>
        /// 現在のパスワード入力欄の文字を隠すかの可否を反転させるコマンド
        /// </summary>
        public DelegateCommand RepCurrentPasswordCharCheckedReversCommand { get; set; }
        /// <summary>
        /// 担当者データ操作コマンド
        /// </summary>
        public DelegateCommand RepDataOperationCommand { get; set; }
        /// <summary>
        /// 担当者更新コマンドのCanExecuteを切り替えます
        /// </summary>
        /// <returns>CanExecute</returns>
        private bool IsRepUpdatable() => IsRepOperationButtonEnabled;
        /// <summary>
        /// 担当者登録コマンドのCanExecuteを切り替えます
        /// </summary>
        /// <returns>CanExecute</returns>
        private bool IsRepRegistrable()
        {
            var repError = GetErrors(nameof(RepName));
            if (repError == null & !string.IsNullOrEmpty(RepNewPassword)) repError = GetErrors(nameof(RepNewPassword));
            if (repError == null & !string.IsNullOrEmpty(ConfirmationPassword)) repError = GetErrors(nameof(ConfirmationPassword));
            return repError == null;
        }
        /// <summary>
        /// 担当者を登録して、担当者一覧に加えます
        /// </summary>
        private async void RepRegistration()
        {
            CurrentRep = new Rep(null, RepName, RepNewPassword, IsRepValidity, false);
            if (CallConfirmationDataOperation
                ($"担当者名 : {CurrentRep.Name}\r\nパスワード : {new string('*', RepNewPassword.Length)}\r\n有効性 : {CurrentRep.IsValidity}\r\n\r\n登録しますか？",
                "担当者") == MessageBoxResult.Cancel)
                return;

            IsRepOperationButtonEnabled = false;
            RepDataOperationButtonContent = "登録中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => DataBaseConnect.Registration(CurrentRep, loginRep.Rep));
            IsRepOperationButtonEnabled = true;
            RepDataOperationButtonContent = "登録";

            RepDetailClear();
            RepList = DataBaseConnect.ReferenceRep(string.Empty, RepValidityTrueOnly);
        }
        /// <summary>
        /// 担当者データの変更、リストを最新データに更新します
        /// </summary>
        private async void RepUpdate()
        {
            string updateContents = string.Empty;

            if (CurrentRep.IsValidity != IsRepValidity)
            {
                updateContents += $"有効性 : {CurrentRep.IsValidity} → {IsRepValidity}\r\n";
                CurrentRep.IsValidity = IsRepValidity;
            }

            if (RepNewPassword.Length > 0)
            {
                updateContents += $"パスワード変更 : {new string('*', RepNewPassword.Length)}\r\n";
                CurrentRep.Password = RepNewPassword;
            }

            if (updateContents == string.Empty)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            updateContents = $"担当者 : {CurrentRep.Name}\r\n\r\n{updateContents}";
            if (CallConfirmationDataOperation($"{updateContents}\r\n\r\n更新します。よろしいですか？", "担当者") == MessageBoxResult.Cancel) return;

            IsRepOperationButtonEnabled = false;
            RepDataOperationButtonContent = "更新中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => DataBaseConnect.Update(CurrentRep, loginRep.Rep));
            IsRepOperationButtonEnabled = true;
            RepDataOperationButtonContent = "更新";

            RepDetailClear();
            RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, RepValidityTrueOnly);
        }
        /// <summary>
        /// 新しいパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepNewPasswordCharCheckedRevers() => NewPasswordCharCheck = !NewPasswordCharCheck;
        /// <summary>
        /// 再入力パスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void ConfirmationPasswordCharCheckedRevers() => ConfirmationPasswordCharCheck = !ConfirmationPasswordCharCheck;
        /// <summary>
        /// 現在のパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepCurrentPasswordCharCheckedRevers() => CurrentPasswordCharCheck = !CurrentPasswordCharCheck;
        /// <summary>
        /// 担当者管理の更新のためのコントロールのEnableを設定し、フィールドをクリアします
        /// </summary>
        private void SetFieldEnabledUpdaterRep()
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
        private void SetFieldEnabledRegisterRep()
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
            ConfirmationPassword = string.Empty;
            CurrentRep = new Rep(null, null, null, false, false);
        }
        /// <summary>
        /// 担当者ID
        /// </summary>
        public string RepIDField
        {
            get => _repIDField;
            set
            {
                _repIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者名
        /// </summary>
        public string RepName
        {
            get => _repName;
            set
            {
                if (value == null) return;
                _repName = value.Replace('　', ' ');
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
            get => _repCurrentPassword;
            set
            {
                _repCurrentPassword = value;
                ValidationProperty(nameof(RepCurrentPassword), value);
                if (CurrentOperation == DataOperation.更新)
                {
                    IsRepNewPasswordEnabled = value == CurrentRep.Password;
                    SetRepOperationButtonEnabled();
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しい担当者パスワード
        /// </summary>
        public string RepNewPassword
        {
            get => _repNewPassword;
            set
            {
                _repNewPassword = value;
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
            get => _isRepValidity;
            set
            {
                _isRepValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者名欄のEnable設定
        /// </summary>
        public bool IsRepNameDataEnabled
        {
            get => _isRepNameDataEnabled;
            set
            {
                _isRepNameDataEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者の現在のパスワード欄のEnable設定
        /// </summary>
        public bool IsRepPasswordEnabled
        {
            get => _isRepPasswordEnabled;
            set
            {
                _isRepPasswordEnabled = value;
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
            get => _repDataOperationButtonContent;
            set
            {
                _repDataOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者検索文字列
        /// </summary>
        public string ReferenceRepName
        {
            get => _referenceRepName;
            set
            {
                _referenceRepName = value;
                RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, RepValidityTrueOnly);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データの有効性
        /// </summary>
        public bool RepValidityTrueOnly
        {
            get => _repValidityTrueOnly;
            set
            {
                _repValidityTrueOnly = value;
                RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄の文字を隠すかのチェック
        /// </summary>
        public bool NewPasswordCharCheck
        {
            get => _newPasswordCharCheck;
            set
            {
                _newPasswordCharCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 現在のパスワードの文字を隠すかのチェック
        /// </summary>
        public bool CurrentPasswordCharCheck
        {
            get => _currentPasswordCharCheck;
            set
            {
                _currentPasswordCharCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄のEnabled
        /// </summary>
        public bool IsRepNewPasswordEnabled
        {
            get => _isRepNewPasswordEnabled;
            set
            {
                _isRepNewPasswordEnabled = value;
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
            get => _currentRepPasswordBorderBrush;
            set
            {
                _currentRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 現在のパスワード欄の背景の色
        /// </summary>
        public string CurrentRepPasswordBackground
        {
            get => _currentRepPasswordBackground;
            set
            {
                _currentRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄のボーダーの色
        /// </summary>
        public string NewRepPasswordBorderBrush
        {
            get => _newRepPasswordBorderBrush;
            set
            {
                _newRepPasswordBorderBrush = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 新しいパスワード欄の背景の色
        /// </summary>
        public string NewRepPasswordBackground
        {
            get => _newRepPasswordBackground;
            set
            {
                _newRepPasswordBackground = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者検索メニューEnabled
        /// </summary>
        public bool IsRepReferenceMenuEnabled
        {
            get => _isRepReferenceMenuEnabled;
            set
            {
                _isRepReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者詳細に表示されている担当者
        /// </summary>
        public Rep CurrentRep
        {
            get => _currentRep;
            set
            {
                _currentRep = value;
                if (_currentRep != null) SetRepDetailProperty();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者詳細に担当者クラスのプロパティを代入します
        /// </summary>
        private void SetRepDetailProperty()
        {
            RepIDField = CurrentRep.ID;
            RepName = CurrentRep.Name;
            IsRepValidity = CurrentRep.IsValidity;
            RepCurrentPassword = string.Empty;
        }
        /// <summary>
        /// 担当者リスト
        /// </summary>
        public ObservableCollection<Rep> RepList
        {
            get => _repList;
            set
            {
                _repList = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当者データ操作ボタンEnabled
        /// </summary>
        public bool IsRepOperationButtonEnabled
        {
            get => _isRepOperationButtonEnabled;
            set
            {
                _isRepOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 再入力パスワード
        /// </summary>
        public string ConfirmationPassword
        {
            get => _confirmationPassword;
            set
            {
                _confirmationPassword = value;
                ValidationProperty(nameof(ConfirmationPassword), value);
                SetRepOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 再入力パスワード欄の文字を隠すかのチェック
        /// </summary>
        public bool ConfirmationPasswordCharCheck
        {
            get => _confirmationPasswordCharCheck;
            set
            {
                _confirmationPasswordCharCheck = value;
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
            if (!IsAdminPermisson)
            {
                IsRepOperationButtonEnabled = false;
                return;
            }
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    IsRepOperationButtonEnabled = !(string.IsNullOrEmpty(RepName) | string.IsNullOrEmpty(RepNewPassword) | string.IsNullOrEmpty(ConfirmationPassword));
                    if (IsRepOperationButtonEnabled) IsRepOperationButtonEnabled = RepNewPassword == ConfirmationPassword;
                    break;
                case DataOperation.更新:
                    IsRepOperationButtonEnabled = CurrentRep.Password == RepCurrentPassword;
                    if (IsRepOperationButtonEnabled) IsRepOperationButtonEnabled = RepNewPassword == ConfirmationPassword;
                    break;
                default:
                    break;
            }
        }

        #endregion
        #region AccountingSubjectOperation

        /// <summary>
        /// 勘定科目ID
        /// </summary>
        public string AccountingSubjectIDField
        {
            get => accountingSubjectIDField;
            set
            {
                accountingSubjectIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目有効性
        /// </summary>
        public bool IsAccountingSubjectValidity
        {
            get => isAccountingSubjectValidity;
            set
            {
                isAccountingSubjectValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コード
        /// </summary>
        public string AccountingSubjectCodeField
        {
            get => accountingSubjectCodeField;
            set
            {
                accountingSubjectCodeField = int.TryParse(value, out int i) ? i.ToString("000") : string.Empty;
                ValidationProperty(nameof(AccountingSubjectCodeField), value);
                SetAccountingSubjectOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目
        /// </summary>
        public string AccountingSubjectField
        {
            get => accountingSubjectField;
            set
            {
                accountingSubjectField = value;
                ValidationProperty(nameof(AccountingSubjectField), value);
                SetAccountingSubjectOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目データ操作ボタンのContent
        /// </summary>
        public string AccountingSubnectOperationButtonContent
        {
            get => accountingSubnectOperationButtonContent;
            set
            {
                accountingSubnectOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目データ操作ボタンのEnabled
        /// </summary>
        public bool IsAccountingSubjectOperationButtonEnabled
        {
            get => isAccountingSubjectOperationButtonEnabled;
            set
            {
                isAccountingSubjectOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目を詳細欄に表示します
        /// </summary>
        public AccountingSubject CurrentAccountingSubject
        {
            get => currentAccountingSubject;
            set
            {
                currentAccountingSubject = value;

                AccountingSubjectIDField = currentAccountingSubject.ID;
                AccountingSubjectCodeField = currentAccountingSubject.SubjectCode;
                AccountingSubjectField = currentAccountingSubject.Subject;
                isAccountingSubjectValidity = currentAccountingSubject.IsValidity;
                SetAccountingSubjectOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目データ操作ボタンのEnabledを設定します
        /// </summary>
        private void SetAccountingSubjectOperationButtonEnabled()
        {
            if (!HasErrors)
            {
                IsAccountingSubjectOperationButtonEnabled = true;
            }
            IsAccountingSubjectOperationButtonEnabled = !string.IsNullOrEmpty(AccountingSubjectCodeField) & !string.IsNullOrEmpty(AccountingSubjectField);
        }
        /// <summary>
        /// 勘定科目データ操作コマンド
        /// </summary>
        public DelegateCommand AccountingSubjectDataOperationCommand { get; set; }
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
        /// 勘定科目リストを有効なデータのみ表示するか
        /// </summary>
        public bool IsAccountingSubjectValidityTrueOnly
        {
            get => isAccountingSubjectValidityTrueOnly;
            set
            {
                isAccountingSubjectValidityTrueOnly = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する勘定科目コード
        /// </summary>
        public string ReferenceAccountingSubjectCode
        {
            get => referenceAccountingSubjectCode;
            set
            {
                referenceAccountingSubjectCode = int.TryParse(value, out int i) ? i.ToString("000") : string.Empty;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する勘定科目
        /// </summary>
        public string ReferenceAccountingSubject
        {
            get => referenceAccountingSubject;
            set
            {
                referenceAccountingSubject = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目検索メニューのEnabled
        /// </summary>
        public bool IsAccountingSubjectReferenceMenuEnabled
        {
            get => isAccountingSubjectReferenceMenuEnabled;
            set
            {
                isAccountingSubjectReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードのEnabled
        /// </summary>
        public bool IsAccountingSubjectCodeFieldEnabled
        {
            get => isAccountingSubjectCodeFieldEnabled;
            set
            {
                isAccountingSubjectCodeFieldEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目のEnabled
        /// </summary>
        public bool IsAccountingSubjectFieldEnabled
        {
            get => isAccountingSubjectFieldEnabled;
            set
            {
                isAccountingSubjectFieldEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目データを登録、更新します
        /// </summary>
        public void AccountingSubjectDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    AccountSubjectRetistration();
                    break;
            }
        }
        /// <summary>
        /// 勘定科目データを登録します
        /// </summary>
        private async void AccountSubjectRetistration()
        {
            CurrentAccountingSubject = new AccountingSubject(null, AccountingSubjectCodeField, AccountingSubjectField, IsAccountingSubjectValidity);
            if (CallConfirmationDataOperation($"勘定科目コード : {CurrentAccountingSubject.SubjectCode}\r\n勘定科目 : {CurrentAccountingSubject.Subject}" +
                $"\r\n有効性 : {CurrentAccountingSubject.IsValidity}\r\n\r\n登録しますか？", "勘定科目") == MessageBoxResult.Cancel) return;

            IsAccountingSubjectOperationButtonEnabled = false;
            AccountingSubnectOperationButtonContent = "登録中";
            await Task.Run(() => DataBaseConnect.Registration(CurrentAccountingSubject, LoginRep.Rep));
            IsAccountingSubjectOperationButtonEnabled = true;
            AccountingSubnectOperationButtonContent = "登録";

            AccountingSubjectDetailFieldClear();
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, IsAccountingSubjectValidityTrueOnly);
        }
        /// <summary>
        /// 勘定科目詳細フィールドをクリアします
        /// </summary>
        private void AccountingSubjectDetailFieldClear()
        {
            AccountingSubjectCodeField = string.Empty;
            AccountingSubjectField = string.Empty;
            IsAccountingSubjectValidity = true;
        }
        /// <summary>
        /// 勘定科目登録のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledRegisterAccountingSubject()
        {
            AccountingSubjectDetailFieldClear();
            IsAccountingSubjectReferenceMenuEnabled = false;
            SetAccountingSubjectOperationButtonEnabled();
            IsAccountingSubjectValidity = true;
            IsAccountingSubjectCodeFieldEnabled = true;
            IsAccountingSubjectFieldEnabled = true;
        }
        /// <summary>
        /// 勘定科目更新のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledUpdateAccountingSubject()
        {
            IsAccountingSubjectReferenceMenuEnabled = true;
            IsAccountingSubjectCodeFieldEnabled = false;
            IsAccountingSubjectFieldEnabled = false;
        }
        private void AccountingSubjectUpdate()
        {
            if (CurrentAccountingSubject.IsValidity == IsAccountingSubjectValidity)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            if (CallConfirmationDataOperation($"有効性 : {CurrentAccountingSubject.IsValidity} → {IsAccountingSubjectValidity}", "勘定科目") == MessageBoxResult.Cancel)
                return;

            CurrentAccountingSubject.IsValidity = IsAccountingSubjectValidity;
        }
        #endregion
        #region CreditAccountOperation
        /// <summary>
        /// 貸方勘定ID
        /// </summary>
        public string CreditAccountIDField
        {
            get => creditAccountIDField;
            set
            {
                creditAccountIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定有効性
        /// </summary>
        public bool IsCreditAccountValidity
        {
            get => isCreditAccountValidity;
            set
            {
                isCreditAccountValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定
        /// </summary>
        public string CreditAccountField
        {
            get => creditAccountField;
            set
            {
                creditAccountField = value;
                ValidationProperty(nameof(CreditAccountField), value);
                CallPropertyChanged();
            }
        }
        #endregion
        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(RepName):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(RepCurrentPassword):
                    if (IsRepPasswordEnabled) ErrorsListOperation(RepCurrentPassword != CurrentRep.Password, propertyName, Properties.Resources.PasswordErrorInfo);
                    break;
                case nameof(RepNewPassword):
                    if (IsRepNewPasswordEnabled) SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ConfirmationPassword):
                    if (!_isRepNewPasswordEnabled) break;
                    SetNullOrEmptyError(propertyName, value.ToString());
                    if (GetErrors(propertyName)==null) ErrorsListOperation(RepNewPassword != ConfirmationPassword, propertyName, Properties.Resources.PasswordErrorInfo);
                    break;
                case nameof(AccountingSubjectCodeField):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    if (GetErrors(propertyName) == null) ErrorsListOperation(AccountingSubjectCodeField.Length != 3, propertyName, "コードの桁数が不正です");
                    break;
                case nameof(AccountingSubjectField):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ReferenceAccountingSubjectCode):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    if (GetErrors(propertyName) == null) ErrorsListOperation(ReferenceAccountingSubjectCode.Length != 3, propertyName, "コードの桁数が不正です");
                    break;
                case nameof(CreditAccountField):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                default:
                    break;
            }
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = "データ管理";
            return DefaultWindowTitle;
        }
    }
}