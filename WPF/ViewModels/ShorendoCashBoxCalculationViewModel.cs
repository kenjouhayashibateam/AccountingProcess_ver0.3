using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;
using static Domain.Entities.ValueObjects.MoneyCategory.Denomination;

namespace WPF.ViewModels
{
    /// <summary>
    /// 青蓮堂金庫金額計算ウィンドウViewModel
    /// </summary>
    class ShorendoCashBoxCalculationViewModel : BaseViewModel
    {
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
        private string totalAmount;
        private readonly Cashbox myCashbox = Cashbox.GetInstance();

        public ShorendoCashBoxCalculationViewModel()
        {
            SetProperty();
        }

        /// <summary>
        /// 各金額プロパティをセットします
        /// </summary>
        /// <param name="value">ビューからの文字列</param>
        /// <param name="otherMoneyAmount">金額</param>
        /// <param name="otherMoneyAmountDisplayValue">表示用金額</param>
        private void SetOtherMoneyAmount(string value, int otherMoneyNumber, ref string otherMoneyAmountDisplayValue)
        {
            otherMoneyAmountDisplayValue = TextHelper.CommaDelimitedAmount(value);

            myCashbox.OtherMoneys[otherMoneyNumber - 1].Amount = otherMoneyAmountDisplayValue != string.Empty ? TextHelper.IntAmount(otherMoneyAmountDisplayValue) : 0;
            TotalAmount = myCashbox.GetTotalAmountWithUnit();
        }

        private void SetProperty()
        {
            if (myCashbox.GetTotalAmount() == 0) return;

            myCashbox.MoneyCategorys[OneYen].Count = 0;
            myCashbox.MoneyCategorys[FiveYen].Count = 0;
            myCashbox.MoneyCategorys[TenYen].Count = 0;
            myCashbox.MoneyCategorys[FiftyYen].Count = 0;
            myCashbox.MoneyCategorys[OneHundredYen].Count = 0;
            myCashbox.MoneyCategorys[FiveHundredYen].Count = 0;
            myCashbox.MoneyCategorys[OneThousandYen].Count = 0;
            myCashbox.MoneyCategorys[FiveThousandYen].Count = 0;
            myCashbox.MoneyCategorys[TenThousandYen].Count = 0;
            myCashbox.MoneyCategorys[OneYenBundle].Count = 0;
            myCashbox.MoneyCategorys[FiveYenBundle].Count = 0;
            myCashbox.MoneyCategorys[TenYenBundle].Count = 0;
            myCashbox.MoneyCategorys[FiftyYenBundle].Count = 0;
            myCashbox.MoneyCategorys[OneHundredYenBundle].Count = 0;
            myCashbox.MoneyCategorys[FiveHundredYenBundle].Count = 0;
        }

        /// <summary>
        /// 金額１
        /// </summary>
        public string OtherMoneyAmountDisplayValue1
        {
            get => otherMoneyAmountDisplayValue1;
            set
            {
                SetOtherMoneyAmount(value, 1, ref otherMoneyAmountDisplayValue1);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額2
        /// </summary>
        public string OtherMoneyAmountDisplayValue2
        {
            get => otherMoneyAmountDisplayValue2;
            set
            {
                SetOtherMoneyAmount(value, 2, ref otherMoneyAmountDisplayValue2);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額3
        /// </summary>
        public string OtherMoneyAmountDisplayValue3
        {
            get => otherMoneyAmountDisplayValue3;
            set
            {
                SetOtherMoneyAmount(value,3, ref otherMoneyAmountDisplayValue3);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額4
        /// </summary>
        public string OtherMoneyAmountDisplayValue4
        {
            get => otherMoneyAmountDisplayValue4;
            set
            {
                SetOtherMoneyAmount(value, 4, ref otherMoneyAmountDisplayValue4);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額5
        /// </summary>
        public string OtherMoneyAmountDisplayValue5
        {
            get => otherMoneyAmountDisplayValue5;
            set
            {
                SetOtherMoneyAmount(value, 5, ref otherMoneyAmountDisplayValue5);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額6
        /// </summary>
        public string OtherMoneyAmountDisplayValue6
        {
            get => otherMoneyAmountDisplayValue6;
            set
            {
                SetOtherMoneyAmount(value, 6, ref otherMoneyAmountDisplayValue6);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額7
        /// </summary>
        public string OtherMoneyAmountDisplayValue7
        {
            get => otherMoneyAmountDisplayValue7;
            set
            {
                SetOtherMoneyAmount(value, 7, ref otherMoneyAmountDisplayValue7);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額8
        /// </summary>
        public string OtherMoneyAmountDisplayValue8
        {
            get => otherMoneyAmountDisplayValue8;
            set
            {
                SetOtherMoneyAmount(value, 8, ref otherMoneyAmountDisplayValue8);
                ValidationProperty(nameof(OtherMoneyAmountDisplayValue1), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// タイトル1
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
        /// タイトル2
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
        /// タイトル3
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
        /// タイトル4
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
        /// タイトル5
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
        /// タイトル6
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
        /// タイトル7
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
        /// タイトル8
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

        public string TotalAmount
        {
            get => totalAmount;
            set
            {
                totalAmount = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(OtherMoneyTitle1):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle1) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue1), nameof(OtherMoneyAmountDisplayValue1), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle2):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle2) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue2), nameof(OtherMoneyAmountDisplayValue2), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle3):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle3) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue3), nameof(OtherMoneyAmountDisplayValue3), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle4):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle4) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue4), nameof(OtherMoneyAmountDisplayValue4), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle5):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle5) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue5), nameof(OtherMoneyAmountDisplayValue5), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle6):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle6) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue6), nameof(OtherMoneyAmountDisplayValue6), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle7):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle7) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue7), nameof(OtherMoneyAmountDisplayValue7), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyTitle8):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyTitle8) & string.IsNullOrEmpty(OtherMoneyAmountDisplayValue8), nameof(OtherMoneyAmountDisplayValue8), "金額を入力して下さい");
                    break;
                case nameof(OtherMoneyAmountDisplayValue1):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue1) & string.IsNullOrEmpty(OtherMoneyTitle1), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue2):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue2) & string.IsNullOrEmpty(OtherMoneyTitle2), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue3):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue3) & string.IsNullOrEmpty(OtherMoneyTitle3), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue4):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue4) & string.IsNullOrEmpty(OtherMoneyTitle4), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue5):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue5) & string.IsNullOrEmpty(OtherMoneyTitle5), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue6):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue6) & string.IsNullOrEmpty(OtherMoneyTitle6), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue7):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue7) & string.IsNullOrEmpty(OtherMoneyTitle7), propertyName, "内容を入力してください");
                    break;
                case nameof(OtherMoneyAmountDisplayValue8):
                    ErrorsListOperation(!string.IsNullOrEmpty(OtherMoneyAmountDisplayValue8) & string.IsNullOrEmpty(OtherMoneyTitle8), propertyName, "内容を入力してください");
                    break;
                default:
                    break;
            }
        }

        protected override string SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"金庫金額計算 : {AccountingProcessLocation.Location}";
            return DefaultWindowTitle;
        }
    }
}
