using Domain.Repositories;
using WPF.ViewModels.Commands;
using Domain.Entities;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;
using Infrastructure;
using System.Threading.Tasks;
using System.Collections;

namespace WPF.ViewModels
{
    /// <summary>
    /// 金庫金額計算ビューモデル
    /// </summary>
    public class RemainingMoneyCalculationViewModel : BaseViewModel
    {
        private readonly Cashbox myCashbox = Cashbox.GetInstance();
        private readonly IDataOutput DataOutput;
        public DelegateCommand OutputCommand { get; }
        private bool outputButtonEnabled;
        private string outputButtonText="出力";

        #region AmountAndCount
        //表示用金額
        private string oneYenBundleAmountWithUnit;
        private string fiveYenBundleAmountWithUnit;
        private string tenYenBundleAmountWithUnit;
        private string fiftyYenBundleAmountWithUnit;
        private string oneHundredYenBundleAmountWithUnit;
        private string fiveHundredYenBundleAmountWithUnit;
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
        private int oneYenBundleCount;
        private int fiveYenBundleCount;
        private int tenYenBundleCount;
        private int fiftyYenBundleCount;
        private int oneHundredYenBundleCount;
        private int fiveHundredYenBundleCount;
        //総金額
        private string totalAmount;
        #endregion

        /// <summary>
        /// 1万円札の枚数
        /// </summary>
        public int TenThousandYenCount
        {
            get => tenThousandYenCount;
            set
            {
                tenThousandYenCount = value;
                myCashbox.MoneyCategorys[TenThousandYen].Count = tenThousandYenCount;
                TenThousandYenAmountWithUnit = myCashbox.MoneyCategorys[TenThousandYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[FiveThousandYen].Count = fiveThousandYenCount;
                FiveThousandYenAmountWithUnit = myCashbox.MoneyCategorys[FiveThousandYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[OneThousandYen].Count = oneThousandYenCount;
                OneThousandYenAmountWithUnit = myCashbox.MoneyCategorys[OneThousandYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[FiveHundredYen].Count = fiveHundredYenCount;
                FiveHundredYenAmountWithUnit = myCashbox.MoneyCategorys[FiveHundredYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[OneHundredYen].Count = oneHundredYenCount;
                OneHundredYenAmountWithUnit = myCashbox.MoneyCategorys[OneHundredYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[FiftyYen].Count = fiftyYenCount;
                FiftyYenAmountWithUnit = myCashbox.MoneyCategorys[FiftyYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[TenYen].Count = tenYenCount;
                TenYenAmountWithUnit = myCashbox.MoneyCategorys[TenYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[FiveYen].Count = fiveYenCount;
                FiveYenAmountWithUnit = myCashbox.MoneyCategorys[FiveYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
                myCashbox.MoneyCategorys[OneYen].Count = oneYenCount;
                OneYenAmountWithUnit = myCashbox.MoneyCategorys[OneYen].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
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
        /// 500円玉束数量
        /// </summary>
        public int FiveHundredYenBundleCount
        {
            get => fiveHundredYenBundleCount;
            set
            {
                fiveHundredYenBundleCount = value;
                myCashbox.MoneyCategorys[FiveHundredYenBundle].Count = fiveHundredYenBundleCount;
                FiveHundredYenBundleAmountWithUnit = myCashbox.MoneyCategorys[FiveHundredYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用500円玉束合計金額
        /// </summary>
        public string FiveHundredYenBundleAmountWithUnit
        {
            get => fiveHundredYenBundleAmountWithUnit;
            set
            {
                fiveHundredYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 100円玉束数量
        /// </summary>
        public int OneHundredYenBundleCount
        {
            get => oneHundredYenBundleCount;
            set
            {
                oneHundredYenBundleCount = value;
                myCashbox.MoneyCategorys[OneHundredYenBundle].Count = oneHundredYenBundleCount;
                OneHundredYenBundleAmountWithUnit = myCashbox.MoneyCategorys[OneHundredYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用100円玉束合計金額
        /// </summary>
        public string OneHundredYenBundleAmountWithUnit
        {
            get => oneHundredYenBundleAmountWithUnit;
            set
            {
                oneHundredYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 50円玉束数量
        /// </summary>
        public int FiftyYenBundleCount
        {
            get => fiftyYenBundleCount;
            set
            {
                fiftyYenBundleCount = value;
                myCashbox.MoneyCategorys[FiftyYenBundle].Count = fiftyYenBundleCount;
                FiftyYenBundleAmountWithUnit = myCashbox.MoneyCategorys[FiftyYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用50円玉束合計金額
        /// </summary>
        public string FiftyYenBundleAmountWithUnit
        {
            get => fiftyYenBundleAmountWithUnit;
            set
            {
                fiftyYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 10円玉束数量
        /// </summary>
        public int TenYenBundleCount
        {
            get => tenYenBundleCount;
            set
            {
                tenYenBundleCount = value;
                myCashbox.MoneyCategorys[TenYenBundle].Count = tenYenBundleCount;
                TenYenBundleAmountWithUnit = myCashbox.MoneyCategorys[TenYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用10円玉束合計金額
        /// </summary>
        public string TenYenBundleAmountWithUnit
        {
            get => tenYenBundleAmountWithUnit;
            set
            {
                tenYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 5円玉束数量
        /// </summary>
        public int FiveYenBundleCount
        {
            get => fiveYenBundleCount;
            set
            {
                fiveYenBundleCount = value;
                myCashbox.MoneyCategorys[FiveYenBundle].Count = fiveYenBundleCount;
                FiveYenBundleAmountWithUnit = myCashbox.MoneyCategorys[FiveYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用5円玉束合計金額
        /// </summary>
        public string FiveYenBundleAmountWithUnit
        {
            get => fiveYenBundleAmountWithUnit;
            set
            {
                fiveYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 1円玉束数量
        /// </summary>
        public int OneYenBundleCount
        {
            get => oneYenBundleCount;
            set
            {
                oneYenBundleCount = value;
                myCashbox.MoneyCategorys[OneYenBundle].Count = oneYenBundleCount;
                OneYenBundleAmountWithUnit = myCashbox.MoneyCategorys[OneYenBundle].AmountWithUnit();
                TotalAmount = myCashbox.GetTotalAmountWithUnit();
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用1円玉束合計金額
        /// </summary>
        public string OneYenBundleAmountWithUnit
        {
            get => oneYenBundleAmountWithUnit;
            set
            {
                oneYenBundleAmountWithUnit = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫の金額プロパティをセットします
        /// </summary>
        /// <param name="value">ビューからの文字列</param>
        /// <param name="otherMoneyAmount">金額</param>
        /// <param name="otherMoneyAmountDisplayValue">表示用金額</param>
        private void SetOtherMoneyAmount(string value, int otherMoneyNumber, ref string otherMoneyAmountDisplayValue)
        {
            string s = value.Replace(",", string.Empty);

            otherMoneyAmountDisplayValue = int.TryParse(s, out int i) ? i.ToString("N0") : string.Empty;

            if (otherMoneyAmountDisplayValue != string.Empty)
            {
                myCashbox.OtherMoneys[otherMoneyNumber - 1].Amount = int.Parse(s);
            }
            else
            {
                myCashbox.OtherMoneys[otherMoneyNumber - 1].Amount = 0;
            }
            TotalAmount = myCashbox.GetTotalAmountWithUnit();
        }

        private void SetOtherMontyTitle(string value, int otherMoneyNumber)
        {
            myCashbox.OtherMoneys[otherMoneyNumber - 1].Title = value;
        }

        /// <summary>
        /// その他金庫等1の内容名
        /// </summary>
        public string OtherMoneyTitle1
        {
            get => otherMoneyTitle1;
            set
            {
                otherMoneyTitle1 = value;
                SetOtherMontyTitle(value, 1);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等1　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue1
        {
            get => otherMoneyAmountDisplayValue1;
            set
            {
                SetOtherMoneyAmount(value, 1, ref otherMoneyAmountDisplayValue1);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等2　内容名
        /// </summary>
        public string OtherMoneyTitle2
        {
            get => otherMoneyTitle2;
            set
            {
                otherMoneyTitle2 = value;
                SetOtherMontyTitle(value, 2);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等2　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue2
        {
            get => otherMoneyAmountDisplayValue2;
            set
            {
                SetOtherMoneyAmount(value, 2, ref otherMoneyAmountDisplayValue2);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等3　内容名
        /// </summary>
        public string OtherMoneyTitle3
        {
            get => otherMoneyTitle3;
            set
            {
                otherMoneyTitle3 = value;
                SetOtherMontyTitle(value, 3);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等3　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue3
        {
            get => otherMoneyAmountDisplayValue3;
            set
            {
                SetOtherMoneyAmount(value, 3, ref otherMoneyAmountDisplayValue3);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等4　内容名
        /// </summary>
        public string OtherMoneyTitle4
        {
            get => otherMoneyTitle4;
            set
            {
                otherMoneyTitle4 = value;
                SetOtherMontyTitle(value, 4);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等4　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue4
        {
            get => otherMoneyAmountDisplayValue4;
            set
            {
                SetOtherMoneyAmount(value, 4, ref otherMoneyAmountDisplayValue4);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等5　内容名
        /// </summary>
        public string OtherMoneyTitle5
        {
            get => otherMoneyTitle5;
            set
            {
                otherMoneyTitle5 = value;
                SetOtherMontyTitle(value, 5);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等5　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue5
        {
            get => otherMoneyAmountDisplayValue5;
            set
            {
                SetOtherMoneyAmount(value, 5, ref otherMoneyAmountDisplayValue5);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等6　内容名
        /// </summary>
        public string OtherMoneyTitle6
        {
            get => otherMoneyTitle6;
            set
            {
                otherMoneyTitle6 = value;
                SetOtherMontyTitle(value, 6);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等6　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue6
        {
            get => otherMoneyAmountDisplayValue6;
            set
            {
                SetOtherMoneyAmount(value, 6, ref otherMoneyAmountDisplayValue6);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等7　内容名
        /// </summary>
        public string OtherMoneyTitle7
        {
            get => otherMoneyTitle7;
            set
            {
                otherMoneyTitle7 = value;
                SetOtherMontyTitle(value, 7);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等7　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue7
        {
            get => otherMoneyAmountDisplayValue7;
            set
            {
                SetOtherMoneyAmount(value, 7, ref otherMoneyAmountDisplayValue7);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等8　内容名
        /// </summary>
        public string OtherMoneyTitle8
        {
            get => otherMoneyTitle8;
            set
            {
                otherMoneyTitle8 = value;
                SetOtherMontyTitle(value, 8);
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// その他金庫等8　表示用金額
        /// </summary>
        public string OtherMoneyAmountDisplayValue8
        {
            get => otherMoneyAmountDisplayValue8;
            set
            {
                SetOtherMoneyAmount(value, 8, ref otherMoneyAmountDisplayValue8);
                CallPropertyChanged();
            }
        }

        public bool OutputButtonEnabled
        {
            get => outputButtonEnabled;
            set
            {
                outputButtonEnabled = value;
                CallPropertyChanged();
            }
        }

        public string OutputButtonText
        {
            get => outputButtonText;
            set
            {
                outputButtonText = value;
                CallPropertyChanged();
            }
        }

        public DelegateCommand OtherMoneyContentsClearCommand { get; }
        public DelegateCommand SetOtherMoneyDefaultTitleCommand { get; }

        #region OtherMoneyContents
        private string otherMoneyAmountDisplayValue1;
        private string otherMoneyAmountDisplayValue2;
        private string otherMoneyAmountDisplayValue3;
        private string otherMoneyAmountDisplayValue4;
        private string otherMoneyAmountDisplayValue5;
        private string otherMoneyAmountDisplayValue6;
        private string otherMoneyAmountDisplayValue7;
        private string otherMoneyAmountDisplayValue8;
        private string otherMoneyTitle1;
        private string otherMoneyTitle2;
        private string otherMoneyTitle3;
        private string otherMoneyTitle4;
        private string otherMoneyTitle5;
        private string otherMoneyTitle6;
        private string otherMoneyTitle7;
        private string otherMoneyTitle8;
        #endregion

        public RemainingMoneyCalculationViewModel(IDataOutput dataOutput)
        {
            DataOutput = dataOutput;

            SetProperty();

            OutputCommand = new DelegateCommand(() => Output(), () => true);
            OtherMoneyContentsClearCommand = new DelegateCommand(() => OtherMoneyContentsClear(), () => true);
            SetOtherMoneyDefaultTitleCommand = new DelegateCommand(() => SetOtherMoneyTitleDefault(), () => true);
            OutputButtonEnabled = true;
        }

        private void SetProperty()
        {
            OneYenCount = myCashbox.MoneyCategorys[OneYen].Count;
            FiveYenCount = myCashbox.MoneyCategorys[FiveYen].Count;
            TenYenCount = myCashbox.MoneyCategorys[TenYen].Count;
            FiftyYenCount = myCashbox.MoneyCategorys[FiftyYen].Count;
            OneHundredYenCount = myCashbox.MoneyCategorys[OneHundredYen].Count;
            FiveHundredYenCount = myCashbox.MoneyCategorys[FiveHundredYen].Count;
            OneThousandYenCount = myCashbox.MoneyCategorys[OneThousandYen].Count;
            FiveThousandYenCount = myCashbox.MoneyCategorys[FiveThousandYen].Count;
            TenThousandYenCount = myCashbox.MoneyCategorys[TenThousandYen].Count;

            OneYenBundleCount = myCashbox.MoneyCategorys[OneYenBundle].Count;
            FiveYenBundleCount = myCashbox.MoneyCategorys[FiveYenBundle].Count;
            TenYenBundleCount = myCashbox.MoneyCategorys[TenYenBundle].Count;
            FiftyYenBundleCount = myCashbox.MoneyCategorys[FiftyYenBundle].Count;
            OneHundredYenBundleCount = myCashbox.MoneyCategorys[OneHundredYenBundle].Count;
            FiveHundredYenBundleCount = myCashbox.MoneyCategorys[FiveHundredYenBundle].Count;

            OtherMoneyTitle1 = myCashbox.OtherMoneys[0].Title;
            OtherMoneyAmountDisplayValue1 = myCashbox.OtherMoneys[0].AmountWithUnit();
            OtherMoneyTitle2 = myCashbox.OtherMoneys[1].Title;
            OtherMoneyAmountDisplayValue2 = myCashbox.OtherMoneys[1].AmountWithUnit();
            OtherMoneyTitle3 = myCashbox.OtherMoneys[2].Title;
            OtherMoneyAmountDisplayValue3 = myCashbox.OtherMoneys[2].AmountWithUnit();
            OtherMoneyTitle4 = myCashbox.OtherMoneys[3].Title;
            OtherMoneyAmountDisplayValue4 = myCashbox.OtherMoneys[3].AmountWithUnit();
            OtherMoneyTitle5 = myCashbox.OtherMoneys[4].Title;
            OtherMoneyAmountDisplayValue5 = myCashbox.OtherMoneys[4].AmountWithUnit();
            OtherMoneyTitle6 = myCashbox.OtherMoneys[5].Title;
            OtherMoneyAmountDisplayValue6 = myCashbox.OtherMoneys[5].AmountWithUnit();
            OtherMoneyTitle7 = myCashbox.OtherMoneys[6].Title;
            OtherMoneyAmountDisplayValue7 = myCashbox.OtherMoneys[6].AmountWithUnit();
            OtherMoneyTitle8 = myCashbox.OtherMoneys[7].Title;
            OtherMoneyAmountDisplayValue8 = myCashbox.OtherMoneys[7].AmountWithUnit();
        }

        public void OtherMoneyContentsClear()
        {
            OtherMoneyAmountDisplayValue1 = string.Empty;
            OtherMoneyAmountDisplayValue2 = string.Empty;
            OtherMoneyAmountDisplayValue3 = string.Empty;
            OtherMoneyAmountDisplayValue4 = string.Empty;
            OtherMoneyAmountDisplayValue5 = string.Empty;
            OtherMoneyAmountDisplayValue6 = string.Empty;
            OtherMoneyAmountDisplayValue7 = string.Empty;
            OtherMoneyAmountDisplayValue8 = string.Empty;
            OtherMoneyTitle1 = string.Empty;
            OtherMoneyTitle2 = string.Empty;
            OtherMoneyTitle3 = string.Empty;
            OtherMoneyTitle4 = string.Empty;
            OtherMoneyTitle5 = string.Empty;
            OtherMoneyTitle6 = string.Empty;
            OtherMoneyTitle7 = string.Empty;
            OtherMoneyTitle8 = string.Empty;
        }

        public void SetOtherMoneyTitleDefault()
        {
            OtherMoneyTitle1 = "青蓮堂";
            OtherMoneyTitle2 = "香華売り場";
            OtherMoneyTitle3 = "春秋庵";
            OtherMoneyTitle4 = "石材工事部";
            OtherMoneyTitle5 = string.Empty;
            OtherMoneyTitle6 = string.Empty;
            OtherMoneyTitle7 = string.Empty;
            OtherMoneyTitle8 = string.Empty;
        }

        public RemainingMoneyCalculationViewModel() : this(new ExcelOutputInfrastructure()) { }

        public async void Output()
        {
            OutputButtonEnabled = false;
            OutputButtonText = "出力中";
            await Task.Run(() => DataOutput.CashBoxDataOutput());
            OutputButtonEnabled = true;
            OutputButtonText = "出力";
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}