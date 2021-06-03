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

namespace WPF.ViewModels
{
    /// <summary>
    /// 受納証管理画面ViewModel
    /// </summary>
    public class VoucherManagementViewModel : BaseViewModel
    {
        private DateTime searchDateStart;
        private DateTime searchDateEnd;
        private ObservableCollection<Voucher> vouchers;
        private Voucher selectedVoucher;
        private bool isValidity;
        private bool isInputReissueText;
        private bool isValidityTrueOnly = true;

        public VoucherManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            SearchDateStart = DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1));
            SearchDateEnd = DateTime.Today;
            IsOutputCheckOperationCommand = new DelegateCommand(() => IsOutputCheckOperation(), () => true);
        }
        public VoucherManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }

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
            if (MessageBox.Result == System.Windows.MessageBoxResult.No) return;
            IsValidity = !IsValidity;
            SelectedVoucher.IsValidity = IsValidity;
            DataBaseConnect.Update(SelectedVoucher);
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

        private void CreateVoucherList() =>
            Vouchers = DataBaseConnect.ReferenceVoucher(SearchDateStart, searchDateEnd, IsvalidityTrueOnly);        

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle =
                    $"{DefaultWindowTitle}（ログイン : {rep.FirstName}）";
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
        }

        protected override void SetWindowDefaultTitle() =>
            DefaultWindowTitle = $"受納証管理 : {AccountingProcessLocation.Location}";
    }
}
