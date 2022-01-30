using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証作成画面ViewModel
    /// </summary>
    public class CreateVoucherViewModel : BaseViewModel,
        IReceiptsAndExpenditureOperationObserver, IPagenationObserver, IClosing
    {
        #region Properties
        #region Strings
        private string voucherAddressee;
        private string voucherTotalAmountDisplayValue;
        private string outputButtonContent;
        public string InputSVGFullPath { get => System.IO.Path.GetFullPath("./svgFiles/input_black_24dp.svg"); }
        #endregion
        #region ObservableCollections
        private ObservableCollection<ReceiptsAndExpenditure> voucherContents =
            new ObservableCollection<ReceiptsAndExpenditure>();
        private ObservableCollection<ReceiptsAndExpenditure> searchReceiptsAndExpenditures;
        #endregion
        private DateTime searchDate = DateTime.Today;
        private DateTime outputDate;
        private DateTime prepaidDate = DefaultDate;
        private ReceiptsAndExpenditure selectedVoucherContent;
        private ReceiptsAndExpenditure selectedSeachReceiptsAndExpenditure;
        private readonly IDataOutput DataOutput;
        private readonly ReceiptsAndExpenditureOperation OperationData;
        private bool isOutputButtonEnabled;
        private bool isPrepaid = false;
        private bool isClose = true;
        private Pagination pagination;
        #endregion

        public CreateVoucherViewModel
            (IDataBaseConnect dataBaseConnect, IDataOutput dataOutput) : base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();
            OperationData.Add(this);
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            Pagination.SortDirectionIsASC = false;
            SearchDate = DateTime.Today;
            OutputDate = DateTime.Today;
            OutputButtonContent = "登録して出力";
            SetDelegateCommand();
        }
        public CreateVoucherViewModel() : this
            (DefaultInfrastructure.GetDefaultDataBaseConnect(),
                DefaultInfrastructure.GetDefaultDataOutput())
        { }
        /// <summary>
        /// 受納証管理画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowVoucherManagementCommand { get; set; }
        private void ShowVoucherManagement()
        {
            CreateShowWindowCommand(ScreenTransition.VoucherManagement());
        }

        /// <summary>
        /// 受納証データを使用して御布施一覧を表示してデータ登録するコマンド
        /// </summary>
        public DelegateCommand ShowCondolenceOperationCommand { get; set; }
        private void ShowCondolenceOperation()
        {
            string addressee = default;
            int almsgiving = default;
            int carTip = default;
            int mealTip = default;
            int carAndMealTip = default;
            int socialGethering = default;
            DateTime accountActivityDate = DefaultDate;
            bool isHeadData = true;

            foreach (ReceiptsAndExpenditure rae in VoucherContents)
            {
                switch (rae.Content.AccountingSubject.SubjectCode)
                {
                    case "815":
                        almsgiving = rae.Price;
                        break;
                    case "831":
                        socialGethering = rae.Price;
                        break;
                    case "832":
                        SetAmount();
                        break;
                    default:
                        break;
                }

                if (isHeadData)
                {
                    addressee = rae.Detail;
                    accountActivityDate = rae.AccountActivityDate;
                }

                void SetAmount()
                {
                    switch (rae.Content.Text)
                    {
                        case "御車代":
                            carTip = rae.Price;
                            break;
                        case "御膳料":
                            mealTip = rae.Price;
                            break;
                        case "御車代御膳料":
                            carAndMealTip = rae.Price;
                            break;
                        default:
                            break;
                    }
                }
            }

            Condolence condolence = new Condolence
                (0, AccountingProcessLocation.Location.ToString(), addressee, string.Empty, "法事", almsgiving,
                    carTip, mealTip, carAndMealTip, socialGethering, string.Empty, accountActivityDate,
                    LoginRep.GetInstance().Rep.Name, string.Empty);
            CondolenceOperation co = CondolenceOperation.GetInstance();
            co.SetData(condolence);

            CreateShowWindowCommand(ScreenTransition.CondolenceOperation());
        }
        /// <summary>
        /// ソートするカラムリストを設定します
        /// </summary>
        public void SetSortColumns()
        {
            Pagination.SortColumns = ReceptsAndExpenditureListSortColumns();
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
            if ((MessageBox = new MessageBoxInfo()
            {
                Message = "データベースに登録し、出力します。よろしいですか？",
                Image = System.Windows.MessageBoxImage.Question,
                Title = "操作確認",
                Button = System.Windows.MessageBoxButton.OKCancel
            }).Result == System.Windows.MessageBoxResult.Cancel)
            {
                return;
            }

            OutputButtonContent = "出力中";
            IsOutputButtonEnabled = false;
            IsClose = false;
            Voucher voucher;
            VoucherRegistration();
            await Task.Run(() => DataOutput.VoucherData(voucher, false, PrepaidDate));
            VoucherAddressee = string.Empty;
            VoucherContents.Clear();
            SetTotalAmount();
            OutputButtonContent = "登録して出力";
            IsClose = true;

            void VoucherRegistration()
            {
                voucher = new Voucher
                    (0, VoucherAddressee, VoucherContents, OutputDate, LoginRep.GetInstance().Rep, true);
                _ = DataBaseConnect.Registration(voucher);
                voucher = DataBaseConnect.CallLatestVoucher();
                voucher.ReceiptsAndExpenditures = VoucherContents;
                foreach (ReceiptsAndExpenditure rae in VoucherContents)
                { _ = DataBaseConnect.Registration(voucher.ID, rae.ID); }
            }
        }
        /// <summary>
        /// 受納証の出納データリストに出納データを追加するコマンド
        /// </summary>
        public DelegateCommand AddVoucherContentCommand { get; set; }
        private async void AddVoucherContent()
        {
            await Task.Delay(1);
            if (SelectedSeachReceiptsAndExpenditure == null) { return; }
            if (VoucherContents.Contains(SelectedSeachReceiptsAndExpenditure)) { return; }
            if (VoucherContents.Count == 8)
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
            string convertText =
                DataBaseConnect.CallContentConvertText(SelectedSeachReceiptsAndExpenditure.Content.ID);
            SelectedSeachReceiptsAndExpenditure.Content.Text =
                convertText ?? SelectedSeachReceiptsAndExpenditure.Content.Text;
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
            _ = VoucherContents.Remove(selectedVoucherContent);
            SetTotalAmount();
            SetOutputEnabled();
        }
        private void SetTotalAmount()
        {
            int i = default;

            foreach (ReceiptsAndExpenditure rae in VoucherContents) { i += rae.Price; }
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
                CallPropertyChanged();
                ValidationProperty(nameof(VoucherAddressee), value);
                SetOutputEnabled();
            }
        }

        private void SetOutputEnabled()
        {
            IsOutputButtonEnabled = VoucherContents.Count != 0 & !string.IsNullOrEmpty(VoucherAddressee);
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
                CallPropertyChanged();
                int i = default;
                foreach (ReceiptsAndExpenditure rae in voucherContents) { i += rae.Price; }
                VoucherTotalAmountDisplayValue = i.ToString();
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
                voucherTotalAmountDisplayValue = CommaDelimitedAmount(value);
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
                CallPropertyChanged();
                CreateReceiptsAndExpenditures(true);
            }
        }
        private void CreateReceiptsAndExpenditures(bool isPageReset)
        {
            Pagination.CountReset(isPageReset);

            (int count, ObservableCollection<ReceiptsAndExpenditure> list) =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                (DefaultDate, DateTime.Today,
                AccountingProcessLocation.Location.ToString(), string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, true, true, true, true, true, SearchDate, SearchDate, DefaultDate,
                DateTime.Today, Pagination.PageCount, Pagination.SelectedSortColumn,
                Pagination.SortDirectionIsASC, Pagination.CountEachPage);

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
        /// <summary>
        /// 出力日
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
        /// <summary>
        /// 事前領収の日付
        /// </summary>
        public DateTime PrepaidDate
        {
            get => prepaidDate;
            set
            {
                prepaidDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 事前領収の日付を受納証に入力するか
        /// </summary>
        public bool IsPrepaid
        {
            get => isPrepaid;
            set
            {
                isPrepaid = value;
                CallPropertyChanged();
                PrepaidDate = value ? DateTime.Today : DefaultDate;
            }
        }
        /// <summary>
        /// ウィンドウを閉じる許可を統括
        /// </summary>
        public bool IsClose
        {
            get => isClose;
            set
            {
                isClose = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(VoucherAddressee):
                    SetNullOrEmptyError(propertyName, (string)value);
                    break;
                default:
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
            ShowVoucherManagementCommand = new DelegateCommand
                (() => ShowVoucherManagement(), () => true);
            ShowCondolenceOperationCommand = new DelegateCommand
                (() => ShowCondolenceOperation(), () => true);
        }

        protected override void SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"受納証作成 : {AccountingProcessLocation.Location}";
        }

        public void ReceiptsAndExpenditureOperationNotify()
        {
            SearchDate = OperationData.Data.AccountActivityDate;
            VoucherContents.Add(OperationData.Data);
            SetTotalAmount();
            SetOutputEnabled();
        }

        public void SortNotify()
        {
            CreateReceiptsAndExpenditures(true);
        }

        public void PageNotify()
        {
            CreateReceiptsAndExpenditures(false);
        }

        public bool OnClosing() { return !IsClose; }

        public int SetCountEachPage() => 10;
    }
}