using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace WPF.ViewModels
{
    public class ReceiptsAndExpenditureMangementViewModel : BaseViewModel
    {
        #region Properties
        private readonly Cashbox Cashbox = Cashbox.GetInstance();
        private string cashBoxTotalAmount;
        private bool isRegistrationCheck;
        private bool isUpdateCheck;
        private bool isPaymentCheck;
        private string depositAndWithdrawalContetnt;
        private readonly IDataBaseConnect DataBaseConnect;
        private SolidColorBrush detailBackGroundColor;
        private Content selectedContent;
        private ObservableCollection<Content> comboContents;
        private string comboContentText;
        private AccountingSubject selectedAccountingSubject;
        private ObservableCollection<AccountingSubject> comboAccountingSubjects;
        private string comboAccountingSubjectText;
        private string comboAccountingSubjectCode;
        private string detailText;
        private DateTime accountActivityDate;
        private string price;
        private string receiptsAndExpenditureIDField;
        private ObservableCollection<CreditAccount> comboCreditAccounts;
        private CreditAccount selectedCreditAccount;
        private string comboCreditAccountText;
        #endregion

        public ReceiptsAndExpenditureMangementViewModel(IDataBaseConnect dataBaseConnect)
        {
            DataBaseConnect = dataBaseConnect;
            IsPaymentCheck = true;
            CashBoxTotalAmount = $"金庫の金額 : {Cashbox.GetTotalAmountWithUnit()}";
            AccountActivityDate = DateTime.Today;
            SetComboBoxItem();
        }
         public ReceiptsAndExpenditureMangementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
       /// <summary>
        /// 各コンボボックスのリストをセットします
        /// </summary>
        private void SetComboBoxItem()
        {
            ComboContents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty, string.Empty, true);
            ComboAccountingSubjects = DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboCreditAccounts = DataBaseConnect.ReferenceCreditAccount(string.Empty, true);
            SelectedCreditAccount = ComboCreditAccounts[0];
        }
        /// <summary>
        /// 金庫の総計金額
        /// </summary>
        public string CashBoxTotalAmount
        {
            get => cashBoxTotalAmount;
            set
            {
                cashBoxTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作　登録チェック
        /// </summary>
        public bool IsRegistrationCheck
        {
            get => isRegistrationCheck;
            set
            {
                isRegistrationCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作　更新チェック
        /// </summary>
        public bool IsUpdateCheck
        {
            get => isUpdateCheck;
            set
            {
                isUpdateCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金チェック　入金がTrue
        /// </summary>
        public bool IsPaymentCheck
        {
            get => isPaymentCheck;
            set
            {
                isPaymentCheck = value;
                if (value)
                {
                    DepositAndWithdrawalContetnt = "入金";
                    DetailBackGroundColor = new SolidColorBrush(Colors.MistyRose);
                }
                else
                {
                    DepositAndWithdrawalContetnt = "出金";
                    DetailBackGroundColor = new SolidColorBrush(Colors.LightCyan);
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金のトグルコンテント
        /// </summary>
        public string DepositAndWithdrawalContetnt
        {
            get => depositAndWithdrawalContetnt;
            set
            {
                depositAndWithdrawalContetnt = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金に応じて、詳細の色を決める
        /// </summary>
        public SolidColorBrush DetailBackGroundColor
        {
            get => detailBackGroundColor;
            set
            {
                detailBackGroundColor = value;
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

                if (selectedContent.FlatRate > 0) Price = TextHelper.CommaDelimitedAmount(selectedContent.FlatRate);
                else Price = string.Empty;

                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスリスト
        /// </summary>
        public ObservableCollection<Content> ComboContents
        {
            get => comboContents;
            set
            {
                comboContents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスのText
        /// </summary>
        public string ComboContentText
        {
            get => comboContentText;
            set
            {
                if (comboContentText == value) return;
                comboContentText = value;
                SelectedContent = ComboContents.FirstOrDefault(c => c.Text == value);
                if (SelectedContent == null) ComboContentText = string.Empty;
                else
                {
                    ComboAccountingSubjects = DataBaseConnect.ReferenceAffiliationAccountingSubject(comboContentText);
                    ComboAccountingSubjectText = ComboAccountingSubjects[0].Subject;
                    ComboAccountingSubjectCode = comboAccountingSubjects[0].SubjectCode;
                }
                ValidationProperty(nameof(ComboContentText), comboContentText);
                
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// コンボボックスで選択された勘定科目
        /// </summary>
        public AccountingSubject SelectedAccountingSubject
        {
            get => selectedAccountingSubject;
            set
            {
                if (selectedAccountingSubject != null && selectedAccountingSubject.Equals(value)) return;
                selectedAccountingSubject = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コンボボックスの勘定科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> ComboAccountingSubjects
        {
            get => comboAccountingSubjects;
            set
            {
                comboAccountingSubjects = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectText
        {
            get => comboAccountingSubjectText;
            set
            {
                if (comboAccountingSubjectText == value) return;
                comboAccountingSubjectText = value;
                SelectedAccountingSubject = ComboAccountingSubjects.FirstOrDefault(a => a.Subject == comboAccountingSubjectText);
                if (SelectedAccountingSubject == null) comboAccountingSubjectText = string.Empty;
                else
                {
                    comboAccountingSubjectText = selectedAccountingSubject.Subject;
                    ComboAccountingSubjectCode = selectedAccountingSubject.SubjectCode;
                    SetAccountingSubjectChildContents();
                }

                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードコンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectCode
        {
            get => comboAccountingSubjectCode;
            set
            {
                if (comboAccountingSubjectCode == value) return;
                comboAccountingSubjectCode = value;
                
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 詳細欄のText
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
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納金額
        /// </summary>
        public string Price
        {
            get => price;
            set
            {
                price = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納IDフィールド
        /// </summary>
        public string ReceiptsAndExpenditureIDField
        {
            get => receiptsAndExpenditureIDField;
            set
            {
                receiptsAndExpenditureIDField = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定コンボボックスのリスト
        /// </summary>
        public ObservableCollection<CreditAccount> ComboCreditAccounts
        {
            get => comboCreditAccounts;
            set
            {
                comboCreditAccounts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方勘定
        /// </summary>
        public CreditAccount SelectedCreditAccount
        {
            get => selectedCreditAccount;
            set
            {
                selectedCreditAccount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方勘定コンボボックスのText
        /// </summary>
        public string ComboCreditAccountText
        {
            get => comboCreditAccountText;
            set
            {
                comboCreditAccountText = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 勘定科目に所属している伝票内容を検索してリストに代入します
        /// </summary>
        private void SetAccountingSubjectChildContents()
        {
             ComboContents = DataBaseConnect.ReferenceContent(string.Empty, string.Empty, ComboAccountingSubjectText, true);
            if (SelectedContent == null) ComboContentText = ComboContents[0].Text;
            else SelectedContent = ComboContents.FirstOrDefault(c => c.Text == ComboContentText);
        }
        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(ComboContentText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
            }
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"出納管理 : {AccountingProcessLocation.Location}";
            return DefaultWindowTitle;
        }
    }
}