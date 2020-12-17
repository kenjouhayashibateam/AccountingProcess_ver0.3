using Domain.Entities.Helpers;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 金種クラス
    /// </summary>
    public class MoneyCategory
    {
        /// <summary>
        /// 金種
        /// </summary>
        private readonly int Category;
        private int count;

        /// <summary>
        /// 金種名
        /// </summary>
        public enum Denomination
        {
            /// <summary>
            /// 1円
            /// </summary>
            OneYen,
            /// <summary>
            /// 5円玉
            /// </summary>
            FiveYen,
            /// <summary>
            /// 10円玉
            /// </summary>
            TenYen,
            /// <summary>
            /// 50円玉
            /// </summary>
            FiftyYen,
            /// <summary>
            /// 100円玉
            /// </summary>
            OneHundredYen,
            /// <summary>
            /// 500円玉
            /// </summary>
            FiveHundredYen,
            /// <summary>
            /// 1,000円札
            /// </summary>
            OneThousandYen,
            /// <summary>
            /// 5,000円札
            /// </summary>
            FiveThousandYen,
            /// <summary>
            /// 10,000円札
            /// </summary>
            TenThousandYen,
            /// <summary>
            /// 1円玉束
            /// </summary>
            OneYenBundle,
            /// <summary>
            /// 5円玉束
            /// </summary>
            FiveYenBundle,
            /// <summary>
            /// 10円玉束
            /// </summary>
            TenYenBundle,
            /// <summary>
            /// 50円玉束
            /// </summary>
            FiftyYenBundle,
            /// <summary>
            /// 100円玉束
            /// </summary>
            OneHundredYenBundle,
            /// <summary>
            /// 500円玉束
            /// </summary>
            FiveHundredYenBundle
        }
        /// <summary>
        /// 金種クラスコンストラクタ
        /// </summary>
        /// <param name="category">金種</param>
        public MoneyCategory(int category)
        {
            Category = category;
        }

        /// <summary>
        /// 金銭の枚数
        /// </summary>
        public int Count { get => count; set => count = value; }
        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get => Category * count; }

        /// <summary>
        /// 表示用金額の文字列を返します。
        /// </summary>
        /// <returns>00,000,000 円</returns>
        public string AmountWithUnit() => TextHelper.AmountWithUnit(Amount);
    }
}
