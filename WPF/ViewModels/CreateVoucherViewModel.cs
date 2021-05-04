using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Commands;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証作成画面ViewModel
    /// </summary>
    public class CreateVoucherViewModel : BaseViewModel,
        IReceiptsAndExpenditureOperationObserver
    {
        #region Properties
        #region Strings
        private string voucherAddressee;
        private string voucherTotalAmountDisplayValue;
        private string outputButtonContent;
        private string listPageInfo;
        #endregion
        #region ObservableCollections
        private ObservableCollection<ReceiptsAndExpenditure> voucherContents = 
            new ObservableCollection<ReceiptsAndExpenditure>();
        private ObservableCollection<ReceiptsAndExpenditure> searchReceiptsAndExpenditures;
        #endregion
        private DateTime searchDate;
        private ReceiptsAndExpenditure selectedVoucherContent;
        private ReceiptsAndExpenditure selectedSeachReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        private readonly ReceiptsAndExpenditureOperation OperationData;
        /// <summary>
        /// 受納証の総額
        /// </summary>
        private int VoucherTotalAmount;
        /// <summary>
        /// リストデータの総数
        /// </summary>
        private int RowCount;
        /// <summary>
        /// リストのページ
        /// </summary>
        private int PageCount;
        /// <summary>
        /// リストのページの総数
        /// </summary>
        private int TotalPageCount;
        private bool isOutputButtonEnabled;
        private bool isNextPageEnabled;
        private bool isPrevPageEnabled;
        #endregion

        public CreateVoucherViewModel
            (IDataBaseConnect dataBaseConnect,IDataOutput dataOutput) :base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();
            OperationData.Add(this);
            SearchDate = DateTime.Today;
            OutputButtonContent = "出力";
            SetListPageInfo();
            SetDelegateCommand();
        }
        public CreateVoucherViewModel() : this
            (DefaultInfrastructure.GetDefaultDataBaseConnect(),
            DefaultInfrastructure.GetDefaultDataOutput()) { }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; set; }
        private void NextPageListExpress()
        {
            if (PageCount == 0) return;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            CreateReceiptsAndExpenditures(SearchDate,false);
        }
        /// <summary>
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress()
        {
            if (PageCount == 0) return;
            if (PageCount > 1) PageCount--;
            CreateReceiptsAndExpenditures(SearchDate,false);
        }

        private void SetListPageInfo()
        {

            int i = RowCount / 10;
            i += RowCount % 10 == 0 ? 0 : 1;
            TotalPageCount = i;
            ListPageInfo = $"{PageCount}/{i}";
            IsPrevPageEnabled = PageCount > 1;
            IsNextPageEnabled = PageCount != i;
        }
        /// <summary>
        /// 新規登録画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowRegistrationCommand { get; set; }
        private void ShowRegistration()
        {
            OperationData.SetOperationType(ReceiptsAndExpenditureOperation.OperationType.Voucher);
            OperationData.SetData(null);
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureOperation());
        }
        /// <summary>
        /// 受納証データを出力するコマンド
        /// </summary>
        public DelegateCommand VoucherOutputCommand { get; set; }
        private async void VoucherOutput()
        {
            OutputButtonContent = "出力中";
            IsOutputButtonEnabled = false;
            await Task.Run(()=> DataOutput.VoucherData
                (new Voucher(VoucherAddressee,VoucherContents,VoucherTotalAmount)));
            OutputButtonContent = "出力";
            IsOutputButtonEnabled = true;
        }
        /// <summary>
        /// 受納証の出納データリストに出納データを追加するコマンド
        /// </summary>
        public DelegateCommand AddVoucherContentCommand { get; set; }
        private async void AddVoucherContent()
        {
            await Task.Delay(1);
            if (SelectedSeachReceiptsAndExpenditure == null) return;
            if (VoucherContents.Contains(SelectedSeachReceiptsAndExpenditure)) return;
            VoucherContents.Add(SelectedSeachReceiptsAndExpenditure);
            SetTotalAmount();
            SetOutputEnabled();
        }
        /// <summary>
        /// 受納証の出納データリストから出納データを削除するコマンド
        /// </summary>
        public DelegateCommand DeleteVoucherContentCommand { get; set; }
        private async void DeleteVoucherContent()
        {
            await Task.Delay(1);
            VoucherContents.Remove(selectedVoucherContent);
            SetTotalAmount();
            SetOutputEnabled();
        }
        private void SetTotalAmount()
        {
            int i = default;

            foreach (ReceiptsAndExpenditure rae in VoucherContents) i += rae.Price;
            VoucherTotalAmountDisplayValue = i.ToString();
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
                ValidationProperty(nameof(VoucherAddressee), value);
                SetOutputEnabled();
                CallPropertyChanged();
            }
        }

        private void SetOutputEnabled()=>
            IsOutputButtonEnabled = VoucherContents.Count != 0 & !string.IsNullOrEmpty(VoucherAddressee);
        
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
                VoucherTotalAmount = TextHelper.IntAmount(value);
                voucherTotalAmountDisplayValue =TextHelper.CommaDelimitedAmount(value);
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
                CreateReceiptsAndExpenditures(value, true);
                CallPropertyChanged();
            }
        }
        private void CreateReceiptsAndExpenditures(DateTime accountActivityDate,bool isPageReset)
        {
            PageCount = isPageReset ? 1 : PageCount;
            ObservableCollection<ReceiptsAndExpenditure> allDataList=
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                (TextHelper.DefaultDate, new DateTime(9999, 1, 1), string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, true, true, true,
                accountActivityDate, accountActivityDate, TextHelper.DefaultDate, new DateTime(9999, 1, 1));

            SearchReceiptsAndExpenditures =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                (TextHelper.DefaultDate, new DateTime(9999, 1, 1), string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, true, true, true,
                accountActivityDate, accountActivityDate, TextHelper.DefaultDate, new DateTime(9999, 1, 1), PageCount).List;
            RowCount =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                (TextHelper.DefaultDate, new DateTime(9999, 1, 1), string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, true, true, true,
                accountActivityDate, accountActivityDate, TextHelper.DefaultDate, new DateTime(9999, 1, 1), PageCount).TotalRows;
            RowCount = allDataList.Count;
            SetListPageInfo();
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
                SetListPageInfo();
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
        /// <summary>
        /// 出力ボタンのEnable
        /// </summary>
        public bool IsOutputButtonEnabled
        {
            get => isOutputButtonEnabled;
            set
            {
                isOutputButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力ボタンのContent
        /// </summary>
        public string OutputButtonContent
        {
            get => outputButtonContent;
            set
            {
                outputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// リストのページの案内
        /// </summary>
        public string ListPageInfo
        {
            get => listPageInfo;
            set
            {
                listPageInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 次の10件ボタンのEnabled
        /// </summary>
        public bool IsNextPageEnabled
        {
            get => isNextPageEnabled;
            set
            {
                isNextPageEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 前の10件ボタンのEnabled
        /// </summary>
        public bool IsPrevPageEnabled
        {
            get => isPrevPageEnabled;
            set
            {
                isPrevPageEnabled = value;
                CallPropertyChanged();
            }
        }

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle =
                    $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(VoucherAddressee):
                    SetNullOrEmptyError(propertyName, (string)value);
                    break;
            }
        }

        protected void SetDelegateCommand()
        {
            DeleteVoucherContentCommand = new DelegateCommand
                (() => DeleteVoucherContent(), () => true);
            AddVoucherContentCommand = new DelegateCommand
                (() => AddVoucherContent(), () => true);
            VoucherOutputCommand = new DelegateCommand
                (() => VoucherOutput(), () => true);
            ShowRegistrationCommand = new DelegateCommand
                (() => ShowRegistration(), () => true);
            NextPageListExpressCommand = new DelegateCommand
                (() => NextPageListExpress(), () => true);
            PrevPageListExpressCommand = new DelegateCommand
                (() => PrevPageListExpress(), () => true);
        }

        protected override void SetWindowDefaultTitle() =>
            DefaultWindowTitle = $"受納証作成 : {AccountingProcessLocation.Location}";

        public void Notify()
        {
            SearchDate = OperationData.Data.AccountActivityDate;
            VoucherContents.Add(OperationData.Data);
            SetTotalAmount();
            SetOutputEnabled();
        }
    }
}
