using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証管理画面ViewModel
    /// </summary>
    public class VoucherManagementViewModel : BaseViewModel, IClosing
    {
        private DateTime searchDateStart = DefaultDate;
        private DateTime searchDateEnd = DefaultDate;
        private ObservableCollection<Voucher> vouchers;
        private ObservableCollection<ReceiptsAndExpenditure> voucherContents;
        public ReceiptsAndExpenditure SelectedReceiptsAndExpenditure;
        private Voucher selectedVoucher;
        private bool isValidity;
        private bool isInputReissueText;
        private bool isValidityTrueOnly = true;
        private bool isOutputButtonEnabled;
        private bool isClose = true;
        private readonly IDataOutput DataOutput;
        private string outputButtonContent = "選択したデータを再発行";

        public VoucherManagementViewModel
            (IDataBaseConnect dataBaseConnect, IDataOutput dataOutput) : base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            SearchDateStart = DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1));
            SearchDateEnd = DateTime.Today;
            IsOutputCheckOperationCommand = new DelegateCommand
                (() => IsOutputCheckOperation(), () => true);
            ReissueVoucherOutputCommand = new DelegateCommand
                (() => ReissueVoucherOutput(), () => true);
        }
        public VoucherManagementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect(),
                DefaultInfrastructure.GetDefaultDataOutput())
        { }
        /// <summary>
        /// 受納証再発行コマンド
        /// </summary>
        public DelegateCommand ReissueVoucherOutputCommand { get; }
        private async void ReissueVoucherOutput()
        {
            if ((MessageBox = new MessageBoxInfo()
            {
                Message = "出力します。よろしいですか？",
                Image = System.Windows.MessageBoxImage.Question,
                Title = "登録確認",
                Button = System.Windows.MessageBoxButton.OKCancel
            }).Result == System.Windows.MessageBoxResult.Cancel) { return; }

            OutputButtonContent = "出力中";
            IsOutputButtonEnabled = false;
            VoucherUpdate();
            try
            {
                await Task.Run(() =>
                    DataOutput.VoucherData(SelectedVoucher, IsInputReissueText, DefaultDate));
            }
            catch (ApplicationException ex)
            {
                DefaultInfrastructure.GetLogger().Log(ILogger.LogInfomation.ERROR, ex.Message);
            }
            OutputButtonContent = "選択したデータを再発行";

            void VoucherUpdate()
            {
                SelectedVoucher.IsValidity = true;
                _ = DataBaseConnect.Update(SelectedVoucher);
            }
        }
        /// <summary>
        /// 出力チェック更新コマンド
        /// </summary>
        public DelegateCommand IsOutputCheckOperationCommand { get; }
        private async void IsOutputCheckOperation()
        {
            await Task.Delay(1);

            MessageBox = new MessageBoxInfo()
            {
                Message = "出力チェックを変更します。よろしいですか？",
                Image = System.Windows.MessageBoxImage.Question,
                Title = "確認",
                Button = System.Windows.MessageBoxButton.YesNo
            };
            CallPropertyChanged(nameof(MessageBox));
            if (MessageBox.Result == System.Windows.MessageBoxResult.No) { return; }
            IsValidity = !IsValidity;
            SelectedVoucher.IsValidity = IsValidity;
            _ = DataBaseConnect.Update(SelectedVoucher);
        }
        /// <summary>
        /// 検索最古日
        /// </summary>
        public DateTime SearchDateStart
        {
            get => searchDateStart;
            set
            {
                searchDateStart = value;
                CreateVoucherList();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索最新日
        /// </summary>
        public DateTime SearchDateEnd
        {
            get => searchDateEnd;
            set
            {
                searchDateEnd = value;
                CreateVoucherList();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 受納証リスト
        /// </summary>
        public ObservableCollection<Voucher> Vouchers
        {
            get => vouchers;
            set
            {
                vouchers = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された受納証
        /// </summary>
        public Voucher SelectedVoucher
        {
            get => selectedVoucher;
            set
            {
                selectedVoucher = value;
                IsOutputButtonEnabled = value != null;
                if (value != null) { VoucherContents = value.ReceiptsAndExpenditures; }
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
        /// 再発行の文字列を入力するか
        /// </summary>
        public bool IsInputReissueText
        {
            get => isInputReissueText;
            set
            {
                isInputReissueText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ValidityがTrueのみ表示するか
        /// </summary>
        public bool IsvalidityTrueOnly
        {
            get => isValidityTrueOnly;
            set
            {
                isValidityTrueOnly = value;
                CreateVoucherList();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択した受納証データの出納データリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> VoucherContents
        {
            get => voucherContents;
            set
            {
                voucherContents = value;
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
        /// 出力ボタンのEnabled
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

        private void CreateVoucherList()
        {
            Vouchers =
                DataBaseConnect.ReferenceVoucher(SearchDateStart, searchDateEnd, IsvalidityTrueOnly);
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(SelectedVoucher):
                    ErrorsListOperation(value == null, propertyName, "受納証データが選択されていません");
                    break;
                default:
                    break;
            }
        }

        protected override void SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"受納証管理 : {AccountingProcessLocation.Location}";
        }

        public bool OnClosing() { return !IsClose; }
    }
}
