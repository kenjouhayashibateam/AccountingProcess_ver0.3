using System;

namespace Domain.Entities.Helpers
{
    /// <summary>
    /// 文字列の入力補助クラス
    /// </summary>
    public class TextHelper
    {
        public static DateTime DefaultDate = DateTime.Parse("1900/01/01");
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
        /// 金額をIntで返します。単位とカンマを削除しても数字と認識できない場合は0を返します
        /// </summary>
        /// <param name="amount">金額</param>
        /// <returns>000000</returns>
        public static int IntAmount(string amount)
        {
            string s = amount.Replace(Properties.Resources.Unit, string.Empty);
            s = s.Replace(",", string.Empty);
            return CommaDelimitedAmount(s) == string.Empty ? 0 : int.Parse(s);
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
        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string Space = " ";
        /// <summary>
        /// 全角スペース
        /// </summary>
        public const string SpaceF = "　";
    }
}
