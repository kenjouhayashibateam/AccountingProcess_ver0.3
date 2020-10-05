using Domain;
using Domain.Repositories;
using WPF.ViewModels.Commands;
using Domain.Entities;
using static Domain.Entities.ValueObjects.MoneyCategory;
using Infrastructure;
using Microsoft.VisualBasic;

namespace WPF.ViewModels
{
    /// <summary>
    /// 金庫金額計算ビューモデル
    /// </summary>
    public class RemainingMoneyCalculationViewModel : BaseViewModel
    {
        private readonly Cashbox myCashBox = new Cashbox();
        private readonly IDataOutput DataOutput;
        public DelegateCommand OutputCommand { get; }

        #region
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
                myCashBox.MoneyCategorys[Denomination.TenThousandYen].Count = tenThousandYenCount;
                TenThousandYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.TenThousandYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiveThousandYen].Count = fiveThousandYenCount;
                FiveThousandYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiveThousandYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.OneThousandYen].Count = oneThousandYenCount;
                OneThousandYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.OneThousandYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiveHundredYen].Count = fiveHundredYenCount;
                FiveHundredYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiveHundredYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.OneHundredYen].Count = oneHundredYenCount;
                OneHundredYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.OneHundredYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiftyYen].Count = fiftyYenCount;
                FiftyYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiftyYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.TenYen].Count = tenYenCount;
                TenYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.TenYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiveYen].Count = fiveYenCount;
                FiveYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiveYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.OneYen].Count = oneYenCount;
                OneYenAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.OneYen].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 表示用金額の文字列を返します。
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns></returns>
        private string ReturnAmountWithUnit(int amount)
        {
            return $"{amount:N0} {Properties.Resources.Unit}";
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
                myCashBox.MoneyCategorys[Denomination.FiveHundredYenBundle].Count = fiveHundredYenBundleCount;
                FiveHundredYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiveHundredYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.OneHundredYenBundle].Count = oneHundredYenBundleCount;
                OneHundredYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.OneHundredYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiftyYenBundle].Count = fiftyYenBundleCount;
                FiftyYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiftyYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.TenYenBundle].Count = tenYenBundleCount;
                TenYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.TenYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.FiveYenBundle].Count = fiveYenBundleCount;
                FiveYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.FiveYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
                myCashBox.MoneyCategorys[Denomination.OneYenBundle].Count = oneYenBundleCount;
                OneYenBundleAmountWithUnit = ReturnAmountWithUnit(myCashBox.MoneyCategorys[Denomination.OneYenBundle].Amount);
                TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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
            otherMoneyAmountDisplayValue = int.TryParse(value, out int i) ? i.ToString("N0") : string.Empty;
            myCashBox.OtherMoneys[otherMoneyNumber - 1] = i;
            TotalAmount = ReturnAmountWithUnit(myCashBox.GetTotalAmount());
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

        #region
        private string otherMoneyAmountDisplayValue1;
        private string otherMoneyAmountDisplayValue2;
        private string otherMoneyAmountDisplayValue3;
        private string otherMoneyAmountDisplayValue4;
        private string otherMoneyAmountDisplayValue5;
        private string otherMoneyAmountDisplayValue6;
        private string otherMoneyAmountDisplayValue7;
        private string otherMoneyAmountDisplayValue8;
        private string otherMoneyTitle1 = "青蓮堂";
        private string otherMoneyTitle2 = "香華売り場";
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

            //引数()=>{}の意味　小辻さんに質問する
            OutputCommand = new DelegateCommand(() => { Output(); }, () => { return true; });
        }

        public RemainingMoneyCalculationViewModel() : this(new ExcelOutputInfrastructure()) { }

        public void Output()
        {
            DataOutput.CashBoxDataOutput(myCashBox);
        }

    }
}
