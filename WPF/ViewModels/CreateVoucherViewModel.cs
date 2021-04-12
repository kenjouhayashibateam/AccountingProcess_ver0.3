using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Commands;
using Domain.Entities;
using System;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証作成画面ViewModel
    /// </summary>
    public class CreateVoucherViewModel : DataOperationViewModel
    {
        #region Properties
        #region Strings
        private string comboAccountingSubjectCode;
        private string comboAccountingSubject;
        private string comboContent;
        private string addresseeTitle;
        private string registrationAddressee;
        private string registrationPriceDisplayValue;
        private string voucherAddressee;
        private string voucherTotalAmountDisplayValue;
        #endregion
        private int registrationPrice;
        private bool isReducedTaxRate;
        #region ObservableCollections
        private ObservableCollection<CreditDept> creditDepts;
        private ObservableCollection<AccountingSubject> accountingSubjectCodes;
        private ObservableCollection<AccountingSubject> accountingSubjects;
        private ObservableCollection<Content> contents;
        private ObservableCollection<ReceiptsAndExpenditure> voucherContents = new ObservableCollection<ReceiptsAndExpenditure>();
        private ObservableCollection<ReceiptsAndExpenditure> searchReceiptsAndExpenditures;
        #endregion
        private DateTime registrationAccountActivityDate;
        private DateTime searchDate;
        private CreditDept selectedCreditDept;
        private AccountingSubject selectedAccountingSubjectCode;
        private AccountingSubject selectedAccontingSubject;
        private Content selectedContent;
        private ReceiptsAndExpenditure selectedVoucherContent;
        private ReceiptsAndExpenditure selectedSeachReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        #endregion

        public CreateVoucherViewModel(IDataBaseConnect dataBaseConnect,IDataOutput dataOutput) :base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            AddresseeTitle = "様";
            RegistrationAccountActivityDate = DateTime.Today;
            SearchDate = DateTime.Today;
        }
        public CreateVoucherViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect(), DefaultInfrastructure.GetDefaultDataOutput()) { }
        public DelegateCommand AddVoucherContentCommand { get; set; }
        private void AddVoucherContent()
        {
            VoucherContents.Add(SelectedSeachReceiptsAndExpenditure);
            SetTotalAmount();
        }
        /// <summary>
        /// 受納証の出納データリストから出納データを削除するコマンド
        /// </summary>
        public DelegateCommand DeleteVoucherContentCommand { get; set; }
        private void DeleteVoucherContent()
        {
            VoucherContents.Remove(selectedVoucherContent);
            SetTotalAmount();
        }
        private void SetTotalAmount()
        {
            int i = default;
            foreach (ReceiptsAndExpenditure rae in VoucherContents) i += rae.Price;
            VoucherTotalAmountDisplayValue = i.ToString();
        }
        /// <summary>
        /// 出納データ登録コマンド
        /// </summary>
        public DelegateCommand RegistrationReceiptsAndExpenditureCommand { get; set; }
        private void RegistrationReceiptsAndExpenditure()
        {
            ReceiptsAndExpenditure rae = new ReceiptsAndExpenditure(0, DateTime.Today, LoginRep.Rep, AccountingProcessLocation.Location, SelectedCreditDept, SelectedContent,
                    $"{RegistrationAddressee}{AddresseeTitle}", RegistrationPrice, true, true, RegistrationAccountActivityDate, TextHelper.DefaultDate, IsReducedTaxRate);
            if (!ConfirmationRegistration(rae)) return;
            DataBaseConnect.Registration(rae);
            VoucherContents.Add(rae);
            VoucherAddressee = RegistrationAddressee;
        }
        private bool ConfirmationRegistration(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            MessageBox = new Views.Datas.MessageBoxInfo()
            {
                Message = $"経理担当場所\t : {receiptsAndExpenditure.Location}\r\n入出金日\t\t : {receiptsAndExpenditure.AccountActivityDate.ToShortDateString()}\r\n" +
                $"貸方勘定\t\t : {receiptsAndExpenditure.CreditDept.Dept}\r\nコード\t\t : {receiptsAndExpenditure.Content.AccountingSubject.SubjectCode}\r\n" +
                 $"勘定科目\t\t : {receiptsAndExpenditure.Content.AccountingSubject.Subject}\r\n内容\t\t : {receiptsAndExpenditure.Content.Text}\r\n" +
                 $"詳細\t\t : {receiptsAndExpenditure.Detail}\r\n金額\t\t : {TextHelper.AmountWithUnit(receiptsAndExpenditure.Price)}\r\n" +
                 $"軽減税率\t\t : {receiptsAndExpenditure.IsReducedTaxRate}\r\n有効性\t\t : {receiptsAndExpenditure.IsValidity}\r\n" +
                 $"\r\n登録しますか？",
                Button = System.Windows.MessageBoxButton.YesNo,
                Title="登録確認",
                Image=System.Windows.MessageBoxImage.Question
            };
            CallShowMessageBox = true;
            return MessageBox.Result == System.Windows.MessageBoxResult.Yes;
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
                    value = AccountingSubjects[0].SubjectCode;
                    ComboAccountingSubject = AccountingSubjects[0].Subject;
                }
                else
                {
                    value = string.Empty;
                    ComboAccountingSubject = string.Empty;
                }
                comboAccountingSubjectCode = value;
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
        /// <summary>
        /// 伝票登録する宛名
        /// </summary>
        public string RegistrationAddressee
        {
            get => registrationAddressee;
            set
            {
                registrationAddressee = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票登録する金額
        /// </summary>
        public int RegistrationPrice
        {
            get => registrationPrice;
            set
            {
                registrationPrice = value;
                ValidationProperty(nameof(RegistrationPrice), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ビューに表示する伝票登録の金額
        /// </summary>
        public string RegistrationPriceDisplayValue
        {
            get => registrationPriceDisplayValue;
            set
            {
                registrationPriceDisplayValue = TextHelper.CommaDelimitedAmount(value);
                ValidationProperty(nameof(RegistrationPriceDisplayValue), registrationPriceDisplayValue);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する入金日
        /// </summary>
        public DateTime RegistrationAccountActivityDate
        {
            get => registrationAccountActivityDate;
            set
            {
                registrationAccountActivityDate = value;
                CallPropertyChanged();
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
        /// 受納証の宛名
        /// </summary>
        public string VoucherAddressee
        {
            get => voucherAddressee;
            set
            {
                voucherAddressee = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証の但し書きに表示するデータリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> VoucherContents
        {
            get => voucherContents;
            set
            {
                voucherContents = value;
                int i = default;
                foreach (ReceiptsAndExpenditure rae in voucherContents) i += rae.Price;
                VoucherTotalAmountDisplayValue = i.ToString();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証に記載する合計金額のビュー表示文字列
        /// </summary>
        public string VoucherTotalAmountDisplayValue
        {
            get => voucherTotalAmountDisplayValue;
            set
            {
                voucherTotalAmountDisplayValue =TextHelper.CommaDelimitedAmount(value);
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
            }
        }
        /// <summary>
        /// 検索日時
        /// </summary>
        public DateTime SearchDate
        {
            get => searchDate;
            set
            {
                searchDate = value;
                SearchReceiptsAndExpenditures =
                    DataBaseConnect.ReferenceReceiptsAndExpenditure
                    (TextHelper.DefaultDate, new DateTime(9999, 1, 1), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true, true, true, 
                    true, value, value, TextHelper.DefaultDate, new DateTime(9999, 1, 1));
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索した出納データのリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> SearchReceiptsAndExpenditures
        {
            get => searchReceiptsAndExpenditures;
            set
            {
                searchReceiptsAndExpenditures = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された受納証リストの出納データ
        /// </summary>
        public ReceiptsAndExpenditure SelectedVoucherContent
        {
            get => selectedVoucherContent;
            set
            {
                selectedVoucherContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された出納データ検索リストの出納データ
        /// </summary>
        public ReceiptsAndExpenditure SelectedSeachReceiptsAndExpenditure
        {
            get => selectedSeachReceiptsAndExpenditure;
            set
            {
                selectedSeachReceiptsAndExpenditure = value;
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
            switch(propertyName)
            {
                case nameof(RegistrationPrice):
                    ErrorsListOperation((int)value == 0, propertyName, "金額は1円以上で登録して下さい。");
                    break;
                case nameof(RegistrationPriceDisplayValue):
                    SetNullOrEmptyError(propertyName,(string)value);
                    break;
            }
        }

        protected override void SetDataList()
        {
            CreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, true);
            AccountingSubjectCodes = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) { }

        protected override void SetDelegateCommand()
        {
            RegistrationReceiptsAndExpenditureCommand = new DelegateCommand
                (() => RegistrationReceiptsAndExpenditure(), () => true);
            DeleteVoucherContentCommand = new DelegateCommand
                (() => DeleteVoucherContent(), () => true);
            AddVoucherContentCommand = new DelegateCommand
                (() => AddVoucherContent(), () => true);
        }

        protected override void SetDetailLocked() { }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = $"受納証作成 : {AccountingProcessLocation.Location}";
    }
}
