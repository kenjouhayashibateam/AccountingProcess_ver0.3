using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using WPF.Views.Datas;
using WPF.ViewModels.Commands;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;

namespace WPF.ViewModels
{
    /// <summary>
    /// データ管理画面
    /// </summary>
    public class DataManagementViewModel : DataOperationViewModel
    {
        #region Properties
        #region RepProperties
        private string _repIDField;
        private string _repName;
        private string _repCurrentPassword;
        private string _repNewPassword;
        private string _confirmationPassword;
        private string _repDataOperationButtonContent;
        private string _referenceRepName = string.Empty;
        private string _currentRepPasswordBorderBrush;
        private string _currentRepPasswordBackground;
        private string _newRepPasswordBorderBrush;
        private string _newRepPasswordBackground;
        private readonly string TrueControlBorderBrush = "#000000";
        private readonly string FalseControlBorderBrush = "#FFABADB3";
        private readonly string TrueControlBackground = "#FFFFFF";
        private readonly string FalseControlBackground = "#A9A9A9";
        private bool _isRepValidity;
        private bool _isRepNameDataEnabled;
        private bool _isRepPasswordEnabled;
        private bool _isRepNewPasswordEnabled;
        private bool _repValidityTrueOnly;
        private bool _newPasswordCharCheck;
        private bool _currentPasswordCharCheck;
        private bool _confirmationPasswordCharCheck;
        private bool _isRepReferenceMenuEnabled;
        private bool _isRepOperationButtonEnabled;
        private bool _isRepDataAdminPermisson;
        private Rep _currentRep;
        private ObservableCollection<Rep> _repList;
        #endregion
        #region AccountingSubjectProperties
        private string accountingSubjectIDField;
        private string accountingSubjectCodeField;
        private string accountingSubjectField;
        private string accountingSubnectOperationButtonContent;
        private string referenceAccountingSubjectCode;
        private string referenceAccountingSubject;
        private bool isAccountingSubjectOperationButtonEnabled;
        private bool isAccountingSubjectValidity;
        private bool isAccountingSubjectValidityTrueOnly;
        private bool isAccountingSubjectReferenceMenuEnabled;
        private bool isAccountingSubjectCodeFieldEnabled;
        private bool isAccountingSubjectFieldEnabled;
        private AccountingSubject currentAccountingSubject;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        #endregion
        #region CreditAccountProperties
        private string creditAccountIDField;
        private string creditAccountField;
        private string creditAccountOperationButtonContent;
        private string referenceCreditAccount;
        private bool isCreditAccountOperationButtonEnabled;
        private bool isCreditAccountEnabled;
        private bool isCreditAccountValidity;
        private bool isCreditAccountReferenceMenuEnabled;
        private bool isCreditAccountValidityTrueOnly;
        private bool isShunjuenAccount;
        private CreditAccount currentCreditAccount;
        private ObservableCollection<CreditAccount> creditAccounts;
        #endregion
        #region ContentProperties
        private string contentIDField;
        private string selectedAccountingSubjectField;
        private string contentField;
        private string flatRateField;
        private string contentDataOperationContent;
        private string referenceContent;
        private bool isContentValidity;
        private bool isContentOperationEnabled;
        private bool isAffiliationAccountingSubjectEnabled;
        private bool isContentFieldEnabled;
        private bool isContentValidityTrueOnly;
        private bool isContentReferenceMenuEnabled;
        private ObservableCollection<AccountingSubject> affiliationAccountingSubjects;
        private AccountingSubject affiliationAccountingSubject;
        private Content currentContent;　
        private ObservableCollection<Content> contents;
        #endregion
        #endregion

        public DataManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            AffiliationAccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            SetDataRegistrationCommand.Execute();
        }
        public DataManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        protected override void SetDelegateCommand()
        {
            SetRepDelegateCommand();
            AccountingSubjectDataOperationCommand = new DelegateCommand(() => AccountingSubjectDataOperation(), () => IsAccountingSubjectOperationButtonEnabled);
            CreditAccountDataOperationCommand = new DelegateCommand(() => CreditAccountDataOperation(), () => IsCreditAccountOperationButtonEnabled);
            ContentDataOperationCommand = new DelegateCommand(() => ContentDataOperation(), () => IsContentOperationEnabled);
        }

        protected override void SetDataList()
        {
            RepList = DataBaseConnect.ReferenceRep(string.Empty, false);
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, false);
            CreditAccounts = DataBaseConnect.ReferenceCreditAccount(string.Empty, false,false);
            Contents = DataBaseConnect.ReferenceContent(string.Empty,string.Empty,string.Empty, false);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
            RepDataOperationButtonContent = operation.ToString();
            AccountingSubnectOperationButtonContent = operation.ToString();
            CreditAccountOperationButtonContent = operation.ToString();
            ContentDataOperationContent = operation.ToString();
        }

        protected override void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    SetFieldEnabledRegisterRep();
                    SetFieldEnabledRegisterAccountingSubject();
                    SetFieldEnabledRegisterCreditAccount();
                    SetFieldEnabledRegisterContent();
                    break;
                case DataOperation.更新:
                    SetFieldEnabledUpdaterRep();
                    SetFieldEnabledUpdateAccountingSubject();
                    SetFieldEnabledUpdateCreditAccount();
                    SetFieldEnabledUpdateContent();
                    break;
                default:
                    break;
            }
        }

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
            CurrentRep = new Rep(null, RepName, RepNewPassword, IsRepValidity, IsRepDataAdminPermisson);
            if (CallConfirmationDataOperation
                ($"担当者名 : {CurrentRep.Name}\r\nパスワード : {new string('*', RepNewPassword.Length)}\r\n有効性 : {CurrentRep.IsValidity}\r\n管理者権限 : {CurrentRep.IsAdminPermisson}\r\n \r\n登録しますか？",
                "担当者") == MessageBoxResult.Cancel)
                return;

            IsRepOperationButtonEnabled = false;
            RepDataOperationButtonContent = "登録中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => DataBaseConnect.Registration(CurrentRep));
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
            await Task.Run(() => DataBaseConnect.Update(CurrentRep));
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
            IsRepOperationButtonEnabled = false;
            RepDetailClear();
        }
        /// <summary>
        /// 担当者管理の登録のためのコントロールのEnableを設定し、フィールドをクリアします
        /// </summary>
        private void SetFieldEnabledRegisterRep()
        {
            IsRepValidity = true;
            IsRepDataAdminPermisson = true;
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
            RepName = string.Empty;
            RepCurrentPassword = string.Empty;
            RepNewPassword = string.Empty;
            ConfirmationPassword = string.Empty;
            CurrentRep = new Rep(null, null, null, true, false);
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
        /// 担当者データの有効性Trueのみ検索チェック
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
                else _currentRep = new Rep(string.Empty, string.Empty, string.Empty, true, false);
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
            IsAdminPermisson = CurrentRep.IsAdminPermisson;
            IsRepDataAdminPermisson = CurrentRep.IsAdminPermisson;
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
        /// <summary>
        /// 担当者詳細の管理者権限チェック
        /// </summary>
        public bool IsRepDataAdminPermisson
        {
            get => _isRepDataAdminPermisson;
            set
            {
                _isRepDataAdminPermisson = value;
                CallPropertyChanged();
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
                if (value == null) AccountingSubjectDetailFieldClear();
                else
                {
                    AccountingSubjectIDField = currentAccountingSubject.ID;
                    AccountingSubjectCodeField = currentAccountingSubject.SubjectCode;
                    AccountingSubjectField = currentAccountingSubject.Subject;
                    isAccountingSubjectValidity = currentAccountingSubject.IsValidity;
                    SetAccountingSubjectOperationButtonEnabled();
                }
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
                return;
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
                case DataOperation.更新:
                    AccountingSubjectUpdate();
                    break;
                default:
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
            await Task.Run(() => DataBaseConnect.Registration(CurrentAccountingSubject));
            IsAccountingSubjectOperationButtonEnabled = true;
            AccountingSubnectOperationButtonContent = "登録";

            AccountingSubjectDetailFieldClear();
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, IsAccountingSubjectValidityTrueOnly);
            AffiliationAccountingSubjects = AccountingSubjects;
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
            isAccountingSubjectOperationButtonEnabled = false;
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
                IsCreditAccountOperationButtonEnabled = !string.IsNullOrEmpty(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定データ操作ボタンのContent
        /// </summary>
        public string CreditAccountOperationButtonContent
        {
            get => creditAccountOperationButtonContent;
            set
            {
                creditAccountOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定データ操作ボタンのEnabled
        /// </summary>
        public bool IsCreditAccountOperationButtonEnabled
        {
            get => isCreditAccountOperationButtonEnabled;
            set
            {
                isCreditAccountOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定フィールドのEnabled
        /// </summary>
        public bool IsCreditAccountEnabled
        {
            get => isCreditAccountEnabled;
            set
            {
                isCreditAccountEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定登録のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledRegisterCreditAccount()
        {
            IsCreditAccountValidity = true;
            IsCreditAccountEnabled = true;
            IsCreditAccountReferenceMenuEnabled = false;
            IsCreditAccountOperationButtonEnabled = false;
            IsShunjuenAccount = true;
            CreditAccountField = string.Empty;
        }
        /// <summary>
        /// 貸方勘定更新のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledUpdateCreditAccount()
        {
            IsCreditAccountReferenceMenuEnabled = true;
            IsCreditAccountEnabled = false;
            IsCreditAccountOperationButtonEnabled = false;
        }
        /// <summary>
        /// 貸方勘定データ操作コマンド
        /// </summary>
        public DelegateCommand CreditAccountDataOperationCommand { get; set; }
        /// <summary>
        /// 選択された貸方勘定
        /// </summary>
        public CreditAccount CurrentCreditAccount
        {
            get => currentCreditAccount;
            set
            {
                currentCreditAccount = value;
                if(value!=null)
                {
                    CreditAccountIDField = value.ID;
                    CreditAccountField = value.Account;
                    IsCreditAccountValidity = value.IsValidity;
                    IsShunjuenAccount = value.IsShunjuenAccount;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定検索メニューのEnable
        /// </summary>
        public bool IsCreditAccountReferenceMenuEnabled
        {
            get => isCreditAccountReferenceMenuEnabled;
            set
            {
                isCreditAccountReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方勘定
        /// </summary>
        public string ReferenceCreditAccount
        {
            get => referenceCreditAccount;
            set
            {
                referenceCreditAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方勘定のValidity
        /// </summary>
        public bool IsCreditAccountValidityTrueOnly
        {
            get => isCreditAccountValidityTrueOnly;
            set
            {
                isCreditAccountValidityTrueOnly = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索した貸方勘定リスト
        /// </summary>
        public ObservableCollection<CreditAccount> CreditAccounts
        {
            get => creditAccounts;
            set
            {
                creditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定データを登録、更新します
        /// </summary>
        private void CreditAccountDataOperation()
        {
            switch(CurrentOperation)
            {
                case DataOperation.登録:
                    CreditAccountDataRegistration();
                    break;
                case DataOperation.更新:
                    CreditAccountDataUpdate();
                    break;
                default:
                    break;
            }    
        }
        /// <summary>
        /// 貸方勘定データを登録します
        /// </summary>
        private async void CreditAccountDataRegistration()
        {
            CurrentCreditAccount = new CreditAccount(null, CreditAccountField, IsCreditAccountValidity,IsShunjuenAccount);

           if (CallConfirmationDataOperation($"貸方勘定 : {CreditAccountField}\r\n有効性 : {IsCreditAccountValidity}", "貸方勘定") == MessageBoxResult.Cancel) return;

            CreditAccountOperationButtonContent = "登録中";
            IsCreditAccountOperationButtonEnabled = false;
            await Task.Run(() => DataBaseConnect.Registration(CurrentCreditAccount));
            CreditAccountOperationButtonContent = "登録";
            IsCreditAccountOperationButtonEnabled = true;

            CreditAccounts = DataBaseConnect.ReferenceCreditAccount(string.Empty, IsCreditAccountValidityTrueOnly,false);
            CreditAccountFieldClear();
        }
        private void CreditAccountFieldClear()
        {
            CreditAccountField = string.Empty;
            IsCreditAccountValidity = true;
        }
        /// <summary>
        /// 貸方勘定データを更新します
        /// </summary>
        private async void CreditAccountDataUpdate()
        {
            if(IsCreditAccountValidity==CurrentCreditAccount.IsValidity)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            CreditAccountOperationButtonContent = "更新中";
            IsCreditAccountOperationButtonEnabled = false;
            await Task.Run(() => DataBaseConnect.Update(CurrentCreditAccount));
            IsCreditAccountOperationButtonEnabled = true;
            CreditAccountOperationButtonContent = "更新";
        }
        #endregion
        #region ContentOperation
        
        /// <summary>
        /// 伝票内容ID
        /// </summary>
        public string ContentIDField
        {
            get => contentIDField;
            set
            {
                contentIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容の有効性
        /// </summary>
        public bool IsContentValidity
        {
            get => isContentValidity;
            set
            {
                isContentValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容データ登録で使用する勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> AffiliationAccountingSubjects
        {
            get => affiliationAccountingSubjects;
            set
            {
                affiliationAccountingSubjects = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容が所属する勘定科目
        /// </summary>
        public AccountingSubject AffiliationAccountingSubject
        {
            get => affiliationAccountingSubject;
            set
            {
                affiliationAccountingSubject = value;
                ValidationProperty(nameof(AffiliationAccountingSubject), value);
                if(affiliationAccountingSubject!=null) SelectedAccountingSubjectField = affiliationAccountingSubject.Subject;
                SetContentOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容
        /// </summary>
        public string ContentField
        {
            get => contentField;
            set
            {
                contentField = value;
                ValidationProperty(nameof(ContentField), value);
                SetContentOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容で決められた定額
        /// </summary>
        public string FlatRateField
        {
            get => flatRateField;
            set
            {
                string s = value.Replace(",",string.Empty);
                flatRateField = int.TryParse(s, out int i) ? TextHelper.CommaDelimitedAmount(i) : string.Empty;
                flatRateField = (i <= 0) ? string.Empty : TextHelper.CommaDelimitedAmount(i);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目
        /// </summary>
        public string SelectedAccountingSubjectField
        {
            get => selectedAccountingSubjectField;
            set
            {
                selectedAccountingSubjectField = (AffiliationAccountingSubject == null) ? string.Empty : value;
                ValidationProperty(nameof(SelectedAccountingSubjectField), selectedAccountingSubjectField);
                SetContentOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容データ操作ボタンのContent
        /// </summary>
        public string ContentDataOperationContent
        {
            get => contentDataOperationContent;
            set
            {
                contentDataOperationContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票データ操作ボタンのEnable
        /// </summary>
        public bool IsContentOperationEnabled
        {
            get => isContentOperationEnabled;
            set
            {
                isContentOperationEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 所属する勘定科目欄のEnable
        /// </summary>
        public bool IsAffiliationAccountingSubjectEnabled
        {
            get => isAffiliationAccountingSubjectEnabled;
            set
            {
                isAffiliationAccountingSubjectEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容のEnable
        /// </summary>
        public bool IsContentFieldEnabled
        {
            get => isContentFieldEnabled;
            set
            {
               isContentFieldEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容登録時のEnableを設定します
        /// </summary>
        private void SetFieldEnabledRegisterContent()
        {
            IsAffiliationAccountingSubjectEnabled = true;
            IsContentFieldEnabled = true;
            IsContentValidity = true;
            IsContentReferenceMenuEnabled = false;
            ContentDetailFieldClear();
        }
        /// <summary>
        /// 伝票内容更新時のEnableを設定します
        /// </summary>
        private void SetFieldEnabledUpdateContent()
        {
            IsAffiliationAccountingSubjectEnabled = false;
            IsContentFieldEnabled = false;
            IsContentReferenceMenuEnabled = true;
        }
        /// <summary>
        /// 伝票内容のデータ操作ボタンのEnableを設定します
        /// </summary>
        private void SetContentOperationButtonEnabled()
        {
            if(!HasErrors)
            {
                IsContentOperationEnabled = true;
                return;
            }

            IsContentOperationEnabled = affiliationAccountingSubject != null & !string.IsNullOrEmpty(ContentField);
        }
        /// <summary>
        /// 伝票内容データ操作コマンド
        /// </summary>
        public DelegateCommand ContentDataOperationCommand { get; set; }
        /// <summary>
        /// 詳細に表示された伝票内容
        /// </summary>
        public Content CurrentContent
        {
            get => currentContent;
            set
            {
                currentContent = value;
                if (currentContent == null) ContentDetailFieldClear();
                else
                {
                    IsContentValidity = currentContent.IsValidity;
                    AffiliationAccountingSubject = currentContent.AccountingSubject;
                    ContentField = currentContent.Text;
                    FlatRateField = currentContent.FlatRate.ToString();
                }
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
        /// 伝票内容検索で有効な物のみ表示するか
        /// </summary>
        public bool IsContentValidityTrueOnly
        {
            get => isContentValidityTrueOnly;
            set
            {
                isContentValidityTrueOnly = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する伝票内容
        /// </summary>
        public string ReferenceContent
        {
            get => referenceContent;
            set
            {
                referenceContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索メニューのEnable
        /// </summary>
        public bool IsContentReferenceMenuEnabled
        {
            get => isContentReferenceMenuEnabled;
            set
            {
                isContentReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 春秋苑会計に掲載されるデータかのチェック
        /// </summary>
        public bool IsShunjuenAccount
        {
            get => isShunjuenAccount;
            set
            {
                isShunjuenAccount = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 伝票内容データを登録、更新します
        /// </summary>
        private void ContentDataOperation()
        {
            switch(CurrentOperation)
            {
                case DataOperation.登録:
                    ContentRegistration();
                    break;
                case DataOperation.更新:
                    ContentUpdate();
                    break;
            }
        }
        /// <summary>
        /// 伝票内容データを登録します
        /// </summary>
        private async void ContentRegistration()
        {
            int i = (!int.TryParse(FlatRateField.Replace(",", string.Empty), out int j)) ? -1 : j;
            CurrentContent = new Content(null, AffiliationAccountingSubject, i, ContentField, IsContentValidity);
            string flatRateInfo = (i > 0) ? TextHelper.AmountWithUnit(i) : "金額設定無し";
            if (CallConfirmationDataOperation($"伝票内容 : {CurrentContent.Text}\r\n勘定科目 : {CurrentContent.AccountingSubject.Subject}\r\n" +
                $"定額 : {flatRateInfo}\r\n有効性 : {CurrentContent.IsValidity}", "伝票内容") == MessageBoxResult.Cancel)
                return;

            ContentDataOperationContent = "登録中";
            IsContentOperationEnabled = false;
            await Task.Run(() => DataBaseConnect.Registration(CurrentContent));
            IsContentOperationEnabled = true;
            ContentDataOperationContent = "登録";

            Contents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty,string.Empty, IsContentValidityTrueOnly);
            ContentDetailFieldClear();
        }
        /// <summary>
        /// 詳細に表示された伝票内容をクリアします
        /// </summary>
        private void ContentDetailFieldClear()
        {
            AffiliationAccountingSubject = null;
            FlatRateField = string.Empty;
            ContentField = string.Empty;
            IsContentValidity = true;
        }
        /// <summary>
        /// 伝票内容データを更新します
        /// </summary>
        private async void ContentUpdate()
        {
            string updateContents=string.Empty;

            int oldFlatRate = CurrentContent.FlatRate;
            if (oldFlatRate < 0) oldFlatRate = 0;

            int newFlatRate = int.TryParse(FlatRateField, out int i) ? i : 0;
            if (newFlatRate < 0) newFlatRate = 0;

            if (oldFlatRate != newFlatRate)updateContents= $"定額 : {TextHelper.AmountWithUnit(CurrentContent.FlatRate)} → {FlatRateField}円\r\n";
            if (CurrentContent.IsValidity != IsContentValidity) updateContents += $"有効性 : {CurrentContent.IsValidity} → {IsContentValidity}\r\n";

            if(updateContents.Length==0)
            {
                CallNoRequiredUpdateMessage();
                return;
            }
            if (CallConfirmationDataOperation($"伝票内容 : {ContentField}\r\n\r\n{updateContents}", "伝票内容") == MessageBoxResult.Cancel)
                return;

            i = (!int.TryParse(FlatRateField.Replace(",", string.Empty), out int j)) ? -1 : j;
            CurrentContent = new Content(ContentIDField, AffiliationAccountingSubject, i, ContentField, IsContentValidity);

            IsContentOperationEnabled = false;
            ContentDataOperationContent = "更新中";
            await Task.Run(() => DataBaseConnect.Update(CurrentContent));
            ContentDataOperationContent = "更新";
            IsContentOperationEnabled = true;

            ContentDetailFieldClear();
            Contents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty,string.Empty, IsContentValidityTrueOnly);
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
                case nameof(SelectedAccountingSubjectField):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ContentField):
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