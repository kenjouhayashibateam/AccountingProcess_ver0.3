using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 物販売上登録画面ViewModel
    /// </summary>
    public class ProductSalesRegistrationViewModel : JustRegistraterDataViewModel, IClosing
    {
        #region Properties
        #region ints
        private int lighterCount;
        private int beerCount;
        private int sakeCount;
        private int lighterProtecterCount;
        private int tieCount;
        #endregion
        #region strings
        private const string LIGHTER = "ライター";
        private const string BEER = "ビール";
        private const string SAKE = "日本酒";
        private const string LIGHTERPROTECTER = "にぎっ点火";
        private const string TIE = "ネクタイ";
        private string lighterUnitPrice;
        private string lighterTotalAmount;
        private string beerUnitPrice;
        private string beerTotalAmount;
        private string sakeUnitPrice;
        private string sakeTotalAmount;
        private string ligterProtecterUnitPrice;
        private string lighterProtecterTotalAmount;
        private string tieUnitPrice;
        private string tieTotalAmount;
        private string menuTitle;
        public string Lighter => LIGHTER;
        public string Beer => BEER;
        public string Sake => SAKE;
        public string LighterProtecter => LIGHTERPROTECTER;
        public string Tie => TIE;
        private string totalAmount;
        private string operationButtonContent = "登録";
        #endregion
        private DateTime selectedDate = DateTime.Today;
        private bool isRegistrationEnabled;
        private bool isClose = true;
        #endregion

        public ProductSalesRegistrationViewModel(IDataBaseConnect dataBaseConnect) :
            base(dataBaseConnect)
        {
            AccountingSubject accountingSubject = DataBaseConnect.ReferenceAccountingSubject
                (string.Empty, "その他茶所収入", true, true)[0] ?? null;
            MenuTitle = $"{accountingSubject.SubjectCode}：{accountingSubject.Subject}";
            RegistrationCommand = new DelegateCommand(() => Registration(), () => true);
            SetProperty();
        }
        public ProductSalesRegistrationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// データ登録コマンド
        /// </summary>
        public DelegateCommand RegistrationCommand { get; }
        private async void Registration()
        {
            string dataContent = $"{SelectedDate.ToString("ggy年M月d日", JapanCulture)}\r\n\r\n";
            List<ReceiptsAndExpenditure> list = new List<ReceiptsAndExpenditure>();

            if (LighterCount > 0)
            { AddString(LIGHTER, LighterCount, IntAmount(LighterTotalAmount)); }
            if (BeerCount > 0)
            { AddString(BEER, BeerCount, IntAmount(BeerTotalAmount)); }
            if (SakeCount > 0)
            { AddString(SAKE, SakeCount, IntAmount(SakeTotalAmount)); }
            if (LighterProtecterCount > 0) 
            { AddString(LIGHTERPROTECTER, LighterCount, IntAmount(LighterProtecterTotalAmount)); }
            if (TieCount > 0)
            { AddString(TIE, TieCount, IntAmount(TieTotalAmount)); }

            if (CallConfirmationDataOperation(dataContent, "物販") ==
                System.Windows.MessageBoxResult.Cancel) { return; }

            IsRegistrationEnabled = false;
            OperationButtonContent = "登録中";
            IsClose = false;
            await Task.Run(() => DataRegistration());
            CallCompletedRegistration();
            IsRegistrationEnabled = true;
            OperationButtonContent = "登録";
            IsClose = true;

            void DataRegistration()
            {
                foreach (ReceiptsAndExpenditure rae in list)
                { _ = DataBaseConnect.Registration(rae); }
            }

            void AddString(string genre, int count, int amount)
            {
                dataContent += $"{genre}\t{count}{Space}件\t{AmountWithUnit(amount)}\r\n";

                CreditDept creditDept = DataBaseConnect.ReferenceCreditDept(SHUNJUEN, true, true)[0];
                Content content = DataBaseConnect.ReferenceContent
                    (genre, string.Empty, string.Empty, true, true)[0];

                list.Add(new ReceiptsAndExpenditure
                    (0, DateTime.Today, LoginRep.GetInstance().Rep,
                        AccountingProcessLocation.Location.ToString(), creditDept, content, $"{count}{Space}件",
                        amount, true, true, SelectedDate, DefaultDate, false));
            }
        }
        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            LighterUnitPrice = AmountWithUnit(DataBaseConnect.ReferenceContent
                (LIGHTER, string.Empty, string.Empty, true, true)[0].FlatRate);
            LighterCount = 0;
            BeerUnitPrice = AmountWithUnit(DataBaseConnect.ReferenceContent
                (BEER, string.Empty, string.Empty, true, true)[0].FlatRate);
            BeerCount = 0;
            SakeUnitPrice = AmountWithUnit(DataBaseConnect.ReferenceContent
                (SAKE, string.Empty, string.Empty, true, true)[0].FlatRate);
            SakeCount = 0;
            LighterProtecterUnitPrice = AmountWithUnit(DataBaseConnect.ReferenceContent
                (LIGHTERPROTECTER, string.Empty, string.Empty, true, true)[0].FlatRate);
            LighterProtecterCount = 0;
            TieUnitPrice = AmountWithUnit(DataBaseConnect.ReferenceContent
                (TIE, string.Empty, string.Empty, true, true)[0].FlatRate);
            TieCount = 0;
        }
        /// <summary>
        /// ライター個数
        /// </summary>
        public int LighterCount
        {
            get => lighterCount;
            set
            {
                lighterCount = value;
                CallPropertyChanged();
                LighterTotalAmount = AmountWithUnit(value * IntAmount(LighterUnitPrice));
                ValidationProperty(nameof(LighterCount), value);
            }
        }
        /// <summary>
        /// ライター単価
        /// </summary>
        public string LighterUnitPrice
        {
            get => lighterUnitPrice;
            set
            {
                lighterUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ライター小計
        /// </summary>
        public string LighterTotalAmount
        {
            get => lighterTotalAmount;
            set
            {
                lighterTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmount();
            }
        }
        /// <summary>
        /// ビール個数
        /// </summary>
        public int BeerCount
        {
            get => beerCount;
            set
            {
                beerCount = value;
                CallPropertyChanged();
                BeerTotalAmount = AmountWithUnit(value * IntAmount(BeerUnitPrice));
                ValidationProperty(nameof(BeerCount), value);
            }
        }
        /// <summary>
        /// ビール単価
        /// </summary>
        public string BeerUnitPrice
        {
            get => beerUnitPrice;
            set
            {
                beerUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ビール小計
        /// </summary>
        public string BeerTotalAmount
        {
            get => beerTotalAmount;
            set
            {
                beerTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmount();
            }
        }
        /// <summary>
        /// 日本酒単価
        /// </summary>
        public string SakeUnitPrice
        {
            get => sakeUnitPrice;
            set
            {
                sakeUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 日本酒小計
        /// </summary>
        public string SakeTotalAmount
        {
            get => sakeTotalAmount;
            set
            {
                sakeTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmount();
            }
        }
        /// <summary>
        /// 日本酒個数
        /// </summary>
        public int SakeCount
        {
            get => sakeCount;
            set
            {
                sakeCount = value;
                CallPropertyChanged();
                SakeTotalAmount = AmountWithUnit(value * IntAmount(SakeUnitPrice));
                ValidationProperty(nameof(SakeCount), value);
            }
        }
        /// <summary>
        /// にぎっ点火単価
        /// </summary>
        public string LighterProtecterUnitPrice
        {
            get => ligterProtecterUnitPrice;
            set
            {
                ligterProtecterUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// にぎっ点火小計
        /// </summary>
        public string LighterProtecterTotalAmount
        {
            get => lighterProtecterTotalAmount;
            set
            {
                lighterProtecterTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmount();
            }
        }
        /// <summary>
        /// にぎっ点火個数
        /// </summary>
        public int LighterProtecterCount
        {
            get => lighterProtecterCount;
            set
            {
                lighterProtecterCount = value;
                CallPropertyChanged();
                LighterProtecterTotalAmount =
                    AmountWithUnit(value * IntAmount(LighterProtecterUnitPrice));
                ValidationProperty(nameof(LighterProtecterCount), value);
            }
        }
        /// <summary>
        /// ネクタイ個数
        /// </summary>
        public int TieCount
        {
            get => tieCount;
            set
            {
                tieCount = value;
                CallPropertyChanged();
                TieTotalAmount = AmountWithUnit(value * IntAmount(TieUnitPrice));
                ValidationProperty(nameof(TieCount), value);
            }
        }
        /// <summary>
        /// ネクタイ単価
        /// </summary>
        public string TieUnitPrice
        {
            get => tieUnitPrice;
            set
            {
                tieUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ネクタイ小計
        /// </summary>
        public string TieTotalAmount
        {
            get => tieTotalAmount;
            set
            {
                tieTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmount();
            }
        }
        /// <summary>
        /// 選択された日付
        /// </summary>
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                CallPropertyChanged();
                ValidationProperty(nameof(SelectedDate), value);
            }
        }
        /// <summary>
        /// グループボックスのタイトル
        /// </summary>
        public string MenuTitle
        {
            get => menuTitle;
            set
            {
                menuTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録機能のEnabled
        /// </summary>
        public bool IsRegistrationEnabled
        {
            get => isRegistrationEnabled;
            set
            {
                isRegistrationEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 総計金額
        /// </summary>
        public string TotalAmount
        {
            get => totalAmount;
            set
            {
                totalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録ボタンのContent
        /// </summary>
        public string OperationButtonContent
        {
            get => operationButtonContent;
            set
            {
                operationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// フォームを閉じるか
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

        private void SetTotalAmount()
        {
            int i = IntAmount(LighterTotalAmount) + IntAmount(BeerTotalAmount) +
                IntAmount(SakeTotalAmount) + IntAmount(LighterProtecterTotalAmount) +
                IntAmount(TieTotalAmount);
            TotalAmount = $"総計：{AmountWithUnit(i)}";
        }

        private void SetRegistrationButtonEnabled()
        {
            IsRegistrationEnabled =
                LighterCount + BeerCount + SakeCount + LighterProtecterCount + TieCount > 0;
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            if (propertyName == nameof(SelectedDate))
            {
                ErrorsListOperation
                    ((DateTime)value > DateTime.Today, propertyName, "未来のデータは登録できません");
            }
            else
            {
                ErrorsListOperation
                    (!int.TryParse(value.ToString(), out _), propertyName, "数字を入力して下さい。");
            }

            SetRegistrationButtonEnabled();
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = "物販売上登録"; }

        public bool OnClosing() { return !IsClose; }
    }
}
