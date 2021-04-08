using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証作成画面ViewModel
    /// </summary>
    public class CreateVoucherViewModel : DataOperationViewModel
    {
        #region Properties
        private string comboAccountingSubjectCode;
        private string comboAccountingSubject;
        private string comboContent;
        private string addresseeTitle;
        private ObservableCollection<CreditDept> creditDepts;
        private ObservableCollection<AccountingSubject> accountingSubjectCodes;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        private ObservableCollection<Content> contents;
        private CreditDept selectedCreditDept;
        private AccountingSubject selectedAccountingSubjectCode;
        private AccountingSubject selectedAccontingSubject;
        public Content SelectedContent;
        private readonly IDataOutput DataOutput;
        #endregion

        public CreateVoucherViewModel(IDataBaseConnect dataBaseConnect,IDataOutput dataOutput) :base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            AddresseeTitle = "様";
        }
        public CreateVoucherViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect(), DefaultInfrastructure.GetDefaultDataOutput()) { }
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
        /// 選択された勘定科目コード
        /// </summary>
        public AccountingSubject SelectedAccountingSubjectCode
        {
            get => selectedAccountingSubjectCode;
            set
            {
                selectedAccountingSubjectCode = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードリスト
        /// </summary>
        public ObservableCollection<AccountingSubject> AccountingSubjectCodes
        {
            get => accountingSubjectCodes;
            set
            {
                accountingSubjectCodes = value;
                CallPropertyChanged();
            }
        }
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
        /// 選択された勘定科目
        /// </summary>
        public AccountingSubject SelectedAccontingSubject
        {
            get => selectedAccontingSubject;
            set
            {
                selectedAccontingSubject = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コード文字列
        /// </summary>
        public string ComboAccountingSubjectCode
        {
            get => comboAccountingSubjectCode;
            set
            {
                if (!string.IsNullOrEmpty(value)) AccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(value, string.Empty, true);
                else
                {
                    AccountingSubjects.Clear();
                    ComboAccountingSubject = string.Empty;
                }

                if(AccountingSubjects.Count>0)
                {
                    comboAccountingSubjectCode = AccountingSubjects[0].SubjectCode;
                    ComboAccountingSubject = AccountingSubjects[0].Subject;
                }
                else
                {
                    comboAccountingSubjectCode = string.Empty;
                    ComboAccountingSubject = string.Empty;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目文字列
        /// </summary>
        public string ComboAccountingSubject
        {
            get => comboAccountingSubject;
            set
            {
                if (!string.IsNullOrEmpty(value)) Contents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty, value, true);
                else
                {
                    Contents.Clear();
                    ComboContent = string.Empty;
                }

                if(Contents.Count>0)
                {
                    comboAccountingSubject = Contents[0].AccountingSubject.Subject;
                    ComboContent = Contents[0].Text;
                }
                else
                {
                    comboAccountingSubject = string.Empty;
                    ComboContent = string.Empty;
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
        /// 伝票内容文字列
        /// </summary>
        public string ComboContent
        {
            get => comboContent;
            set
            {
                comboContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する宛名の敬称
        /// </summary>
        public string AddresseeTitle
        {
            get => addresseeTitle;
            set
            {
                addresseeTitle = value;
                CallPropertyChanged();
            }
        }

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle = $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDataList()
        {
            CreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, true);
            AccountingSubjectCodes = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDelegateCommand()
        {
            
        }

        protected override void SetDetailLocked()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = $"受納証作成 : {AccountingProcessLocation.Location}";
    }
}
