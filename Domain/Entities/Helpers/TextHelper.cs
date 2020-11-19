
namespace Domain.Entities.Helpers
{
    /// <summary>
    /// 文字列の入力補助クラス
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにして、単位をつけて返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000 円</returns>
        public static string AmountWithUnit(int amount)
        {
            return $"{CommaDelimitedAmount(amount)} {Properties.Resources.Unit}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(int amount)
        {
            return $"{amount:N0}";
        }
        /// <summary>
        /// 金額を3桁ごとのカンマ区切りにした文字列を返します。数字と認識できない場合はEmptyを返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>00,000,000</returns>
        public static string CommaDelimitedAmount(string amount)
        {
            string s = amount.Replace(",", string.Empty);
            return int.TryParse(s, out int i) ? CommaDelimitedAmount(i) : string.Empty;
        }
        /// <summary>
        /// 金額をIntで返します。数字と認識できない場合は0を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>000000</returns>
        public static int IntAmount(string amount)
        {
            return CommaDelimitedAmount(amount) == string.Empty ? 0 : int.Parse(amount.Replace(",", string.Empty));
        }
        /// <summary>
        /// 名前の最初のスペースまでの文字列を返します
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>苗字</returns>
        public static string GetFirstName(string name)
        {
            string[] nameArray = name.Split(' ');

            return nameArray[0];
        }
    }
}
