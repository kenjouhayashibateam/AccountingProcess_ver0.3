using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

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
        private string referenceAccountingSubjectCode = string.Empty;
        private string referenceAccountingSubject = string.Empty;
        private string branchNumber = string.Empty;
        private string branchNumberOperationContent;
        private bool isAccountingSubjectOperationButtonEnabled;
        private bool isAccountingSubjectValidity;
        private bool isAccountingSubjectValidityTrueOnly;
        private bool isAccountingSubjectReferenceMenuEnabled;
        private bool isAccountingSubjectCodeFieldEnabled;
        private bool isAccountingSubjectFieldEnabled;
        private bool isBranchNumberVisibility;
        private bool isShunjuen;
        private bool isBranchNumberOperationVisibility;
        private AccountingSubject currentAccountingSubject;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        #endregion
        #region CreditDeptProperties
        private string creditDeptIDField;
        private string creditDeptField = string.Empty;
        private string creditDeptOperationButtonContent;
        private string referenceCreditDept;
        private bool isCreditDeptOperationButtonEnabled;
        private bool isCreditDeptEnabled;
        private bool isCreditDeptValidity;
        private bool isCreditDeptReferenceMenuEnabled;
        private bool isCreditDeptValidityTrueOnly;
        private bool isShunjuenDept;
        private CreditDept currentCreditDept;
        private ObservableCollection<CreditDept> creditDepts;
        #endregion
        #region ContentProperties
        #region Strings
        private string contentIDField;
        private string selectedAccountingSubjectField;
        private string contentField;
        private string flatRateField;
        private string contentDataOperationContent;
        private string referenceContent = string.Empty;
        private string affiliationAccountingSubjectCode;
        private string referenceAccountingSubjectCodeBelognsContent = string.Empty;
        private string referenceAccountingSubjectBelognsContent = string.Empty;
        private string contentConvertText;
        private string contentConvertOperationContent;
        private string contentDefaultDeptOperationContent;
        #endregion
        #region Bools
        private bool isContentValidity;
        private bool isContentOperationEnabled;
        private bool isAffiliationAccountingSubjectEnabled;
        private bool isContentFieldEnabled;
        private bool isContentValidityTrueOnly;
        private bool isContentReferenceMenuEnabled;
        private bool isContentConvertButtonVisibility;
        private bool isContentConvertOperationButtonEnabled;
        private bool isContentConvertVoucherRegistration;
        private bool isContentDefaultCreditDeptButtonVisibility;
        private bool isContentDefaultCreditDeptSetting;
        private bool isContentDefaultCreditDeptRegistration;
        private bool isContentDefaultCreditDeptEnabled = true;
        private bool isContentDefaultCreditDeptSettingEnabled = true;
        #endregion
        private AccountingSubject affiliationAccountingSubject;
        private Content currentContent;
        private CreditDept selectedContentDefaultCreditDept;
        private ObservableCollection<AccountingSubject> affiliationAccountingSubjects;
        private ObservableCollection<Content> contents;
        private ObservableCollection<CreditDept> contentDefaultCreditDepts;
        #endregion
        #endregion

        public DataManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            AffiliationAccountingSubjects = DataBaseConnect.ReferenceAccountingSubject
                                                                (string.Empty, string.Empty,
                                                                    AccountingProcessLocation.IsAccountingGenreShunjuen, true);
            ContentDefaultCreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, true, AccountingProcessLocation.IsAccountingGenreShunjuen);
            SetDataRegistrationCommand.Execute();
            IsContentDefaultCreditDeptRegistration = true;
            IsBranchNumberVisibility = !AccountingProcessLocation.IsAccountingGenreShunjuen;
            IsShunjuenDept = IsShunjuen = AccountingProcessLocation.IsAccountingGenreShunjuen;            
        }
        public DataManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

        protected override void SetDelegateCommand()
        {
            SetRepDelegateCommand();
            AccountingSubjectDataOperationCommand =
                new DelegateCommand(() => AccountingSubjectDataOperation(),
                                                                () => IsAccountingSubjectOperationButtonEnabled);
            CreditDeptDataOperationCommand =
                new DelegateCommand(() => CreditDeptDataOperation(),
                                                                () => IsCreditDeptOperationButtonEnabled);
            ContentDataOperationCommand =
                new DelegateCommand(() => ContentDataOperation(), () => IsContentOperationEnabled);
            ContentConvertVoucherOperationCommand = new DelegateCommand
                (() => ContentConvertVoucherOperation(), () => true);
            DeleteContentConvertVoucherCommand =
                new DelegateCommand(() => DeleteContentConertVoucher(), () => true);
            ContentDefaultCreditDeptOperationCommand = new DelegateCommand
                (() => ContentDefaultCreditDeptOperation(), () => true);
            DeleteContentDefaultCreditDeptCommand = new DelegateCommand
                (() => DeleteContentDefaultCreditDept(), () => true);
            BranchNumberOperationCommand = new DelegateCommand
                (() => BranchNumberOperation(), () => true);
        }

        protected override void SetDataList()
        {
            RepList = DataBaseConnect.ReferenceRep(string.Empty, false);
            ReferenceAccountingSubject = string.Empty;
            ReferenceAccountingSubjectCode = string.Empty;
            IsAccountingSubjectValidityTrueOnly = true;
            CreateAccountSubjects();
            CreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, false, AccountingProcessLocation.IsAccountingGenreShunjuen);
            CreateContents();
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
            RepDataOperationButtonContent = operation.ToString();
            AccountingSubnectOperationButtonContent = operation.ToString();
            CreditDeptOperationButtonContent = operation.ToString();
            ContentDataOperationContent = operation.ToString();
        }

        protected override void SetDetailLocked()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    SetFieldEnabledRegisterRep();
                    SetFieldEnabledRegisterAccountingSubject();
                    SetFieldEnabledRegisterCreditDept();
                    SetFieldEnabledRegisterContent();
                    break;
                case DataOperation.更新:
                    SetFieldEnabledUpdaterRep();
                    SetFieldEnabledUpdateAccountingSubject();
                    SetFieldEnabledUpdateCreditDept();
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
            RepNewPasswordCharCheckedReversCommand =
                new DelegateCommand(() => RepNewPasswordCharCheckedRevers(), () => true);
            RepCurrentPasswordCharCheckedReversCommand =
                new DelegateCommand(() => RepCurrentPasswordCharCheckedRevers(), () => true);
            ConfirmationPasswordCheckedReversCommand =
                new DelegateCommand(() => ConfirmationPasswordCharCheckedRevers(), () => true);
            RepDataOperationCommand =
                new DelegateCommand(() => RepDataOperation(), () => IsRepDataOperationCanExecute());
        }
        /// <summary>
        /// 担当者データ操作コマンドのCanExecuteを設定します
        /// </summary>
        /// <returns></returns>
        private bool IsRepDataOperationCanExecute()
        {
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
        private bool IsRepUpdatable() { return IsRepOperationButtonEnabled; }

        /// <summary>
        /// 担当者登録コマンドのCanExecuteを切り替えます
        /// </summary>
        /// <returns>CanExecute</returns>
        private bool IsRepRegistrable()
        {
            System.Collections.IEnumerable repError = GetErrors(nameof(RepName));
            if (repError == null & !string.IsNullOrEmpty(RepNewPassword))
            { repError = GetErrors(nameof(RepNewPassword)); }
            if (repError == null & !string.IsNullOrEmpty(ConfirmationPassword))
            { repError = GetErrors(nameof(ConfirmationPassword)); }
            return repError == null;
        }
        /// <summary>
        /// 担当者を登録して、担当者一覧に加えます
        /// </summary>
        private async void RepRegistration()
        {
            CurrentRep = new Rep
                                    (null, RepName, RepNewPassword, IsRepValidity, IsRepDataAdminPermisson);
            if (CallConfirmationDataOperation
                ($"担当者名 : {CurrentRep.Name}\r\n" +
                    $"パスワード : {new string('*', RepNewPassword.Length)}\r\n" +
                    $"有効性 : {CurrentRep.IsValidity}\r\n管理者権限 : {CurrentRep.IsAdminPermisson}\r\n " +
                    $"\r\n登録しますか？",
                    "担当者") == MessageBoxResult.Cancel)
            { return; }
            CurrentRep.Password = GetHashValue(RepNewPassword, CurrentRep.ID);

            IsRepOperationButtonEnabled = false;
            RepDataOperationButtonContent = "登録中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => Registration());
            CallCompletedRegistration();
            RepDetailClear();
            RepList = DataBaseConnect.ReferenceRep(string.Empty, RepValidityTrueOnly);
            IsRepOperationButtonEnabled = true;
            RepDataOperationButtonContent = "登録";

            void Registration()
            {
                _ = DataBaseConnect.Registration(CurrentRep);
                RepList = DataBaseConnect.ReferenceRep(RepName, true);
                CurrentRep = RepList[RepList.Count - 1];
                CurrentRep.Password = GetHashValue(RepNewPassword, CurrentRep.ID);
                _ = DataBaseConnect.Update(CurrentRep);
            }
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

            if (CurrentRep.IsAdminPermisson != IsRepDataAdminPermisson)
            {
                updateContents +=
                    $"管理者権限{Space}:{Space}{CurrentRep.IsAdminPermisson}{Space}→" +
                    $"{Space}{IsRepDataAdminPermisson}";
                CurrentRep.IsAdminPermisson = IsRepDataAdminPermisson;
            }

            if (RepNewPassword.Length > 0)
            {
                updateContents += $"パスワード変更 : {new string('*', RepNewPassword.Length)}\r\n";
                CurrentRep.Password = GetHashValue(RepNewPassword, CurrentRep.ID);
            }

            if (updateContents == string.Empty)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            updateContents = $"担当者 : {CurrentRep.Name}\r\n\r\n{updateContents}";
            if (CallConfirmationDataOperation
                ($"{updateContents}\r\n\r\n更新します。よろしいですか？", "担当者") ==
                    MessageBoxResult.Cancel) { return; }

            IsRepOperationButtonEnabled = false;
            RepDataOperationButtonContent = "更新中";
            LoginRep loginRep = LoginRep.GetInstance();
            _ = await Task.Run(() => DataBaseConnect.Update(CurrentRep));
            RepDetailClear();
            RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, RepValidityTrueOnly);
            CallCompletedUpdate();
            IsRepOperationButtonEnabled = true;
            RepDataOperationButtonContent = "更新";

        }
        /// <summary>
        /// 新しいパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepNewPasswordCharCheckedRevers()
        {
            NewPasswordCharCheck = !NewPasswordCharCheck;
        }

        /// <summary>
        /// 再入力パスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void ConfirmationPasswordCharCheckedRevers()
        {
            ConfirmationPasswordCharCheck = !ConfirmationPasswordCharCheck;
        }

        /// <summary>
        /// 現在のパスワード入力欄の文字を隠すかのチェックを切り替えます
        /// </summary>
        private void RepCurrentPasswordCharCheckedRevers()
        {
            CurrentPasswordCharCheck = !CurrentPasswordCharCheck;
        }

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
                if (value == null) { return; }
                _repName = value.Replace('　', ' ');
                CallPropertyChanged();
                ValidationProperty(nameof(RepName), value);
                SetRepOperationButtonEnabled();
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
                CallPropertyChanged();
                ValidationProperty(nameof(RepCurrentPassword), value);
                if (CurrentOperation == DataOperation.更新)
                {
                    IsRepNewPasswordEnabled = GetHashValue(value, CurrentRep.ID) == CurrentRep.Password;
                    SetRepOperationButtonEnabled();
                }
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
                CallPropertyChanged();
                ValidationProperty(nameof(RepNewPassword), value);
                SetRepOperationButtonEnabled();
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
                CallPropertyChanged();
                RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, RepValidityTrueOnly);
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
                CallPropertyChanged();
                RepList = DataBaseConnect.ReferenceRep(ReferenceRepName, value);
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
                CallPropertyChanged();
                if (_currentRep != null) { SetRepDetailProperty(); }
                else { _currentRep = new Rep(string.Empty, string.Empty, string.Empty, true, false); }
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
                CallPropertyChanged();
                ValidationProperty(nameof(ConfirmationPassword), value);
                SetRepOperationButtonEnabled();
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
            if (LoginRep.GetInstance().Rep == null) { return; }
            if (!LoginRep.GetInstance().Rep.IsAdminPermisson)
            {
                IsRepOperationButtonEnabled = false;
                return;
            }
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    IsRepOperationButtonEnabled = !(string.IsNullOrEmpty(RepName) |
                        string.IsNullOrEmpty(RepNewPassword) |
                        string.IsNullOrEmpty(ConfirmationPassword));
                    if (IsRepOperationButtonEnabled)
                    {
                        IsRepOperationButtonEnabled =
                            RepNewPassword == ConfirmationPassword;
                    }
                    break;
                case DataOperation.更新:
                    IsRepOperationButtonEnabled = CurrentRep.Password == GetHashValue(RepCurrentPassword, CurrentRep.ID);
                    if (!IsRepOperationButtonEnabled)
                    {
                        IsRepOperationButtonEnabled = CurrentRep.Password == RepCurrentPassword;
                    }
                    if (IsRepOperationButtonEnabled)
                    {
                        IsRepOperationButtonEnabled =
                            RepNewPassword == ConfirmationPassword;
                    }
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
                ReferenceAccountingSubjectCode = value;
                CallPropertyChanged();
                ValidationProperty(nameof(AccountingSubjectCodeField), value);
                SetAccountingSubjectOperationButtonEnabled();
            }
        }
        /// <summary>
        /// 勘定科目コード枝番
        /// </summary>
        public string BranchNumber
        {
            get => branchNumber;
            set
            {
                branchNumber = int.TryParse(value, out _) ? value : string.Empty;
                ValidationProperty(nameof(BranchNumber), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 枝番のVisiblity
        /// </summary>
        public bool IsBranchNumberVisibility
        {
            get => isBranchNumberVisibility;
            set
            {
                isBranchNumberVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 枝番の操作ボタンのContent
        /// </summary>
        public string BranchNumberOperationContent
        {
            get => branchNumberOperationContent;
            set
            {
                branchNumberOperationContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 枝番の操作ボタンのVisibility
        /// </summary>
        public bool IsBranchNumberOperationVisibility
        {
            get => isBranchNumberOperationVisibility;
            set
            {
                isBranchNumberOperationVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 枝番データ操作コマンド
        /// </summary>
        public DelegateCommand BranchNumberOperationCommand { get; set; }
        private void BranchNumberOperation()
        {
            switch (BranchNumberOperationContent)
            {
                case "登録":
                    BranchNumberRegistration(CurrentAccountingSubject);
                    CallCompletedRegistration();
                    break;
                case "更新":
                    if (string.IsNullOrEmpty(BranchNumber)) { return; }
                    if (DataBaseConnect.GetBranchNumber(CurrentAccountingSubject) == BranchNumber)
                    {
                        CallNoRequiredUpdateMessage();
                        break;
                    }
                    BranchNumberUpdate(CurrentAccountingSubject, BranchNumber);
                    CallCompletedUpdate();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 枝番を更新します
        /// </summary>
        /// <param name="accountingSubject"></param>
        /// <param name="branchNumber"></param>
        private void BranchNumberUpdate(AccountingSubject accountingSubject, string branchNumber)
        {
            _ = DataBaseConnect.Update(accountingSubject, branchNumber);
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
                CallPropertyChanged();
                ValidationProperty(nameof(AccountingSubjectField), value);
                SetAccountingSubjectOperationButtonEnabled();
            }
        }
        /// <summary>
        /// 春秋苑会計の勘定科目か
        /// </summary>
        public bool IsShunjuen
        {
            get => isShunjuen;
            set
            {
                isShunjuen = value;
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
        /// 選択された勘定科目
        /// </summary>
        public AccountingSubject CurrentAccountingSubject
        {
            get => currentAccountingSubject;
            set
            {
                currentAccountingSubject = value;
                CallPropertyChanged();
                if (value == null) { AccountingSubjectDetailFieldClear(); }
                else
                {
                    AccountingSubjectIDField = value.ID;
                    AccountingSubjectCodeField = value.SubjectCode;
                    AccountingSubjectField = value.Subject;
                    IsAccountingSubjectValidity = value.IsValidity;
                    BranchNumber = DataBaseConnect.GetBranchNumber(value);
                    BranchNumberOperationContent = string.IsNullOrEmpty(BranchNumber) ? "登録" : "更新";
                    IsBranchNumberOperationVisibility = true;
                    SetAccountingSubjectOperationButtonEnabled();
                }
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
            IsAccountingSubjectOperationButtonEnabled =
                !string.IsNullOrEmpty(AccountingSubjectCodeField) &
                !string.IsNullOrEmpty(AccountingSubjectField);
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
                CreateAccountSubjects();
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
                if (referenceAccountingSubject == value) { return; }
                referenceAccountingSubjectCode = int.TryParse(value, out int i) ? i.ToString("000") : string.Empty;
                CallPropertyChanged();
                CreateAccountSubjects();
            }
        }

        private void CreateAccountSubjects()
        {
            AccountingSubjects =
                DataBaseConnect.ReferenceAccountingSubject
                (ReferenceAccountingSubjectCode, ReferenceAccountingSubject,
                    AccountingProcessLocation.IsAccountingGenreShunjuen, IsAccountingSubjectValidityTrueOnly);
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
                CreateAccountSubjects();
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
            AccountingSubject accountingSubject =
                new AccountingSubject(string.Empty, AccountingSubjectCodeField, AccountingSubjectField,
                    IsShunjuen, IsAccountingSubjectValidity);
            string branchNo = string.IsNullOrEmpty(BranchNumber) ? string.Empty :
                $"枝番\t\t:\t{BranchNumber}\r\n";
            if (CallConfirmationDataOperation
                ($"勘定科目コード\t:\t{accountingSubject.SubjectCode}\r\n" +
                $"{branchNo}" +
                $"勘定科目\t\t:\t{accountingSubject.Subject}" +
                $"\r\n有効性\t\t:\t{accountingSubject.IsValidity}" +
                $"\r\n会計\t\t:\t{(IsShunjuen ? SHUNJUEN : WIZECORE)}" +
                $"\r\n\r\n登録しますか？", "勘定科目") ==
                MessageBoxResult.Cancel) { return; }

            IsAccountingSubjectOperationButtonEnabled = false;
            AccountingSubnectOperationButtonContent = "登録中";
            await Task.Run(() => Registration());
            AccountingSubjectDetailFieldClear();
            AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject
                (string.Empty, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen,
                    IsAccountingSubjectValidityTrueOnly);
            AffiliationAccountingSubjects = AccountingSubjects;
            CallCompletedRegistration();
            IsAccountingSubjectOperationButtonEnabled = true;
            AccountingSubnectOperationButtonContent = "登録";
            
            void Registration()
            {
                _ = DataBaseConnect.Registration(accountingSubject);
                ObservableCollection<AccountingSubject> accountingSubjects =
                    DataBaseConnect.ReferenceAccountingSubject(AccountingSubjectCodeField, AccountingSubjectField, IsShunjuen, false);
                if (accountingSubjects.Count != 1)
                {
                    MessageBox = new MessageBoxInfo()
                    {
                        Message = "勘定科目が特定できなかったため、枝番は登録されませんでした。",
                        Title = "枝番登録エラー",
                        Image = MessageBoxImage.Exclamation,
                        Button = MessageBoxButton.OK
                    };
                    CallShowMessageBox = true;
                }
                else
                { BranchNumberRegistration(accountingSubjects[0]); }
            }
        }
        /// <summary>
        /// 枝番を登録します
        /// </summary>
        /// <param name="accountingSubject"></param>
        private void BranchNumberRegistration(AccountingSubject accountingSubject)
        {
            if (!string.IsNullOrEmpty(BranchNumber))
            { _ = DataBaseConnect.Registration(accountingSubject, BranchNumber); }
        }
        /// <summary>
        /// 勘定科目詳細フィールドをクリアします
        /// </summary>
        private void AccountingSubjectDetailFieldClear()
        {
            AccountingSubjectCodeField = string.Empty;
            AccountingSubjectField = string.Empty;
            BranchNumber = string.Empty;
            IsAccountingSubjectValidity = true;
            IsBranchNumberOperationVisibility = false;
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

            if (CallConfirmationDataOperation
                ($"有効性 : {CurrentAccountingSubject.IsValidity} → " +
                    $"{IsAccountingSubjectValidity}", "勘定科目") == MessageBoxResult.Cancel)
            { return; }
            CurrentAccountingSubject.IsValidity = IsAccountingSubjectValidity;
            _ = DataBaseConnect.Update(CurrentAccountingSubject);
            AccountingSubjectDetailFieldClear();
            CurrentAccountingSubject = null;
            SetDataUpdateCommand.Execute();
            CallCompletedUpdate();
        }
        #endregion
        #region CreditDeptOperation
        /// <summary>
        /// 貸方勘定ID
        /// </summary>
        public string CreditDeptIDField
        {
            get => creditDeptIDField;
            set
            {
                creditDeptIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定有効性
        /// </summary>
        public bool IsCreditDeptValidity
        {
            get => isCreditDeptValidity;
            set
            {
                isCreditDeptValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定
        /// </summary>
        public string CreditDeptField
        {
            get => creditDeptField;
            set
            {
                creditDeptField = value;
                CallPropertyChanged();
                ValidationProperty(nameof(CreditDeptField), value);
                IsCreditDeptOperationButtonEnabled = !string.IsNullOrEmpty(value);
            }
        }
        /// <summary>
        /// 貸方勘定データ操作ボタンのContent
        /// </summary>
        public string CreditDeptOperationButtonContent
        {
            get => creditDeptOperationButtonContent;
            set
            {
                creditDeptOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定データ操作ボタンのEnabled
        /// </summary>
        public bool IsCreditDeptOperationButtonEnabled
        {
            get => isCreditDeptOperationButtonEnabled;
            set
            {
                isCreditDeptOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定フィールドのEnabled
        /// </summary>
        public bool IsCreditDeptEnabled
        {
            get => isCreditDeptEnabled;
            set
            {
                isCreditDeptEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定登録のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledRegisterCreditDept()
        {
            IsCreditDeptValidity = true;
            IsCreditDeptEnabled = true;
            IsCreditDeptReferenceMenuEnabled = false;
            IsCreditDeptOperationButtonEnabled = false;
            IsShunjuenDept = true;
            CreditDeptField = string.Empty;
        }
        /// <summary>
        /// 貸方勘定更新のためのコントロールのEnableを設定します
        /// </summary>
        private void SetFieldEnabledUpdateCreditDept()
        {
            IsCreditDeptReferenceMenuEnabled = true;
            IsCreditDeptEnabled = false;
            IsCreditDeptOperationButtonEnabled = false;
        }
        /// <summary>
        /// 貸方勘定データ操作コマンド
        /// </summary>
        public DelegateCommand CreditDeptDataOperationCommand { get; set; }
        /// <summary>
        /// 選択された貸方勘定
        /// </summary>
        public CreditDept CurrentCreditDept
        {
            get => currentCreditDept;
            set
            {
                currentCreditDept = value;
                CallPropertyChanged();
                if (value != null)
                {
                    CreditDeptIDField = value.ID;
                    CreditDeptField = value.Dept;
                    IsCreditDeptValidity = value.IsValidity;
                    IsShunjuenDept = value.IsShunjuenDept;
                }
            }
        }
        /// <summary>
        /// 貸方勘定検索メニューのEnable
        /// </summary>
        public bool IsCreditDeptReferenceMenuEnabled
        {
            get => isCreditDeptReferenceMenuEnabled;
            set
            {
                isCreditDeptReferenceMenuEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方勘定
        /// </summary>
        public string ReferenceCreditDept
        {
            get => referenceCreditDept;
            set
            {
                referenceCreditDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方勘定のValidity
        /// </summary>
        public bool IsCreditDeptValidityTrueOnly
        {
            get => isCreditDeptValidityTrueOnly;
            set
            {
                isCreditDeptValidityTrueOnly = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索した貸方勘定リスト
        /// </summary>
        public ObservableCollection<CreditDept> CreditDepts
        {
            get => creditDepts;
            set
            {
                creditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定データを登録、更新します
        /// </summary>
        private void CreditDeptDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    CreditDeptDataRegistration();
                    break;
                case DataOperation.更新:
                    CreditDeptDataUpdate();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 貸方勘定データを登録します
        /// </summary>
        private async void CreditDeptDataRegistration()
        {
            CurrentCreditDept = new CreditDept(null, CreditDeptField, IsCreditDeptValidity, IsShunjuenDept);

            if (CallConfirmationDataOperation
                ($"貸方勘定 : {CreditDeptField}\r\n有効性 : {IsCreditDeptValidity}\r\n" +
                    $"春秋苑会計：{IsShunjuenDept}", "貸方勘定") == MessageBoxResult.Cancel) { return; }

            CreditDeptOperationButtonContent = "登録中";
            IsCreditDeptOperationButtonEnabled = false;
            _ = await Task.Run(() => DataBaseConnect.Registration(CurrentCreditDept));
            CreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, IsCreditDeptValidityTrueOnly, AccountingProcessLocation.IsAccountingGenreShunjuen);
            CreditDeptFieldClear();
            CallCompletedRegistration();
            CreditDeptOperationButtonContent = "登録";
            IsCreditDeptOperationButtonEnabled = true;
        }

        private void CreditDeptFieldClear()
        {
            CreditDeptField = string.Empty;
            IsCreditDeptValidity = true;
        }
        /// <summary>
        /// 貸方勘定データを更新します
        /// </summary>
        private async void CreditDeptDataUpdate()
        {
            if (IsCreditDeptValidity == CurrentCreditDept.IsValidity)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            CreditDeptOperationButtonContent = "更新中";
            IsCreditDeptOperationButtonEnabled = false;
            _ = await Task.Run(() => _ = DataBaseConnect.Update(CurrentCreditDept));
            CallCompletedUpdate();
            IsCreditDeptOperationButtonEnabled = true;
            CreditDeptOperationButtonContent = "更新";
        }
        #endregion
        #region ContentOperation

        /// <summary>
        /// 伝票内容規定の貸方部門を削除するコマンド
        /// </summary>
        public DelegateCommand DeleteContentDefaultCreditDeptCommand { get; set; }
        private async void DeleteContentDefaultCreditDept()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = $"{CurrentContent.Text}{Space}の規定貸方部門{Space}" +
                    $"{SelectedContentDefaultCreditDept.Dept}" +
                    $"\r\n\r\n削除しますか？",
                Image = MessageBoxImage.Question,
                Button = MessageBoxButton.OKCancel,
                Title = "削除確認"
            };
            CallShowMessageBox = true;

            if (MessageBox.Result != MessageBoxResult.OK) { return; }

            IsContentDefaultCreditDeptEnabled = false;
            _ = await Task.Run
                (() => DataBaseConnect.DeleteContentDefaultCreditDept(CurrentContent));
            SelectedContentDefaultCreditDept = null;
            IsContentDefaultCreditDeptEnabled = true;
        }
        /// <summary>
        /// 伝票内容規定の貸方部門データを操作するコマンド
        /// </summary>
        public DelegateCommand ContentDefaultCreditDeptOperationCommand { get; set; }
        private void ContentDefaultCreditDeptOperation()
        {
            if (SelectedContentDefaultCreditDept == null)
            {
                MessageBox = new MessageBoxInfo()
                {
                    Title = "データ操作エラー",
                    Message = "貸方部門が選択されていないので操作できません。",
                    Image = MessageBoxImage.Warning,
                    Button = MessageBoxButton.OK
                };
                CallShowMessageBox = true;
                return;
            }
            if (IsContentDefaultCreditDeptRegistration) { ContentDefaultCreditDeptRegistration(); }
            else { ContentDefaultCreditDeptUpdate(); }
        }
        private async void ContentDefaultCreditDeptRegistration()
        {
            if (CallConfirmationDataOperation($"既定の貸方部門{Space}:{Space}" +
                    $"{SelectedContentDefaultCreditDept.Dept}\r\n\r\n" +
                    $"登録しますか？", "伝票内容規定の貸方部門") == MessageBoxResult.Cancel)
            { return; }

            IsContentDefaultCreditDeptEnabled = false;
            ContentDefaultDeptOperationContent = "登録中";
            _ = await Task.Run(() =>
                DataBaseConnect.Registration(SelectedContentDefaultCreditDept, CurrentContent));
            IsContentDefaultCreditDeptRegistration = false;
            IsContentDefaultCreditDeptEnabled = true;
            CallCompletedRegistration();
        }
        private async void ContentDefaultCreditDeptUpdate()
        {
            string updateContent = string.Empty;

            if (DataBaseConnect.CallContentDefaultCreditDept(CurrentContent) !=
                SelectedContentDefaultCreditDept)
            {
                updateContent = $"規定の貸方部門{Space}:{Space}" +
                      $"{DataBaseConnect.CallContentDefaultCreditDept(CurrentContent).Dept}{Space}→{Space}" +
                      $"{SelectedContentDefaultCreditDept.Dept}";
            }

            if (string.IsNullOrEmpty(updateContent))
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            if (CallConfirmationDataOperation
                ($"{updateContent}\r\n\r\n更新しますか？", "既定の貸方部門") != MessageBoxResult.OK)
            { return; }

            IsContentDefaultCreditDeptEnabled = false;
            ContentDefaultDeptOperationContent = "更新中";
            _ = await Task.Run(() =>
                DataBaseConnect.Update(SelectedContentDefaultCreditDept, CurrentContent));
            IsContentDefaultCreditDeptRegistration = false;
            IsContentDefaultCreditDeptEnabled = true;
            CallCompletedUpdate();
        }
        /// <summary>
        /// 受納証に表示する内容を削除するコマンド
        /// </summary>
        public DelegateCommand DeleteContentConvertVoucherCommand { get; set; }
        private async void DeleteContentConertVoucher()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = $"受納証表記内容{Space}:{Space}{CurrentContent.Text}{Space}を{Space}" +
                    $"{DataBaseConnect.CallContentConvertText(CurrentContent.ID)}{Space}" +
                    $"で表記するデータ\r\n\r\n削除してよろしいですか？",
                Image = MessageBoxImage.Question,
                Button = MessageBoxButton.YesNo,
                Title = "受納証表記データ削除"
            };
            CallShowMessageBox = true;

            if (MessageBox.Result != MessageBoxResult.Yes) { return; }
            IsContentConvertOperationButtonEnabled = false;
            _ = await Task.Run(() => DataBaseConnect.DeleteConvertContent(CurrentContent));
            ContentConvertText = string.Empty;
        }
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
                IsAffiliationAccountingSubjectEnabled = value.Count != 0;
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
                CallPropertyChanged();
                ValidationProperty(nameof(AffiliationAccountingSubject), value);
                if (affiliationAccountingSubject != null)
                {
                    SelectedAccountingSubjectField = affiliationAccountingSubject.Subject;
                    AffiliationAccountingSubjectCode = value.SubjectCode;
                    SetContentOperationButtonEnabled();
                }
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
                CallPropertyChanged();
                ValidationProperty(nameof(ContentField), value);
                SetContentOperationButtonEnabled();
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
                string s = value.Replace(",", string.Empty);
                flatRateField = int.TryParse(s, out int i) ? CommaDelimitedAmount(i) : string.Empty;
                flatRateField = (i == 0) ? string.Empty : CommaDelimitedAmount(i);
                if (value == "-") { flatRateField = value; }
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
                selectedAccountingSubjectField = value;
                CallPropertyChanged();
                ValidationProperty(nameof(SelectedAccountingSubjectField), selectedAccountingSubjectField);
                SetContentOperationButtonEnabled();
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
            if (!HasErrors)
            {
                IsContentOperationEnabled = true;
                return;
            }

            bool isContentDefaultCreditDeptCheck =
                (IsContentDefaultCreditDeptSetting && SelectedContentDefaultCreditDept != null) ||
                !IsContentDefaultCreditDeptSetting;

            IsContentOperationEnabled =
                affiliationAccountingSubject != null && !string.IsNullOrEmpty(ContentField) &&
                isContentDefaultCreditDeptCheck;
                
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
                CallPropertyChanged();
                if (currentContent == null) { ContentDetailFieldClear(); }
                else
                {
                    ContentIDField = currentContent.ID;
                    IsContentValidity = currentContent.IsValidity;
                    ContentField = currentContent.Text;
                    FlatRateField = currentContent.FlatRate == -1 ?
                        string.Empty : currentContent.FlatRate.ToString();
                    AffiliationAccountingSubjectCode = currentContent.AccountingSubject.SubjectCode;
                    IsContentConvertButtonVisibility = true;
                    IsContentDefaultCreditDeptButtonVisibility = true;
                    ContentConvertText = DataBaseConnect.CallContentConvertText(ContentIDField);
                    IsContentConvertVoucherRegistration = string.IsNullOrEmpty(ContentConvertText);
                    SelectedContentDefaultCreditDept =
                        DataBaseConnect.CallContentDefaultCreditDept(CurrentContent);
                    IsContentDefaultCreditDeptRegistration = SelectedContentDefaultCreditDept == null;
                }
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
                CreateContents();
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
                CreateContents();
            }
        }

        private void CreateContents()
        {
            Contents = DataBaseConnect.ReferenceContent(ReferenceContent,
                ReferenceAccountingSubjectCodeBelognsContent, ReferenceAccountingSubjectBelognsContent,
                AccountingProcessLocation.IsAccountingGenreShunjuen, IsContentValidityTrueOnly);
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
        public bool IsShunjuenDept
        {
            get => isShunjuenDept;
            set
            {
                isShunjuenDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容をグルーピングする勘定科目コード
        /// </summary>
        public string AffiliationAccountingSubjectCode
        {
            get => affiliationAccountingSubjectCode;
            set
            {
                if (affiliationAccountingSubjectCode == value) { return; }
                affiliationAccountingSubjectCode = value;
                CallPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    AffiliationAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(value, string.Empty,
                            AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                    AffiliationAccountingSubject = AffiliationAccountingSubjects.Count == 0 ?
                        null : AffiliationAccountingSubjects[0];
                    ReferenceAccountingSubjectCodeBelognsContent = value;
                }
                else
                {
                    AffiliationAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty,
                            AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                    AffiliationAccountingSubject = null;
                }
            }
        }
        /// <summary>
        /// 伝票内容検索で使用する勘定科目コード
        /// </summary>
        public string ReferenceAccountingSubjectCodeBelognsContent
        {
            get => referenceAccountingSubjectCodeBelognsContent;
            set
            {
                referenceAccountingSubjectCodeBelognsContent = value;
                CallPropertyChanged();
                CreateContents();
            }
        }
        /// <summary>
        /// 伝票内容検索で使用する勘定科目
        /// </summary>
        public string ReferenceAccountingSubjectBelognsContent
        {
            get => referenceAccountingSubjectBelognsContent;
            set
            {
                referenceAccountingSubjectBelognsContent = value;
                CallPropertyChanged();
                CreateContents();
            }
        }
        /// <summary>
        /// 受納証用ContentConvertデータ操作ボタンのVisiblity
        /// </summary>
        public bool IsContentConvertButtonVisibility
        {
            get => isContentConvertButtonVisibility;
            set
            {
                isContentConvertButtonVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証の但し書き文字列
        /// </summary>
        public string ContentConvertText
        {
            get => contentConvertText;
            set
            {
                contentConvertText = value;
                CallPropertyChanged();
                IsContentConvertOperationButtonEnabled = !string.IsNullOrEmpty(value);
            }
        }
        /// <summary>
        /// 受納証用文字列データを登録、更新します
        /// </summary>
        public DelegateCommand ContentConvertVoucherOperationCommand { get; set; }
        private async void ContentConvertVoucherOperation()
        {
            ContentConvertOperationContent = "書込中";
            IsContentConvertOperationButtonEnabled = false;
            _ = IsContentConvertVoucherRegistration
                ? await Task.Run(() =>
                _ = DataBaseConnect.Registration(ContentIDField, ContentConvertText))
                : await Task.Run(() =>
                _ = DataBaseConnect.Update(ContentIDField, ContentConvertText));
            IsContentConvertVoucherRegistration = false;
            IsContentConvertOperationButtonEnabled = true;
        }
        /// <summary>
        /// 受納証の但し書きデータ操作が登録か更新か。登録ならtrue
        /// </summary>
        private bool IsContentConvertVoucherRegistration
        {
            get => isContentConvertVoucherRegistration;
            set
            {
                ContentConvertOperationContent = value ? "登録" : "更新";
                isContentConvertVoucherRegistration = value;
            }
        }
        /// <summary>
        /// 受納証の但し書き文字列操作ボタンのContent
        /// </summary>
        public string ContentConvertOperationContent
        {
            get => contentConvertOperationContent;
            set
            {
                contentConvertOperationContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証但し書き文字列操作ボタンのEnabled
        /// </summary>
        public bool IsContentConvertOperationButtonEnabled
        {
            get => isContentConvertOperationButtonEnabled;
            set
            {
                isContentConvertOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容に設定する既定の貸方部門のリスト
        /// </summary>
        public ObservableCollection<CreditDept> ContentDefaultCreditDepts
        {
            get => contentDefaultCreditDepts;
            set
            {
                contentDefaultCreditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票内容規定の貸方部門
        /// </summary>
        public CreditDept SelectedContentDefaultCreditDept
        {
            get => selectedContentDefaultCreditDept;
            set
            {
                selectedContentDefaultCreditDept = value;
                CallPropertyChanged();
                SetContentOperationButtonEnabled();
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門のデータ操作ボタンのContent
        /// </summary>
        public string ContentDefaultDeptOperationContent
        {
            get => contentDefaultDeptOperationContent;
            set
            {
                contentDefaultDeptOperationContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門のデータ操作ボタンのVisibility
        /// </summary>
        public bool IsContentDefaultCreditDeptButtonVisibility
        {
            get => isContentDefaultCreditDeptButtonVisibility;
            set
            {
                isContentDefaultCreditDeptButtonVisibility = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門の設定をするか
        /// </summary>
        public bool IsContentDefaultCreditDeptSetting
        {
            get => isContentDefaultCreditDeptSetting;
            set
            {
                isContentDefaultCreditDeptSetting = value;
                CallPropertyChanged();
                SetContentOperationButtonEnabled();
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門データ操作が登録か更新か　登録ならTrue
        /// </summary>
        public bool IsContentDefaultCreditDeptRegistration
        {
            get => isContentDefaultCreditDeptRegistration;
            set
            {
                ContentDefaultDeptOperationContent = value ? "登録" : "更新";
                isContentDefaultCreditDeptRegistration = value;
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門データ操作ボタンのEnabled
        /// </summary>
        public bool IsContentDefaultCreditDeptEnabled
        {
            get => isContentDefaultCreditDeptEnabled;
            set
            {
                isContentDefaultCreditDeptEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容規定の貸方部門を操作するかのチェックのEnabled
        /// </summary>
        public bool IsContentDefaultCreditDeptSettingEnabled
        {
            get => isContentDefaultCreditDeptSettingEnabled;
            set
            {
                isContentDefaultCreditDeptSettingEnabled = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 伝票内容データを登録、更新します
        /// </summary>
        private void ContentDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    ContentRegistration();
                    break;
                case DataOperation.更新:
                    ContentUpdate();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 伝票内容データを登録します
        /// </summary>
        private async void ContentRegistration()
        {
            int i = (!int.TryParse(FlatRateField.Replace(",", string.Empty), out int j)) ? -1 : j;
            CreditDept defaultCreditDept = SelectedContentDefaultCreditDept;
            CurrentContent = new Content(string.Empty, AffiliationAccountingSubject, i, ContentField, IsContentValidity);
            string flatRateInfo = (i != -1) ? AmountWithUnit(i) : "金額設定無し";
            string confirmationVoucherText = string.IsNullOrEmpty(ContentConvertText) ?
                string.Empty : $"\r\n受納証但し書き{Space}:{Space}{ContentConvertText}";
            string confirmationDefaultCreditDept = defaultCreditDept == null ?
                string.Empty : $"\r\n既定の貸方部門{Space}:{Space}{defaultCreditDept.Dept}";

            if (CallConfirmationDataOperation($"伝票内容 : {CurrentContent.Text}\r\n" +
                $"勘定科目 : {CurrentContent.AccountingSubject.Subject}\r\n" +
                $"定額 : {flatRateInfo}\r\n有効性 : {CurrentContent.IsValidity}{confirmationVoucherText}" +
                $"{confirmationDefaultCreditDept}\r\n\r\n登録しますか？", "伝票内容")
                == MessageBoxResult.Cancel)
            {
                IsContentDefaultCreditDeptButtonVisibility = false;
                return;
            }

            ContentDataOperationContent = "登録中";
            IsContentOperationEnabled = false;

            await Task.Run(() => RegistrationContentAndOptionPropety());

            CreateContents();
            ContentDetailFieldClear();
            CallCompletedRegistration();
            IsContentOperationEnabled = true;
            ContentDataOperationContent = "登録";

            void RegistrationContentAndOptionPropety()
            {
                _ = DataBaseConnect.Registration(CurrentContent);
                ObservableCollection<Content> contents = DataBaseConnect.ReferenceContent
                    (CurrentContent.Text, CurrentContent.AccountingSubject.SubjectCode, 
                        CurrentContent.AccountingSubject.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, true);

                if (!string.IsNullOrEmpty(ContentConvertText)) { RegistrationContentConvertText(); }

                if (IsContentDefaultCreditDeptSetting)
                { _ = DataBaseConnect.Registration(defaultCreditDept, contents[0]); }

                void RegistrationContentConvertText()
                {
                    if (contents.Count > 1)
                    {
                        MessageBox = new MessageBoxInfo()
                        {
                            Message = $"登録内容が重複していませんか？確認してください\r\n" +
                            $"伝票内容が一つに絞り込めなかったため、受納証但し書きデータは登録されません。",
                            Image = MessageBoxImage.Warning,
                            Title = "受納証但し書きデータが登録できませんでした",
                            Button = MessageBoxButton.OK
                        };
                        { return; }

                    }
                    string id = contents[0].ID;

                    _ = DataBaseConnect.Registration(id, ContentConvertText);
                }
            }
        }
        /// <summary>
        /// 詳細に表示された伝票内容をクリアします
        /// </summary>
        private void ContentDetailFieldClear()
        {
            if (CurrentContent != null) { CurrentContent = null; }
            ContentIDField = string.Empty;
            AffiliationAccountingSubject = null;
            AffiliationAccountingSubjectCode = string.Empty;
            FlatRateField = string.Empty;
            ContentField = string.Empty;
            IsContentValidity = true;
            IsContentConvertButtonVisibility = false;
            IsContentDefaultCreditDeptButtonVisibility = false;
            IsContentDefaultCreditDeptSetting = false;
            SelectedContentDefaultCreditDept = null;
            IsContentDefaultCreditDeptSettingEnabled = true;
        }
        /// <summary>
        /// 伝票内容データを更新します
        /// </summary>
        private async void ContentUpdate()
        {
            string updateContents = string.Empty;

            int oldFlatRate = CurrentContent.FlatRate;
            if (oldFlatRate < 0) { oldFlatRate = 0; }

            int newFlatRate = IntAmount(FlatRateField);
            if (newFlatRate < 0) { newFlatRate = 0; }

            int i = CurrentContent.FlatRate;
            string currentFlatRateInfo = (i != -1) ? AmountWithUnit(i) : "金額設定無し";

            i = (!int.TryParse(FlatRateField.Replace(",", string.Empty), out int j)) ? -1 : j;
            string newFlatRateInfo = (i != -1) ? AmountWithUnit(i) : "金額設定無し";

            if (oldFlatRate != newFlatRate)
            {
                updateContents =
                    $"定額 : {currentFlatRateInfo} → {newFlatRateInfo}\r\n";
            }
            if (CurrentContent.IsValidity != IsContentValidity)
            {
                updateContents +=
                    $"有効性 : {CurrentContent.IsValidity} → {IsContentValidity}\r\n";
            }

            if (updateContents.Length == 0)
            {
                CallNoRequiredUpdateMessage();
                return;
            }
            if (CallConfirmationDataOperation
                ($"伝票内容 : {ContentField}\r\n\r\n{updateContents}\r\n\r\n更新しますか？", "伝票内容") ==
                MessageBoxResult.Cancel) { return; }

            i = (!int.TryParse(FlatRateField.Replace(",", string.Empty), out j)) ? -1 : j;
            CurrentContent =
                new Content(ContentIDField, AffiliationAccountingSubject, i, ContentField, IsContentValidity);

            IsContentOperationEnabled = false;
            ContentDataOperationContent = "更新中";
            _ = await Task.Run(() => DataBaseConnect.Update(CurrentContent));
            ContentDetailFieldClear();
            CreateContents();
            CallCompletedUpdate();
            ContentDataOperationContent = "更新";
            IsContentOperationEnabled = true;
        }
        #endregion

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(RepName):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(RepCurrentPassword):
                    if (RepDataOperationButtonContent != "登録")
                    {
                        ErrorsListOperation
                            (RepCurrentPassword != CurrentRep.Password, propertyName,
                                Properties.Resources.PasswordErrorInfo);
                        if (string.IsNullOrEmpty((string)value)) { break; }
                        if (GetErrors(propertyName) != null)
                        {
                            ErrorsListOperation
                                (GetHashValue(RepCurrentPassword, CurrentRep.ID) !=
                                    CurrentRep.Password, propertyName, Properties.Resources.PasswordErrorInfo);
                        }
                    }
                    break;
                case nameof(RepNewPassword):
                    if (IsRepNewPasswordEnabled) { SetNullOrEmptyError(propertyName, value); }
                    break;
                case nameof(ConfirmationPassword):
                    if (!_isRepNewPasswordEnabled) { break; }
                    SetNullOrEmptyError(propertyName, value);
                    if (GetErrors(propertyName) == null)
                    {
                        ErrorsListOperation
                            (RepNewPassword != ConfirmationPassword, propertyName,
                                Properties.Resources.PasswordErrorInfo);
                    }
                    break;
                case nameof(AccountingSubjectCodeField):
                    SetNullOrEmptyError(propertyName, value);
                    if (GetErrors(propertyName) == null)
                    {
                        ErrorsListOperation(AccountingSubjectCodeField.Length != 3, propertyName,
                            "コードの桁数が不正です");
                    }
                    break;
                case nameof(AccountingSubjectField):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(ReferenceAccountingSubjectCode):
                    SetNullOrEmptyError(propertyName, value);
                    if (GetErrors(propertyName) == null)
                    {
                        ErrorsListOperation(ReferenceAccountingSubjectCode.Length != 3, propertyName,
                            "コードの桁数が不正です");
                    }
                    break;
                case nameof(BranchNumber):
                    
                    break;
                case nameof(CreditDeptField):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(SelectedAccountingSubjectField):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(ContentField):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                default:
                    break;
            }
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "データ管理"; }
    }
}