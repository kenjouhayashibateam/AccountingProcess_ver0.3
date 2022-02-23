using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

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
        private int stockingCount;
        private int nosiBagCount;
        private int candleCount;
        #endregion
        #region strings
        private const string LIGHTER = "ライター";
        private const string BEER = "ビール";
        private const string SAKE = "日本酒";
        private const string LIGHTERPROTECTER = "にぎっ点火";
        private const string TIE = "ネクタイ";
        private const string STOCKING = "ストッキング";
        private const string NOSIBAG = "のし袋";
        private const string CANDLE = "ろうそく";
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
        private string stockingUnitPrice;
        private string stockingTotalAmount;
        private string nosiBagUnitPrice;
        private string nosiBagTotalAmount;
        private string candleUnitPrice;
        private string candleTotalAmount;
        private string menuTitle;
        private string lighter;
        private string beer;
        private string sake;
        private string lighterProtecter;
        private string tie;
        private string stocking;
        private string nosiBag;
        private string candle;
        private string totalAmount;
        private string operationButtonContent = "登録";
        private string InvalidContent = string.Empty;
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
            CallContentsWarningMessageCommand = new DelegateCommand
                (() => CallContentsWarningMessage(), () => true);
            SetProperty();
        }
        public ProductSalesRegistrationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 一覧の内容に不足があった場合に警告を発するコマンド
        /// </summary>
        public DelegateCommand CallContentsWarningMessageCommand { get; }
        private void CallContentsWarningMessage()
        {
            if (InvalidContent.Length == 0) { return; }

            MessageBox = new MessageBoxInfo()
            {
                Message = $"{InvalidContent}\r\nのデータが不足しています。先に登録を済ませて下さい。",
                Title = "データ不足警告",
                Image = MessageBoxImage.Warning,
                Button = MessageBoxButton.OK
            };
            CallShowMessageBox = true;
        }
        /// <summary>
        /// データ登録コマンド
        /// </summary>
        public DelegateCommand RegistrationCommand { get; }
        private async void Registration()
        {
            string dataContent = $"{SelectedDate.ToString("ggy年M月d日", JapanCulture)}\r\n\r\n";
            List<ReceiptsAndExpenditure> list = new List<ReceiptsAndExpenditure>();
            InvalidContent = string.Empty;

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
            if (StockingCount > 0)
            { AddString(STOCKING, StockingCount, IntAmount(StockingTotalAmount)); }
            if (NosiBagCount > 0)
            { AddString(NOSIBAG, NosiBagCount, IntAmount(NosiBagTotalAmount)); }
            if (CandleCount > 0)
            { AddString(CANDLE, CandleCount, IntAmount(CandleTotalAmount)); }

            if (CallConfirmationDataOperation(dataContent, "物販") == MessageBoxResult.Cancel)
            { return; }

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
                dataContent += $"{genre}{(genre != STOCKING ? "\t" : string.Empty)}\t" +
                    $"{AmountWithUnit(amount)}\r\n";

                CreditDept creditDept = DataBaseConnect.ReferenceCreditDept(SHUNJUEN, true, true)[0];
                Content content = DataBaseConnect.ReferenceContent
                    (genre, string.Empty, string.Empty, true, true)[0];

                list.Add(new ReceiptsAndExpenditure
                    (0, DateTime.Today, LoginRep.GetInstance().Rep,
                        AccountingProcessLocation.Location.ToString(), creditDept, content, string.Empty,
                        amount, true, true, SelectedDate, DefaultDate, false));
            }
        }
        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            Content content;

            LighterUnitPrice = ReturnContentFlatRateWithUnit(LIGHTER);
            if (content != null) { Lighter = content.Text; }
            LighterCount = 0;
            BeerUnitPrice = ReturnContentFlatRateWithUnit(BEER);
            if (content != null) { Beer = content.Text; }
            BeerCount = 0;
            SakeUnitPrice = ReturnContentFlatRateWithUnit(SAKE);
            if (content != null) { Sake = content.Text; }
            SakeCount = 0;
            LighterProtecterUnitPrice = ReturnContentFlatRateWithUnit(LIGHTERPROTECTER);
            if (content != null) { LighterProtecter = content.Text; }
            LighterProtecterCount = 0;
            TieUnitPrice = ReturnContentFlatRateWithUnit(TIE);
            if (content != null) { Tie = content.Text; }
            TieCount = 0;
            StockingUnitPrice = ReturnContentFlatRateWithUnit(STOCKING);
            if (content != null) { Stocking = content.Text; }
            StockingCount = 0;
            NosiBagUnitPrice = ReturnContentFlatRateWithUnit(NOSIBAG);
            if (content != null) { NosiBag = content.Text; }
            NosiBagCount = 0;
            CandleUnitPrice = ReturnContentFlatRateWithUnit(CANDLE);
            if (content != null) { Candle = content.Text; }
            CandleCount = 0;

            string ReturnContentFlatRateWithUnit(string genre)
            {
                content = null;
                ObservableCollection<Content> list = DataBaseConnect.ReferenceContent
                    (genre, string.Empty, string.Empty, true, true);

                if (list.Count > 0) { content = list[0]; }
                else { InvalidContent += $"{genre}\r\n"; }

                return content == null ? string.Empty : AmountWithUnit(ValidationFlatRate());

                int ValidationFlatRate()
                {
                    if (content.FlatRate <= 0) { InvalidContent += $"{genre}{Space}の定額\r\n"; }

                    return content.FlatRate <= 0 ? 0 : content.FlatRate;
                }
            }
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
                SetTotalAmountAndOperaionEnabled();
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
                SetTotalAmountAndOperaionEnabled();
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
                SetTotalAmountAndOperaionEnabled();
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
                SetTotalAmountAndOperaionEnabled();
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
                SetTotalAmountAndOperaionEnabled();
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
        /// <summary>
        /// ストッキング個数
        /// </summary>
        public int StockingCount
        {
            get => stockingCount;
            set
            {
                stockingCount = value;
                CallPropertyChanged();
                StockingTotalAmount = AmountWithUnit(IntAmount(StockingUnitPrice) * value);
            }
        }
        /// <summary>
        /// のし袋個数
        /// </summary>
        public int NosiBagCount
        {
            get => nosiBagCount;
            set
            {
                nosiBagCount = value;
                CallPropertyChanged();
                NosiBagTotalAmount = AmountWithUnit(IntAmount(NosiBagUnitPrice) * value);
            }
        }
        /// <summary>
        /// 蝋燭個数
        /// </summary>
        public int CandleCount
        {
            get => candleCount;
            set
            {
                candleCount = value;
                CallPropertyChanged();
                CandleTotalAmount = AmountWithUnit(IntAmount(CandleUnitPrice) * value);
            }
        }
        /// <summary>
        /// ストッキング単価
        /// </summary>
        public string StockingUnitPrice
        {
            get => stockingUnitPrice;
            set
            {
                stockingUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ストッキング小計
        /// </summary>
        public string StockingTotalAmount
        {
            get => stockingTotalAmount;
            set
            {
                stockingTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmountAndOperaionEnabled();
            }
        }
        /// <summary>
        /// のし袋単価
        /// </summary>
        public string NosiBagUnitPrice
        {
            get => nosiBagUnitPrice;
            set
            {
                nosiBagUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// のし袋小計
        /// </summary>
        public string NosiBagTotalAmount
        {
            get => nosiBagTotalAmount;
            set
            {
                nosiBagTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmountAndOperaionEnabled();
            }
        }
        /// <summary>
        /// 蝋燭単価
        /// </summary>
        public string CandleUnitPrice
        {
            get => candleUnitPrice;
            set
            {
                candleUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 蝋燭小計
        /// </summary>
        public string CandleTotalAmount
        {
            get => candleTotalAmount;
            set
            {
                candleTotalAmount = value;
                CallPropertyChanged();
                SetTotalAmountAndOperaionEnabled();
            }
        }
        /// <summary>
        /// ライター
        /// </summary>
        public string Lighter
        {
            get => lighter;
            set
            {
                lighter = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ビール
        /// </summary>
        public string Beer
        {
            get => beer;
            set
            {
                beer = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 日本酒
        /// </summary>
        public string Sake
        {
            get => sake;
            set
            {
                sake = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// にぎっ点火
        /// </summary>
        public string LighterProtecter
        {
            get => lighterProtecter;
            set
            {
                lighterProtecter = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ネクタイ
        /// </summary>
        public string Tie
        {
            get => tie;
            set
            {
                tie = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ストッキング
        /// </summary>
        public string Stocking
        {
            get => stocking;
            set
            {
                stocking = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// のし袋
        /// </summary>
        public string NosiBag
        {
            get => nosiBag;
            set
            {
                nosiBag = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 蝋燭
        /// </summary>
        public string Candle
        {
            get => candle;
            set
            {
                candle = value;
                CallPropertyChanged();
            }
        }

        private void SetTotalAmountAndOperaionEnabled()
        {
            int i = IntAmount(LighterTotalAmount) + IntAmount(BeerTotalAmount) +
                IntAmount(SakeTotalAmount) + IntAmount(LighterProtecterTotalAmount) +
                IntAmount(TieTotalAmount) + IntAmount(StockingTotalAmount) +
                IntAmount(NosiBagTotalAmount) + IntAmount(CandleTotalAmount);

            TotalAmount = $"総計：{AmountWithUnit(i)}";
            IsRegistrationEnabled = i > 0;
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

            SetTotalAmountAndOperaionEnabled();
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = "物販売上登録"; }

        public bool CancelClose() { return !IsClose; }
    }
}
