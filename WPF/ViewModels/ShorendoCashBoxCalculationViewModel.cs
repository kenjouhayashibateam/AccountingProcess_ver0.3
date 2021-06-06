using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using WPF.ViewModels.Datas;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;

namespace WPF.ViewModels
{
    /// <summary>
    /// 青蓮堂金庫金額計算ウィンドウViewModel
    /// </summary>
    public class ShorendoCashBoxCalculationViewModel : BaseViewModel
    {
        #region Properties
        private string outputButtonText = "出力";
        #region AmountAndCount
        //表示用金額
        private string oneYenAmountWithUnit;
        private string tenThousandYenAmountWithUnit;
        private string fiveThousandYenAmountWithUnit;
        private string oneThousandYenAmountWithUnit;
        private string fiveHundredYenAmountWithUnit;
        private string oneHundredYenAmountWithUnit;
        private string tenYenAmountWithUnit;
        private string fiveYenAmountWithUnit;
        //金銭数量
        private int oneYenCount;
        private int fiveYenCount;
        private string fiftyYenAmountWithUnit;
        private int tenYenCount;
        private int fiftyYenCount;
        private int oneHundredYenCount;
        private int fiveHundredYenCount;
        private int tenThousandYenCount;
        private int fiveThousandYenCount;
        private int oneThousandYenCount;
        //総金額
        private string totalAmount;
        #endregion
        #endregion

        public ShorendoCashBoxCalculationViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
        {
            SetProperty();
        }
        public ShorendoCashBoxCalculationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 各プロパティに値を入力します
        /// </summary>
        private void SetProperty()
        {
            if (Cashbox.GetInstance().GetTotalAmount() == 0) return;
            OneYenCount = Cashbox.GetInstance().MoneyCategorys[OneYen].Count;
            FiveYenCount = Cashbox.GetInstance().MoneyCategorys[FiveYen].Count;
            TenYenCount = Cashbox.GetInstance().MoneyCategorys[TenYen].Count;
            FiftyYenCount = Cashbox.GetInstance().MoneyCategorys[FiftyYen].Count;
            OneHundredYenCount = Cashbox.GetInstance().MoneyCategorys[OneHundredYen].Count;
            FiveHundredYenCount = Cashbox.GetInstance().MoneyCategorys[FiveHundredYen].Count;
            OneThousandYenCount = Cashbox.GetInstance().MoneyCategorys[OneThousandYen].Count;
            FiveThousandYenCount = Cashbox.GetInstance().MoneyCategorys[FiveThousandYen].Count;
            TenThousandYenCount = Cashbox.GetInstance().MoneyCategorys[TenThousandYen].Count;
        }

        /// <summary>
        /// 1万円札の枚数
        /// </summary>
        public int TenThousandYenCount
        {
            get => tenThousandYenCount;
            set
            {
                tenThousandYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[TenThousandYen].Count = tenThousandYenCount;
                TenThousandYenAmountWithUnit =
                    Cashbox.GetInstance().MoneyCategorys[TenThousandYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 5千円札の枚数
        /// </summary>
        public int FiveThousandYenCount
        {
            get => fiveThousandYenCount;
            set
            {
                fiveThousandYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[FiveThousandYen].Count = fiveThousandYenCount;
                FiveThousandYenAmountWithUnit =
                    Cashbox.GetInstance().MoneyCategorys[FiveThousandYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 千円札の枚数
        /// </summary>
        public int OneThousandYenCount
        {
            get => oneThousandYenCount;
            set
            {
                oneThousandYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[OneThousandYen].Count = oneThousandYenCount;
                OneThousandYenAmountWithUnit =
                    Cashbox.GetInstance().MoneyCategorys[OneThousandYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 500円玉の枚数
        /// </summary>
        public int FiveHundredYenCount
        {
            get => fiveHundredYenCount;
            set
            {
                fiveHundredYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[FiveHundredYen].Count = fiveHundredYenCount;
                FiveHundredYenAmountWithUnit =
                    Cashbox.GetInstance().MoneyCategorys[FiveHundredYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 100円玉の枚数
        /// </summary>
        public int OneHundredYenCount
        {
            get => oneHundredYenCount;
            set
            {
                oneHundredYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[OneHundredYen].Count = oneHundredYenCount;
                OneHundredYenAmountWithUnit =
                    Cashbox.GetInstance().MoneyCategorys[OneHundredYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 50円玉の枚数
        /// </summary>
        public int FiftyYenCount
        {
            get => fiftyYenCount;
            set
            {
                fiftyYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[FiftyYen].Count = fiftyYenCount;
                FiftyYenAmountWithUnit = Cashbox.GetInstance().MoneyCategorys[FiftyYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 10円玉の枚数
        /// </summary>
        public int TenYenCount
        {
            get => tenYenCount;
            set
            {
                tenYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[TenYen].Count = tenYenCount;
                TenYenAmountWithUnit = Cashbox.GetInstance().MoneyCategorys[TenYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 5円玉の枚数
        /// </summary>
        public int FiveYenCount
        {
            get => fiveYenCount;
            set
            {
                fiveYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[FiveYen].Count = fiveYenCount;
                FiveYenAmountWithUnit = Cashbox.GetInstance().MoneyCategorys[FiveYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 1円玉の枚数
        /// </summary>
        public int OneYenCount
        {
            get => oneYenCount;
            set
            {
                oneYenCount = value;
                Cashbox.GetInstance().MoneyCategorys[OneYen].Count = oneYenCount;
                OneYenAmountWithUnit = Cashbox.GetInstance().MoneyCategorys[OneYen].AmountWithUnit();
                TotalAmount = Cashbox.GetInstance().GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用一万円札合計金額
        /// </summary>
        public string TenThousandYenAmountWithUnit
        {
            get => tenThousandYenAmountWithUnit;
            set
            {
                tenThousandYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用5千円札合計金額
        /// </summary>
        public string FiveThousandYenAmountWithUnit
        {
            get => fiveThousandYenAmountWithUnit;
            set
            {
                fiveThousandYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用千円札合計金額
        /// </summary>
        public string OneThousandYenAmountWithUnit
        {
            get => oneThousandYenAmountWithUnit;
            set
            {
                oneThousandYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用500円玉合計金額
        /// </summary>
        public string FiveHundredYenAmountWithUnit
        {
            get => fiveHundredYenAmountWithUnit;
            set
            {
                fiveHundredYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用100円玉合計金額
        /// </summary>
        public string OneHundredYenAmountWithUnit
        {
            get => oneHundredYenAmountWithUnit;
            set
            {
                oneHundredYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用10円玉合計金額
        /// </summary>
        public string TenYenAmountWithUnit
        {
            get => tenYenAmountWithUnit;
            set
            {
                tenYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用5円玉合計金額
        /// </summary>
        public string FiveYenAmountWithUnit
        {
            get => fiveYenAmountWithUnit;
            set
            {
                fiveYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 1円玉合計金額
        /// </summary>
        public string OneYenAmountWithUnit
        {
            get => oneYenAmountWithUnit;
            set
            {
                oneYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 50円玉合計金額
        /// </summary>
        public string FiftyYenAmountWithUnit
        {
            get => fiftyYenAmountWithUnit;
            set
            {
                fiftyYenAmountWithUnit = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示用総金額
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
        /// 出力ボタンのテキスト
        /// </summary>
        public string OutputButtonText
        {
            get => outputButtonText;
            set
            {
                outputButtonText = value;
                CallPropertyChanged();
            }
        }
        public override void ValidationProperty(string propertyName, object value)
        { }

        protected override void SetWindowDefaultTitle() =>
            DefaultWindowTitle = $"金庫金額計算 : {AccountingProcessLocation.Location}";

        public override void SetRep(Rep rep)
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle = $"{DefaultWindowTitle}（ログイン : {rep.FirstName}）";
                }
            }
        }
    }
}
