using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;
using System.Threading.Tasks;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    public class TransferReceiptsAndExpenditureOperationViewModel : DataOperationViewModel, IClosing
    {
        #region Properties
        private int iD;
        #region Strings
        private string debitAccountCode = string.Empty;
        private string creditAccountCode = string.Empty;
        private string detailText;
        private string dept;
        private string price;
        private string dataOperationButtonContent;
        private string iDFieldValue;
        #endregion
        #region Bools
        private bool isValidity;
        private bool isReducedTaxRate;
        private bool isValidityEnabled;
        private bool canOperation = false;
        private bool CanClosing = true;
        private bool isOutputted;
        #endregion
        #region Dates
        private DateTime accountActivityDate;
        private DateTime outputDate;
        private DateTime registrationDate;
        #endregion
        #region ObservableCollections
        private ObservableCollection<AccountingSubject> debitAccounts;
        private ObservableCollection<AccountingSubject> creditAccounts;
        private ObservableCollection<Content> contents;
        private ObservableCollection<CreditDept> creditDepts;
        private ObservableCollection<Rep> reps;
        #endregion
        #region ValueObjects
        private CreditDept selectedCreditDept;
        private AccountingSubject debitAccount;
        private AccountingSubject creditAccount;
        private Content selectedContent;
        private Rep operationRep;
        #endregion
        #endregion

        public TransferReceiptsAndExpenditureOperationViewModel(IDataBaseConnect dataBaseConnect) :
            base(dataBaseConnect)
        {
            SetDelegateCommand();
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null)
            {
                SetDataOperation(DataOperation.登録);
                 DataOperationButtonContent = DataOperation.登録.ToString();
               FieldClear();
            }
            else
            {
                SetDataOperation(DataOperation.更新);
                DataOperationButtonContent = DataOperation.更新.ToString();
                SetProperty();
            }
        }
        public TransferReceiptsAndExpenditureOperationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 000を追加するコマンド
        /// </summary>
        public DelegateCommand ZeroAddCommand { get; set; }
        private void ZeroAdd()
        {
            int i = IntAmount(Price);
            i *= 1000;
            Price = CommaDelimitedAmount(i);
        }
        /// <summary>
        /// 振替伝票データプロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null) { return; }

            TransferReceiptsAndExpenditure trae = 
                TransferReceiptsAndExpenditureOperation.GetInstance().GetData();
            ID = trae.ID;
            IsValidity = trae.IsValidity;
            AccountActivityDate = trae.AccountActivityDate;
            DebitAccountCode = trae.DebitAccount.SubjectCode;
            DebitAccount = trae.DebitAccount;
            CreditAccountCode = trae.CreditAccount.SubjectCode;
            CreditAccount = trae.CreditAccount;
            ObservableCollection<Content> contents = DataBaseConnect.ReferenceContent
                (trae.ContentText, DebitAccountCode, DebitAccount.Subject,
                    AccountingProcessLocation.IsAccountingGenreShunjuen, false);

            ObservableCollection<Content> credits = DataBaseConnect.ReferenceContent
                   (trae.ContentText, CreditAccountCode, CreditAccount.Subject,
                       AccountingProcessLocation.IsAccountingGenreShunjuen, false);

            foreach(Content c in credits) { contents.Add(c); }

            if (contents.Count == 0)
            {
                DataOperationButtonContent = "内容が無効です";
                return;
            }
            Content content = null;

            foreach(Content c in contents) { content = trae.ContentText == c.Text ? c : null; }

            if(content == null)
            {
                DataOperationButtonContent = "内容が無効です";
                return;
            }

            int i = Contents.IndexOf(content);
            if (i < 0)
            {
                content = DataBaseConnect.ReferenceContent
                    (trae.ContentText, CreditAccountCode, CreditAccount.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, false)[0];
                i = Contents.IndexOf(content);
            }
            SelectedContent = Contents[i];
            Price = trae.Price.ToString();
            DetailText = trae.Detail;
            IsReducedTaxRate = trae.IsReducedTaxRate;
            RegistrationDate = trae.RegistrationDate;
            OperationRep = trae.RegistrationRep;            
            OutputDate = trae.OutputDate;
            IsOutputted = OutputDate != DefaultDate;
            SelectedCreditDept = trae.CreditDept;
            Dept = SelectedCreditDept.Dept;
        }
        /// <summary>
        /// フィールドをクリアします
        /// </summary>
        private void FieldClear()
        {
            CanOperation = false;
            IDFieldValue = string.Empty;
            IsValidity = true;
            AccountActivityDate = DateTime.Now;
            OutputDate = DefaultDate;
            DebitAccountCode = string.Empty;
            CreditAccountCode = string.Empty;
            DetailText = string.Empty;
            Price = string.Empty;
            RegistrationDate = DateTime.Now;
            OperationRep = LoginRep.GetInstance().Rep;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand OperationDataCommand { get; set; }
        private void OperationData()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    DataRegistration();
                    break;
                case DataOperation.更新:
                    DataUpdate();
                    break;
                default:
                    break;
            }
        }
        private async void DataRegistration()
        {
            CanOperation = false;
            CanClosing = false;
            TransferReceiptsAndExpenditure trae = new TransferReceiptsAndExpenditure
                (0, DateTime.Now, LoginRep.GetInstance().Rep, AccountingProcessLocation.Location.ToString(),
                    SelectedCreditDept, DebitAccount, CreditAccount, SelectedContent.Text, DetailText, 
                    IntAmount(Price), IsValidity, AccountActivityDate, OutputDate, IsReducedTaxRate);
            string debitBranchNumber =
                string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(DebitAccount)) ?
                string.Empty : $"-{DataBaseConnect.GetBranchNumber(DebitAccount)}";
            string creditBranchNumber =
                string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(CreditAccount)) ?
                string.Empty : $"-{DataBaseConnect.GetBranchNumber(CreditAccount)}";

            if (CallConfirmationDataOperation
                ( $"経理担当場所\t : {trae.Location}\r\n" +
                    $"振替日\t\t : {trae.AccountActivityDate.ToShortDateString()}\r\n" +
                    $"貸方部門\t\t：{trae.CreditDept.Dept}\r\n" +
                    $"借方勘定コード\t：{trae.DebitAccount.SubjectCode}{debitBranchNumber}\r\n" +
                    $"借方勘定科目\t：{trae.DebitAccount.Subject}\r\n" +
                    $"貸方勘定コード\t : {trae.CreditAccount.SubjectCode}{creditBranchNumber}\r\n" +
                    $"貸方勘定科目\t : {trae.CreditAccount.Subject}\r\n" +
                    $"内容\t\t : {trae.ContentText}\r\n" +
                    $"詳細\t\t : {trae.Detail}\r\n金額\t\t : {AmountWithUnit(trae.Price)}\r\n" +
                    $"軽減税率\t\t : {trae.IsReducedTaxRate}\r\n" +
                    $"有効性\t\t : {trae.IsValidity}\r\n\r\n登録しますか？", "振替伝票")                    
                == System.Windows.MessageBoxResult.Cancel)
            {
                CanOperation = true;
                CanClosing = true;
                return;
            }

            DataOperationButtonContent = "登録中";
            _ = await Task.Run(() => _ = DataBaseConnect.Registration(trae));
            CallShowMessageBox = true;
            CallCompletedRegistration();
            SetOperationData();
            DataOperationButtonContent = "登録";
            CanClosing = true;

            void SetOperationData()
            {
                TransferReceiptsAndExpenditureOperation.GetInstance().SetData(trae);
                TransferReceiptsAndExpenditureOperation.GetInstance().Notify();
                FieldClear();
            }
        }
        private async void DataUpdate()
        {
            CanOperation = false;
            CanClosing= false;

            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null) { return; }

            string UpdateCotent = string.Empty;
            TransferReceiptsAndExpenditure trae =
                TransferReceiptsAndExpenditureOperation.GetInstance().GetData();

            if (trae.AccountActivityDate != AccountActivityDate)
            {
                UpdateCotent +=
                    $"入出金日：{trae.AccountActivityDate.ToShortDateString()}{Space}→{Space}" +
                    $"{AccountActivityDate.ToShortDateString()}\r\n";
            }

            if (trae.CreditDept.Dept != SelectedCreditDept.Dept)
            {
                UpdateCotent +=
                    $"貸方勘定：{trae.CreditDept.Dept}{Space}→{Space}{SelectedCreditDept.Dept}\r\n";
            }

            if (trae.DebitAccount.SubjectCode != DebitAccountCode)
            {
                string updateBranchNumber =
                    string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(DebitAccount)) ?
                    string.Empty : $"-{DataBaseConnect.GetBranchNumber(DebitAccount)}";
                string originalBranchNumber =
                    string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(trae.DebitAccount)) ?
                    string.Empty : $"-{DataBaseConnect.GetBranchNumber(CreditAccount)}";
                UpdateCotent +=
                    $"借方勘定科目コード：{trae.DebitAccount.SubjectCode}{originalBranchNumber}" +
                    $"{Space}→{Space}{DebitAccountCode}{updateBranchNumber}\r\n";
            }

            if (trae.DebitAccount.Subject != DebitAccount.Subject)
            {
                UpdateCotent += $"借方勘定科目：{trae.DebitAccount.Subject}{Space}→{Space}" +
                    $"{DebitAccount.Subject}\r\n";
            }

            if (trae.CreditAccount.SubjectCode != CreditAccountCode)
            {
                string originalBranchNumber = 
                    string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(trae.CreditAccount)) ?
                    string.Empty : $"-{DataBaseConnect.GetBranchNumber(DebitAccount)}";
                string updateBranchNumber =
                    string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(CreditAccount)) ?
                    string.Empty : $"-{DataBaseConnect.GetBranchNumber(CreditAccount)}";
                UpdateCotent +=
                    $"貸方勘定科目コード：{trae.CreditAccount.SubjectCode}{originalBranchNumber}" +
                    $"{Space}→{Space}{CreditAccountCode}{updateBranchNumber}\r\n";
            }

            if (trae.CreditAccount.Subject != CreditAccount.Subject)
            {
                UpdateCotent += $"貸方勘定科目：{trae.CreditAccount.Subject}{Space}→{Space}" +
                    $"{CreditAccount.Subject}\r\n";
            }

            if (trae.ContentText != SelectedContent.Text)
            { UpdateCotent += $"内容：{trae.ContentText}{Space}→{Space}{SelectedContent.Text}\r\n"; }

            if (trae.Detail != DetailText)
            { UpdateCotent += $"詳細：{trae.Detail}{Space}→{Space}{DetailText}\r\n"; }

            if (trae.Price != IntAmount(Price))
            {
                UpdateCotent += $"金額：{AmountWithUnit(trae.Price)}{Space}→{Space}" +
                    $"{AmountWithUnit(IntAmount(Price))}\r\n";
            }

            if (trae.IsValidity != IsValidity)
            { UpdateCotent += $"有効性：{trae.IsValidity}{Space}→{Space}{IsValidity}\r\n"; }

            if (trae.OutputDate != OutputDate)
            {
                UpdateCotent += $"出力日：{trae.OutputDate.ToShortDateString()}{Space}→{Space}" +
                    $"{(OutputDate == DefaultDate ? "未出力" : OutputDate.ToShortDateString())}\r\n";
            }

            if (trae.IsReducedTaxRate != IsReducedTaxRate)
            {
                UpdateCotent +=
                    $"軽減税率データ：{trae.IsReducedTaxRate}{Space}→{Space}{IsReducedTaxRate}\r\n";
            }

            if (UpdateCotent.Length == 0)
            {
                CallNoRequiredUpdateMessage();
                CanOperation = true;
                return;
            }

            if (CallConfirmationDataOperation
                ($"{UpdateCotent}\r\n\r\n更新しますか？", "伝票") ==
                System.Windows.MessageBoxResult.Cancel)
            {
                SetProperty();
                CanOperation = true;
                CanClosing = true;
                return;
            }

            TransferReceiptsAndExpenditure updateData = new TransferReceiptsAndExpenditure
                (ID, RegistrationDate, OperationRep,
                    TransferReceiptsAndExpenditureOperation.GetInstance().GetData().Location,
                    SelectedCreditDept, DebitAccount,CreditAccount, SelectedContent.Text, DetailText, 
                    IntAmount(Price), IsValidity, AccountActivityDate, OutputDate, IsReducedTaxRate);

            DataOperationButtonContent = "更新中";
            _ = await Task.Run(() => DataBaseConnect.Update(updateData));
            TransferReceiptsAndExpenditureOperation.GetInstance().SetData(updateData);
            TransferReceiptsAndExpenditureOperation.GetInstance().Notify();
            CallCompletedUpdate();
            DataOperationButtonContent = "更新";

            CanOperation = true;
        }
        /// <summary>
        /// 有効性
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
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
                CallPropertyChanged();
                ValidationProperty(nameof(AccountActivityDate), value);
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
                ValidationProperty(nameof(SelectedCreditDept), value);
            }
        }
        /// <summary>
        /// 選択された借方、貸方勘定科目にグルーピングされた伝票内容をリストにします
        /// </summary>
        private void SetContents()
        {
            ObservableCollection<Content> debitList;
            ObservableCollection<Content> creditList;
            ObservableCollection<Content> List = new ObservableCollection<Content>();

            if (DebitAccount != null)
            {
                debitList = DataBaseConnect.ReferenceContent
                    (string.Empty, DebitAccount.SubjectCode, DebitAccount.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                foreach (Content content in debitList) { List.Add(content); }
            }
            
            if (CreditAccount != null)
            {
                creditList = DataBaseConnect.ReferenceContent
                    (string.Empty, CreditAccount.SubjectCode, CreditAccount.Subject,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                foreach (Content content in creditList) { List.Add(content); }
            }

            Contents = List;
        }
        /// <summary>
        /// 借方勘定科目
        /// </summary>
        public AccountingSubject DebitAccount
        {
            get => debitAccount;
            set
            {
                debitAccount = value;
                CallPropertyChanged();
                ValidationProperty(nameof(DebitAccount), value);
                SetContents();
            }
        }
        /// <summary>
        /// 借方勘定科目コード
        /// </summary>
        public string DebitAccountCode
        {
            get => debitAccountCode;
            set
            {
                debitAccountCode = value;
                CallPropertyChanged();
                ValidationProperty(nameof(DebitAccountCode), value);
                DebitAccounts = new ObservableCollection<AccountingSubject>();
                if (value.Length == 3)
                {
                    DebitAccounts = DataBaseConnect.ReferenceAccountingSubject
                        (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                }
                if (DebitAccounts.Count > 0) { DebitAccount = DebitAccounts[0]; }
                else { DebitAccount = null; }
            }
        }
        /// <summary>
        /// 借方勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> DebitAccounts
        {
            get => debitAccounts;
            set
            {
                debitAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定科目コード
        /// </summary>
        public string CreditAccountCode
        {
            get => creditAccountCode;
            set
            {
                creditAccountCode = value;
                CallPropertyChanged();
                ValidationProperty(nameof(CreditAccountCode), value);
                CreditAccounts = new ObservableCollection<AccountingSubject>();
                if (value.Length == 3)
                {
                    CreditAccounts = DataBaseConnect.ReferenceAccountingSubject
                        (value, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                }
                if (CreditAccounts.Count > 0) { CreditAccount = CreditAccounts[0]; }
                else { CreditAccount = null; }
            }
        }
        /// <summary>
        /// 貸方勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> CreditAccounts
        {
            get => creditAccounts;
            set
            {
                creditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定科目
        /// </summary>
        public AccountingSubject CreditAccount
        {
            get => creditAccount;
            set
            {
                creditAccount = value;
                CallPropertyChanged();
                ValidationProperty(nameof(CreditAccount), value);
                SetContents();
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
        /// 選択された伝票内容
        /// </summary>
        public Content SelectedContent
        {
            get => selectedContent;
            set
            {
                selectedContent = value;
                CallPropertyChanged();
                ValidationProperty(nameof(SelectedContent), value);
            }
        }
        /// <summary>
        /// 詳細
        /// </summary>
        public string DetailText
        {
            get => detailText;
            set
            {
                detailText = value.Replace('　',' ');
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額
        /// </summary>
        public string Price
        {
            get => price;
            set
            {
                price = CommaDelimitedAmount(value);
                CallPropertyChanged();
                ValidationProperty(nameof(Price), value);
            }
        }
        /// <summary>
        /// 軽減税率チェック
        /// </summary>
        public bool IsReducedTaxRate
        {
            get => isReducedTaxRate;
            set
            {
                isReducedTaxRate = value;
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
        /// 有効性チェックのEnabled
        /// </summary>
        public bool IsValidityEnabled
        {
            get => isValidityEnabled;
            set
            {
                isValidityEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作できるか
        /// </summary>
        public bool CanOperation
        {
            get => canOperation;
            set
            {
                canOperation = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門リスト
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
        /// 伝票出力日
        /// </summary>
        public DateTime OutputDate
        {
            get => outputDate;
            set
            {
                outputDate = value;
                CallPropertyChanged();
            }
        }
        private void SetOutputDate(bool value)
        {
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null)
            { OutputDate = DefaultDate; }
            OutputDate = value ?
                ReturnDate() : DefaultDate;

            static DateTime ReturnDate()
            {
                return TransferReceiptsAndExpenditureOperation.GetInstance().GetData().OutputDate ==
                    DefaultDate ? DateTime.Today : 
                    TransferReceiptsAndExpenditureOperation.GetInstance().GetData().OutputDate;
            }
        }
        /// <summary>
        /// ID表示文字列
        /// </summary>
        public string IDFieldValue
        {
            get => iDFieldValue;
            set
            {
                iDFieldValue = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get => iD;
            set
            {
                iD = value;
                CallPropertyChanged();
                IDFieldValue = $"ID：{value}";
            }
        }
        /// <summary>
        /// 登録日
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
        /// 登録担当者リスト
        /// </summary>
        public ObservableCollection<Rep> Reps
        {
            get => reps;
            set
            {
                reps = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作担当者
        /// </summary>
        public Rep OperationRep
        {
            get => operationRep;
            set
            {
                operationRep = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 印刷フラグ
        /// </summary>
        public bool IsOutputted
        {
            get => isOutputted;
            set
            {
                isOutputted = value;
                CallPropertyChanged();
                SetOutputDate(value);
            }
        }
        /// <summary>
        /// Operationデータがある時に貸方部門コンボボックスに値が表示されないので、その回避策。要検証
        /// </summary>
        public string Dept
        {
            get => dept;
            set
            {
                dept = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(SelectedCreditDept):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(AccountActivityDate):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(DebitAccountCode):
                    CodeValidation(propertyName, value);
                    break;
                case nameof(DebitAccount):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(CreditAccountCode):
                    CodeValidation(propertyName, value);
                    break;
                case nameof(CreditAccount):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(SelectedContent):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(Price):
                    ErrorsListOperation(IntAmount(Price) <= 0, propertyName, "金額は必ず入力してください");
                    break;
                default:
                    break;
            }

            void CodeValidation(string propertyName, object value)
            {
                SetNullOrEmptyError(propertyName, value);
                if (GetErrors(propertyName) == null)
                { ErrorsListOperation(value.ToString().Length != 3, propertyName, 
                    "コードは3桁にしてください"); }
            }

            CanOperation = !HasErrors && SelectedCreditDept != null && AccountActivityDate != null &&
                DebitAccountCode.Length == 3 && DebitAccount != null && CreditAccountCode.Length == 3 &&
                CreditAccount != null && SelectedContent != null && IntAmount(Price) > 0;
        }

        protected override void SetDataList()
        {
            if (SelectedCreditDept == null)
            {
                CreditDepts = DataBaseConnect.ReferenceCreditDept
                    (string.Empty, true, AccountingProcessLocation.IsAccountingGenreShunjuen);
            }
            Reps = DataBaseConnect.ReferenceRep(string.Empty, true);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        { DataOperationButtonContent = operation.ToString(); }

        protected override void SetDelegateCommand()
        {
            OperationDataCommand = new DelegateCommand(() => OperationData(), () => true);
            ZeroAddCommand = new DelegateCommand(() => ZeroAdd(), () => true);
        }

        protected override void SetDetailLocked() 
        { IsValidityEnabled = CurrentOperation == DataOperation.更新; }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "振替データ管理"; }

        public bool CancelClose() => !CanClosing;
    }
}
