using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System.Collections.ObjectModel;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Commands;
using Domain.Entities;
using System;
using System.Threading.Tasks;
using WPF.Views.Datas;
using WPF.ViewModels.Datas;

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
        private bool isOutputButtonEnabled;
        private bool isNextPageEnabled;
        private bool isPrevPageEnabled;
        private Pagination pagination;
        #endregion

        public CreateVoucherViewModel
            (IDataBaseConnect dataBaseConnect,IDataOutput dataOutput) :base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();
            OperationData.Add(this);
            Pagination = new Pagination();
            SearchDate = DateTime.Today;
            OutputButtonContent = "登録して出力";
            Pagination.SetProperty();
            SetDelegateCommand();
        }
        public CreateVoucherViewModel() : this
            (DefaultInfrastructure.GetDefaultDataBaseConnect(),
            DefaultInfrastructure.GetDefaultDataOutput()) { }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; set; }
        private void NextPageListExpress() => Pagination.PageCountAdd();
        /// <summary>
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress() => Pagination.PageCountSubtract();
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
            if ((MessageBox = new MessageBoxInfo()
            {
                Message = "出力します。よろしいですか？",
                Image = System.Windows.MessageBoxImage.Question,
                Title = "登録確認",
                Button = System.Windows.MessageBoxButton.OKCancel
            }).Result == System.Windows.MessageBoxResult.Cancel) return;

            OutputButtonContent = "出力中";
            IsOutputButtonEnabled = false;
            Voucher voucher;
            VoucherRegistration();
            await Task.Run(()=> DataOutput.VoucherData(voucher));
            OutputButtonContent = "登録して出力";
            IsOutputButtonEnabled = true;
            
            void VoucherRegistration()
            {
                DataBaseConnect.Registration
                    (new Voucher(0, VoucherAddressee, VoucherContents, DateTime.Today));
                voucher = DataBaseConnect.CallLatestVoucher();
                voucher.ReceiptsAndExpenditures = VoucherContents;
                foreach (ReceiptsAndExpenditure rae in VoucherContents)
                    DataBaseConnect.Registration(voucher.ID, rae.ID);
            }
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
            if(VoucherContents.Count==8)
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message = "9件以上のデータは受納証に載せられません。\r\n受納証を分けて発行してください。",
                    Image = System.Windows.MessageBoxImage.Warning,
                    Button = System.Windows.MessageBoxButton.OK,
                    Title = "内容件数上限超過"
                };
                return;
            }
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
            Pagination.CountReset(isPageReset);
            var(count,list)=
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                (TextHelper.DefaultDate, new DateTime(9999, 1, 1),AccountingProcessLocation.Location, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, true, true, true,
                accountActivityDate, accountActivityDate, TextHelper.DefaultDate, new DateTime(9999, 1, 1), 
                Pagination.PageCount,"ID",false);
            SearchReceiptsAndExpenditures = list;
            Pagination.TotalRowCount = count;
            Pagination.SetProperty();
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
        /// <summary>
        /// ページネーション
        /// </summary>
        public Pagination Pagination
        {
            get => pagination;
            set
            {
                pagination = value;
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

        public void ReceiptsAndExpenditureOperationNotify()
        {
            SearchDate = OperationData.Data.AccountActivityDate;
            VoucherContents.Add(OperationData.Data);
            SetTotalAmount();
            SetOutputEnabled();
        }
    }
}
