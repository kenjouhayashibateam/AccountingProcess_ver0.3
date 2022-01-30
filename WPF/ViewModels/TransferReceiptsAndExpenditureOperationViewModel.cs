using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    public class TransferReceiptsAndExpenditureOperationViewModel : DataOperationViewModel
    {
        #region Properties
        #region Strings
        private string debitAccountCode;
        private string creditAccountCode;
        private string detailText;
        private string price;
        private string dataOperationButtonContent;
        #endregion
        #region Bools
        private bool isValidity;
        private bool isReducedTaxRate;
        private bool isValidityEnabled;
        private bool canOperation = false;
        #endregion
        #region Dates
        private DateTime accountActivityDate;
        #endregion
        #region ObservableCollections
        private ObservableCollection<AccountingSubject> debitAccounts;
        private ObservableCollection<AccountingSubject> creditAccounts;
        private ObservableCollection<Content> contents;
        private ObservableCollection<CreditDept> creditDepts;
        #endregion
        #region ValueObjects
        private CreditDept selectedCreditDept;
        private AccountingSubject debitAccount;
        private AccountingSubject creditAccount;
        private Content selectedContent;
        #endregion
        #endregion

        public TransferReceiptsAndExpenditureOperationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null)
            {
                FieldClear();
                SetDataOperation(DataOperation.登録);
                DataOperationButtonContent = DataOperation.登録.ToString();
            }
            else
            {
                SetProperty();
                SetDataOperation(DataOperation.更新);
                DataOperationButtonContent = DataOperation.更新.ToString();
            }
            SetDelegateCommand();
        }
        public TransferReceiptsAndExpenditureOperationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 振替伝票データプロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            if (TransferReceiptsAndExpenditureOperation.GetInstance().GetData() == null) { return; }

            TransferReceiptsAndExpenditure trae = TransferReceiptsAndExpenditureOperation.GetInstance().GetData();
            AccountActivityDate = trae.AccountActivityDate;
            SelectedCreditDept = trae.CreditDept;
            DebitAccountCode = trae.DebitAccount.SubjectCode;
            DebitAccount = trae.DebitAccount;
            CreditAccountCode = trae.CreditAccount.SubjectCode;
            CreditAccount = trae.CreditAccount;
            SelectedContent = DataBaseConnect.ReferenceContent
                (trae.ContentText, string.Empty, string.Empty, AccountingProcessLocation.IsAccountingGenreShunjuen,
                    false)[0];
            price = trae.Price.ToString();
            IsReducedTaxRate = trae.IsReducedTaxRate;
        }
        /// <summary>
        /// フィールドをクリアします
        /// </summary>
        private void FieldClear()
        {
            CanOperation = false;
            IsValidity = true;
            AccountActivityDate = DateTime.Now;
            DebitAccountCode = string.Empty;
            CreditAccountCode = string.Empty;
            DetailText = string.Empty;
            price = string.Empty;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand DataOperationCommand { get; set; }
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
                detailText = value;
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
                { ErrorsListOperation(value.ToString().Length != 3, propertyName, "コードは3桁にしてください"); }
            }

            CanOperation = !HasErrors && SelectedCreditDept != null && AccountActivityDate != null &&
                DebitAccountCode.Length == 3 && DebitAccount != null && CreditAccountCode.Length == 3 &&
                CreditAccount != null && SelectedContent != null && IntAmount(Price) > 0;
        }

        protected override void SetDataList()
        {
            CreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, true, AccountingProcessLocation.IsAccountingGenreShunjuen);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        { DataOperationButtonContent = operation.ToString(); }

        protected override void SetDelegateCommand()
        {
        }

        protected override void SetDetailLocked() { IsValidityEnabled = CurrentOperation == DataOperation.更新; }

        protected override void SetWindowDefaultTitle() { WindowTitle = "複合伝票管理"; }
    }
}
