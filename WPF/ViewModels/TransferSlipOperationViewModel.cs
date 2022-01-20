using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    public class TransferSlipOperationViewModel : DataOperationViewModel, IPagenationObserver,
        ITransferReceiptsAndExpenditureOperationObserver
    {
        #region Properties
        #region Strings
        private string searchSubjectCode;
        private string selectedSubjectCode;
        private string selectedSubject;
        private string selectedContentText;
        private string selectedDetail;
        private string selectedPrice;
        private string debitAccountCode;
        private string creditAccountCode;
        private string detailText;
        private string price;
        private string dataOperationButtonContent;
        #endregion
        #region Bools
        private bool isLimitedCreditDept;
        private bool isDoNotUseOriginalSlip;
        private bool isPayment;
        private bool isValidity;
        private bool isReducedTaxRate;
        #endregion
        #region Dates
        private DateTime searchStartDate;
        private DateTime searchEndDate;
        private DateTime accountActivityDate;
        #endregion
        #region ObservableCollections
        private ObservableCollection<CreditDept> searchCreditDepts;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private ObservableCollection<TransferReceiptsAndExpenditure> transferReceiptsAndExpenditures;
        private ObservableCollection<AccountingSubject> debitAccounts;
        private ObservableCollection<AccountingSubject> creditAccounts;
        private ObservableCollection<Content> contents;
        #endregion
        #region ValueObjects
        private CreditDept searchCreditDept;
        private CreditDept selectedCreditDept;
        private AccountingSubject debitAccount;
        private AccountingSubject creditAccount;
        private Content selectedContent;
        #endregion
        #endregion

        public TransferSlipOperationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            if(TransferReceiptsAndExpenditureOperation.GetInstance() == null) { FieldClear(); }
            else { FieldClear(); }
        }
        public TransferSlipOperationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        private void FieldClear()
        {
            IsDoNotUseOriginalSlip=false;
            SearchStartDate=DateTime.Now;
            SearchEndDate=DateTime.Now;
            SearchSubjectCode= string.Empty;
            IsLimitedCreditDept=false;
            ReceiptsAndExpenditureFieldClear();
        }

        private void ReceiptsAndExpenditureFieldClear()
        {
            SelectedSubjectCode = string.Empty;
            SelectedSubject= string.Empty;
            SelectedContentText= string.Empty;
            SelectedDetail= string.Empty;
            SelectedPrice= string.Empty;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand DataOperationCommand { get; set; }
        /// <summary>
        /// 出納データ詳細表示コマンド
        /// </summary>
        public DelegateCommand ShowDetailCommand { get; set; }
        /// <summary>
        /// 選択された出納データ反映コマンド
        /// </summary>
        public DelegateCommand SelectedReceiptsAndExpenditureCommand { get; set; }
        /// <summary>
        /// 出納データ検索コマンド
        /// </summary>
        public DelegateCommand SearchReceiptsAndExpenditureCommand { get; set; }
        /// <summary>
        /// 検索メニューの貸方部門
        /// </summary>
        public ObservableCollection<CreditDept> SearchCreditDepts
        {
            get => searchCreditDepts;
            set
            {
                searchCreditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票リスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures
        {
            get => receiptsAndExpenditures;
            set
            {
                receiptsAndExpenditures = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 既に関連付けられている振替伝票リスト
        /// </summary>
        public ObservableCollection<TransferReceiptsAndExpenditure> TransferReceiptsAndExpenditures
        {
            get => transferReceiptsAndExpenditures;
            set
            {
                transferReceiptsAndExpenditures = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索開始日
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                searchStartDate = value > SearchEndDate ? SearchEndDate : value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索最終日
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                searchEndDate = value < SearchStartDate ? SearchStartDate : value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する科目コード
        /// </summary>
        public string SearchSubjectCode
        {
            get => searchSubjectCode;
            set
            {
                searchSubjectCode = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方部門
        /// </summary>
        public CreditDept SearchCreditDept
        {
            get => searchCreditDept;
            set
            {
                searchCreditDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する貸方部門を限定チェック
        /// </summary>
        public bool IsLimitedCreditDept
        {
            get => isLimitedCreditDept;
            set
            {
                isLimitedCreditDept = value;
                CallPropertyChanged();
                if (value) { SearchCreditDept = null; }
            }
        }
        /// <summary>
        /// 元の伝票を使用しないチェック
        /// </summary>
        public bool IsDoNotUseOriginalSlip
        {
            get => isDoNotUseOriginalSlip;
            set
            {
                isDoNotUseOriginalSlip = value;
                CallPropertyChanged();
                if(value) 
                {
                    SearchCreditDept = null;

                }
            }
        }
        /// <summary>
        /// 入出金チェック
        /// </summary>
        public bool IsPayment
        {
            get => isPayment;
            set
            {
                isPayment = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された科目コード
        /// </summary>
        public string SelectedSubjectCode
        {
            get => selectedSubjectCode;
            set
            {
                selectedSubjectCode = value;
                CallPropertyChanged ();
            }
        }
        /// <summary>
        /// 選択された勘定科目
        /// </summary>
        public string SelectedSubject
        {
            get => selectedSubject;
            set
            {
                selectedSubject = value;
                CallPropertyChanged () ;
            }
        }
        /// <summary>
        /// 選択された伝票内容
        /// </summary>
        public string SelectedContentText
        {
            get => selectedContentText;
            set
            {
                selectedContentText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票詳細
        /// </summary>
        public string SelectedDetail
        {
            get => selectedDetail;
            set
            {
                selectedDetail = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票金額
        /// </summary>
        public string SelectedPrice
        {
            get => selectedPrice;
            set
            {
                selectedPrice = value;
                CallPropertyChanged();
            }
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
            }
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
                price =CommaDelimitedAmount(value);
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

        public override void ValidationProperty(string propertyName, object value)
        {
            
        }

        protected override void SetDataList()
        {
            throw new NotImplementedException();
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
            throw new NotImplementedException();
        }

        protected override void SetDelegateCommand()
        {
            throw new NotImplementedException();
        }

        protected override void SetDetailLocked()
        {
            throw new NotImplementedException();
        }

        protected override void SetWindowDefaultTitle() { WindowTitle = "複合伝票管理"; }

        public void SortNotify()
        {
            throw new NotImplementedException();
        }

        public void PageNotify()
        {
            throw new NotImplementedException();
        }

        public void SetSortColumns()
        {
            throw new NotImplementedException();
        }

        public void SetCountEachPage()
        {
            throw new NotImplementedException();
        }

        public void TransferReceiptsAndExpenditureOperationNotify()
        {
            throw new NotImplementedException();
        }
    }
}
